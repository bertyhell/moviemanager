using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using Model;
using SQLite.dsDatabaseVersionTableAdapters;

namespace SQLite
{
    public class MMDatabaseCreation
    {
        private static readonly int CURRENT_DATABASE_VERSION = 1;

        //public static event VideosChanged OnVideosChanged;
        //don't use videos changed -> all database operations could be run in different thread -> different trhead has no access to observable collection
        //update lists in maincontroller in commandobjects

        public static bool ConvertDatabase(string pathToDatabase)
        {
            bool Retval = true;
            DatabaseDetails details = GetDatabaseDetails();
            if (details.DatabaseVersion == CURRENT_DATABASE_VERSION)
                return Retval;

            if (details.DatabaseVersion < 1)
            {
                Retval &= CreateDatabase(pathToDatabase);
            }

            if (Retval && details.DatabaseVersion < 2)
            {
                details.DatabaseVersion = 1;
                details.VersionRecords.Add(new DatabaseVersionRecord { Id = -1, Description = "new tables", Timestamp = DateTime.Now, Version = 2 });
                Retval &= CreateTablesv002();
                Retval &= AlterTablesv002();
                Retval &= AddDefaultValuesv002();
            }

            UpdateDatabaseDetails(details);

            return Retval;


        }


        public static bool CreateDatabase(string pathToDatabase)
        {
            bool Retval = false;

            try
            {
                Database.CreateDatabaseFile(pathToDatabase);
                SQLiteConnection Conn = Database.GetConnection(pathToDatabase);

                string SQLQuery =
                    "CREATE TABLE Genres ( gen_id INTEGER PRIMARY KEY AUTOINCREMENT, gen_label TEXT NOT NULL UNIQUE )";
                Database.ExecuteSQL(Conn, SQLQuery);

                SQLQuery = "CREATE TABLE Franchises (id INTEGER PRIMARY KEY AUTOINCREMENT, name VARCHAR NOT NULL)";
                Database.ExecuteSQL(Conn, SQLQuery);

                SQLQuery = "CREATE TABLE Series (id INTEGER PRIMARY KEY AUTOINCREMENT, name VARCHAR(255) NOT NULL)";
                Database.ExecuteSQL(Conn, SQLQuery);

                SQLQuery =
                    "CREATE TABLE Videos ( id INTEGER PRIMARY KEY AUTOINCREMENT, id_imdb VARCHAR(10), name VARCHAR(255) NOT NULL, release DATE, rating DOUBLE, rating_imdb DOUBLE, path VARCHAR(255), last_play_location INTEGER default 0, runtime TIME, PlayCount INTEGER DEFAULT 0)";
                Database.ExecuteSQL(Conn, SQLQuery);

                SQLQuery = "CREATE TABLE Movies ( id INTEGER PRIMARY KEY, franchise_id INTEGER, id_tmdb INTEGER," +
                           " FOREIGN KEY(id) REFERENCES Videos(id)" +
                           " FOREIGN KEY(franchise_id) REFERENCES Franchises(id))";
                Database.ExecuteSQL(Conn, SQLQuery);

                SQLQuery =
                    "CREATE TABLE Episodes ( id INTEGER PRIMARY KEY, serie_id INTEGER NOT NULL, season INTEGER NOT NULL, episode_number," +
                    " FOREIGN KEY(id) REFERENCES Videos(id)" +
                    " FOREIGN KEY(serie_id) REFERENCES Series(id))";
                Database.ExecuteSQL(Conn, SQLQuery);

                SQLQuery = "CREATE TABLE Videos_genres ( video_id INTEGER NOT NULL, genre_id INTEGER NOT NULL," +
                           "UNIQUE (video_id, genre_id) ON CONFLICT ABORT, " +
                           "FOREIGN KEY(video_id) REFERENCES Videos(id)," +
                           "FOREIGN KEY(genre_id) REFERENCES Genres(gen_id))";
                Database.ExecuteSQL(Conn, SQLQuery);

                SQLQuery =
                    "CREATE TABLE Database_version ( id INTEGER PRIMARY KEY AUTOINCREMENT, version INTEGER NOT NULL, timestamp DATETIME default current_timestamp , description VARCHAR(255) )";
                Database.ExecuteSQL(Conn, SQLQuery);

                AddDefaultValues_version001(Conn);

                Retval = true;
            }
            catch
            {
            }
            return Retval;
        }

        private static void AddDefaultValues_version001(SQLiteConnection Conn)
        {
            string SQLQuery = "INSERT INTO Database_version (version, description) values (1, 'table creation')";
            Database.ExecuteSQL(Conn, SQLQuery);
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

            Database_versionTableAdapter DatabaseVersionTableAdapter = new Database_versionTableAdapter();
            DatabaseVersionTableAdapter.Fill(DataSet.Database_version);

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
            catch(Exception ex)
            {
            }
            return RetVal;
        }


    }
}
