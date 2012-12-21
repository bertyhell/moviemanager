using System;
using Model;
using Tmc.DataAccess.Sqlite.DsDatabaseVersionTableAdapters;
using System.Data.SqlServerCe;

namespace Tmc.DataAccess.Sqlite
{
    public class TmcDatabaseCreation
    {
        public static readonly int CURRENT_DATABASE_VERSION = 1;
        private static SqlCeConnection _conn;

        public static void Init(string connectionString)
        {
            _conn = Database.GetConnection(connectionString);
        }

        //public static event VideosChanged OnVideosChanged;
        //don't use videos changed -> all database operations could be run in different thread -> different trhead has no access to observable collection
        //update lists in maincontroller in commandobjects

        public static bool ConvertDatabase(string connectionString)
        {
            Init(connectionString);
            bool Retval = true;
            DatabaseDetails Details;
            try { Details = GetDatabaseDetails(); }
            catch
            {
                Details = new DatabaseDetails { DatabaseVersion = 1, RequiredVersion = CURRENT_DATABASE_VERSION };
                Retval &= CreateDatabase();
            }

            if (Details.DatabaseVersion == CURRENT_DATABASE_VERSION)
                return Retval;

            if (Retval && Details.DatabaseVersion < 2)
            {
                Details.DatabaseVersion = 1;
                Details.VersionRecords.Add(new DatabaseVersionRecord { Id = -1, Description = "new tables", Timestamp = DateTime.Now, Version = 2 });
                Retval &= CreateTablesv002();
                Retval &= AlterTablesv002();
                Retval &= AddDefaultValuesv002();
            }

            Retval &= UpdateDatabaseDetails(Details);

            return Retval;


        }


        public static bool CreateDatabase()
        {
            bool Retval;

            try
            {
                string SqlQuery =
                    "CREATE TABLE Genres ( gen_id INTEGER PRIMARY KEY AUTOINCREMENT, gen_label TEXT NOT NULL UNIQUE )";
                Database.ExecuteSql(SqlQuery);

                SqlQuery = "CREATE TABLE Franchises (id INTEGER PRIMARY KEY AUTOINCREMENT, name VARCHAR NOT NULL)";
                Database.ExecuteSql(SqlQuery);

                SqlQuery = "CREATE TABLE Series (id INTEGER PRIMARY KEY AUTOINCREMENT, name VARCHAR(255) NOT NULL)";
                Database.ExecuteSql(SqlQuery);

                SqlQuery =
                    "CREATE TABLE Videos ( id INTEGER PRIMARY KEY AUTOINCREMENT, id_imdb VARCHAR(10) DEFAULT NULL, name VARCHAR(255) NOT NULL, release DATE, play_count INTEGER DEFAULT 0 NOT NULL, "
                    + "rating DOUBLE DEFAULT -1 NOT NULL, rating_imdb DOUBLE DEFAULT -1 NOT NULL, path VARCHAR(255), last_play_location INTEGER NOT NULL default 0, runtime TIME,"
                    + "play_count INTEGER DEFAULT 0 NOT NULL, poster VARCHAR(255) )";
                Database.ExecuteSql(SqlQuery);

                SqlQuery = "CREATE TABLE Movies ( id INTEGER PRIMARY KEY, franchise_id INTEGER, id_tmdb INTEGER," +
                           " FOREIGN KEY(id) REFERENCES Videos(id)" +
                           " FOREIGN KEY(franchise_id) REFERENCES Franchises(id))";
                Database.ExecuteSql(SqlQuery);

                SqlQuery =
                    "CREATE TABLE Episodes ( id INTEGER PRIMARY KEY, serie_id INTEGER NOT NULL, season INTEGER NOT NULL, episode_number," +
                    " FOREIGN KEY(id) REFERENCES Videos(id)" +
                    " FOREIGN KEY(serie_id) REFERENCES Series(id))";
                Database.ExecuteSql(SqlQuery);

                SqlQuery = "CREATE TABLE Videos_genres ( video_id INTEGER NOT NULL, genre_id INTEGER NOT NULL," +
                           "UNIQUE (video_id, genre_id) ON CONFLICT ABORT, " +
                           "FOREIGN KEY(video_id) REFERENCES Videos(id)," +
                           "FOREIGN KEY(genre_id) REFERENCES Genres(gen_id))";
                Database.ExecuteSql(SqlQuery);

                SqlQuery =
                    "CREATE TABLE Database_version ( id INTEGER PRIMARY KEY AUTOINCREMENT, version INTEGER NOT NULL, timestamp DATETIME default current_timestamp , description VARCHAR(255) )";
                Database.ExecuteSql(SqlQuery);


                SqlQuery = "CREATE TABLE Video_folders ( id INTEGER PRIMARY KEY AUTOINCREMENT, path VARCHAR(255) NOT NULL UNIQUE, auto_update INTEGER DEFAULT 0 )";
                Database.ExecuteSql(SqlQuery);

                AddDefaultValuesVersion001();

                Retval = true;
            }
            catch
            {
                Retval = false;
            }
            return Retval;
        }

