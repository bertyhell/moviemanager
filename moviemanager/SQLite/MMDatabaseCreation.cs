using System;
using System.Data.SQLite;
using Model;
using SQLite.dsDatabaseVersionTableAdapters;

namespace SQLite
{
    public class MMDatabaseCreation
    {
        private static readonly int CURRENT_DATABASE_VERSION = 1;
        private static SQLiteConnection _conn;




        public static void Init(string connectionString)
        {
            _conn = Database.GetConnection(connectionString);
        }


        //public static event VideosChanged OnVideosChanged;
        //don't use videos changed -> all database operations could be run in different thread -> different trhead has no access to observable collection
        //update lists in maincontroller in commandobjects

        public static bool ConvertDatabase(string connectionString)
        {
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

            UpdateDatabaseDetails(Details);

            return Retval;


        }


        public static bool CreateDatabase()
        {
            bool Retval;

            try
            {
                string SQLQuery =
                    "CREATE TABLE Genres ( gen_id INTEGER PRIMARY KEY AUTOINCREMENT, gen_label TEXT NOT NULL UNIQUE )";
                Database.ExecuteSQL(SQLQuery);

                SQLQuery = "CREATE TABLE Franchises (id INTEGER PRIMARY KEY AUTOINCREMENT, name VARCHAR NOT NULL)";
                Database.ExecuteSQL(SQLQuery);

                SQLQuery = "CREATE TABLE Series (id INTEGER PRIMARY KEY AUTOINCREMENT, name VARCHAR(255) NOT NULL)";
                Database.ExecuteSQL(SQLQuery);

                SQLQuery =
                    "CREATE TABLE Videos ( id INTEGER PRIMARY KEY AUTOINCREMENT, id_imdb VARCHAR(10), name VARCHAR(255) NOT NULL, release DATE, rating DOUBLE, rating_imdb DOUBLE, path VARCHAR(255), last_play_location INTEGER default 0, runtime TIME, PlayCount INTEGER DEFAULT 0)";
                Database.ExecuteSQL(SQLQuery);

                SQLQuery = "CREATE TABLE Movies ( id INTEGER PRIMARY KEY, franchise_id INTEGER, id_tmdb INTEGER," +
                           " FOREIGN KEY(id) REFERENCES Videos(id)" +
                           " FOREIGN KEY(franchise_id) REFERENCES Franchises(id))";
                Database.ExecuteSQL(SQLQuery);

                SQLQuery =
                    "CREATE TABLE Episodes ( id INTEGER PRIMARY KEY, serie_id INTEGER NOT NULL, season INTEGER NOT NULL, episode_number," +
                    " FOREIGN KEY(id) REFERENCES Videos(id)" +
                    " FOREIGN KEY(serie_id) REFERENCES Series(id))";
                Database.ExecuteSQL(SQLQuery);

                SQLQuery = "CREATE TABLE Videos_genres ( video_id INTEGER NOT NULL, genre_id INTEGER NOT NULL," +
                           "UNIQUE (video_id, genre_id) ON CONFLICT ABORT, " +
                           "FOREIGN KEY(video_id) REFERENCES Videos(id)," +
                           "FOREIGN KEY(genre_id) REFERENCES Genres(gen_id))";
                Database.ExecuteSQL(SQLQuery);

                SQLQuery =
                    "CREATE TABLE Database_version ( id INTEGER PRIMARY KEY AUTOINCREMENT, version INTEGER NOT NULL, timestamp DATETIME default current_timestamp , description VARCHAR(255) )";
                Database.ExecuteSQL(SQLQuery);

                AddDefaultValuesVersion001(_conn);

                Retval = true;
            }
            catch
            {
                Retval = false;
            }
            return Retval;
        }

        private static void AddDefaultValuesVersion001(SQLiteConnection conn)
        {
            const string SQL_QUERY = "INSERT INTO Database_version (version, description) values (1, 'table creation')";
            Database.ExecuteSQL(SQL_QUERY);
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
            dsDatabaseVersion DataSet = new dsDatabaseVersion();

            Database_versionTableAdapter DatabaseVersionTableAdapter = new Database_versionTableAdapter{Connection = _conn};

            try
            {
                DatabaseVersionTableAdapter.Fill(DataSet.Database_version);
            }
            catch (SQLiteException ex)
            {
                //database version table doesn't exist
                //create database
                //TODO 040 make backup of old database if not empty
                //TODO 030 make a database backup manager

                CreateDatabase();
                
            }
            

            int DatabaseVersion = 0;
            foreach (dsDatabaseVersion.Database_versionRow DatabaseVersionRow in DataSet.Database_version.Rows)
            {
                if (DatabaseVersionRow.version > DatabaseVersion)
                    DatabaseVersion = (int)DatabaseVersionRow.version;

                RetVal.VersionRecords.Add(new DatabaseVersionRecord
                {
                    Id = (int)DatabaseVersionRow.id,
                    Version = (int)DatabaseVersionRow.version,
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
            bool RetVal = false;

            try
            {

                dsDatabaseVersion DataSet = new dsDatabaseVersion();
                Database_versionTableAdapter DatabaseVersionTableAdapter = new Database_versionTableAdapter();
                DatabaseVersionTableAdapter.Fill(DataSet.Database_version);

                foreach (DatabaseVersionRecord DatabaseVersionRecord in details.VersionRecords)
                {
                    dsDatabaseVersion.Database_versionRow Row = null;
                    if (DatabaseVersionRecord.Id != -1)
                        Row = DataSet.Database_version.FindByid((long)DatabaseVersionRecord.Id);

                    bool AddRow = false;

                    if (Row == null)
                    {
                        DataSet.Database_version.NewDatabase_versionRow();
                        AddRow = true;
                    }

                    Row.version = DatabaseVersionRecord.Version;
                    Row.timestamp = DatabaseVersionRecord.Timestamp;
                    Row.description = DatabaseVersionRecord.Description;

                    if (AddRow)
                        DataSet.Database_version.AddDatabase_versionRow(Row);

                    DatabaseVersionTableAdapter.Update(DataSet);
                    RetVal = true;
                }
            }
            catch (Exception ex)
            {
            }
            return RetVal;
        }
    }
}