        private static void AddDefaultValuesVersion001()
        {
            const string SQL_QUERY = "INSERT INTO Database_version (version, description) values (1, 'table creation')";
            Database.ExecuteSql(SQL_QUERY);
        }

        private static bool CreateTablesv002()
        {
            return true;
        }

        private static bool AlterTablesv002()
        {
            return true;
        }

        private static bool AddDefaultValuesv002()
        {
            return true;
        }

        public static DatabaseDetails GetDatabaseDetails()
        {

            DatabaseDetails RetVal = new DatabaseDetails();
            DsDatabaseVersion DataSet = new DsDatabaseVersion();

            Database_versionTableAdapter DatabaseVersionTableAdapter = new Database_versionTableAdapter{Connection = _conn};

            try
            {
                DatabaseVersionTableAdapter.Fill(DataSet.Database_version);
            }
            catch
            {
                //database version table doesn't exist
                //create database
                //TODO 040 make backup of old database if not empty
                //TODO 030 make a database backup manager

                CreateDatabase();
                
            }
            

            int DatabaseVersion = 0;
            foreach (DsDatabaseVersion.Database_versionRow DatabaseVersionRow in DataSet.Database_version.Rows)
            {
                if (DatabaseVersionRow.version > DatabaseVersion)
                    DatabaseVersion = DatabaseVersionRow.version;

                RetVal.VersionRecords.Add(new DatabaseVersionRecord
                {
                    Id = DatabaseVersionRow.id,
                    Version = DatabaseVersionRow.version,
                    Description = DatabaseVersionRow.description,
                    Timestamp = DatabaseVersionRow.timestamp

                });
            }
            RetVal.DatabaseVersion = DatabaseVersion;
            RetVal.RequiredVersion = CURRENT_DATABASE_VERSION;


            return RetVal;
        }

        private static bool UpdateDatabaseDetails(DatabaseDetails details)
        {
            bool RetVal = true;

            try
            {

                DsDatabaseVersion DataSet = new DsDatabaseVersion();
                Database_versionTableAdapter DatabaseVersionTableAdapter = new Database_versionTableAdapter();
                DatabaseVersionTableAdapter.Fill(DataSet.Database_version);

                foreach (DatabaseVersionRecord DatabaseVersionRecord in details.VersionRecords)
                {
                    DsDatabaseVersion.Database_versionRow Row = null;
                    if (DatabaseVersionRecord.Id != -1)
                        Row = DataSet.Database_version.FindByid(DatabaseVersionRecord.Id);

                    bool AddRow = false;

                    if (Row == null)
                    {
                        Row = DataSet.Database_version.NewDatabase_versionRow();
                        AddRow = true;
                    }

                    if (Row != null)
                    {
                        Row.version = DatabaseVersionRecord.Version;
                        Row.timestamp = DatabaseVersionRecord.Timestamp;
                        Row.description = DatabaseVersionRecord.Description;

                        if (AddRow)
                            DataSet.Database_version.AddDatabase_versionRow(Row);

                        DatabaseVersionTableAdapter.Update(DataSet);
                    }
                }
            }
            catch
            {
                RetVal = false;
            }

            return RetVal;
        }
    }
}
