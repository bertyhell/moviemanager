using System;
using System.IO;
using System.Data.SqlServerCe;

namespace Tmc.DataAccess.Sqlite
{
    internal class TmcDatabaseCreation
    {
    //    private static SqlCeConnection _conn;

    //    public static void Init(SqlCeConnection connection)
    //    {
    //        _conn = connection;
    //    }

    //    /// <summary>
    //    /// This will convert the database to the new version
    //    /// </summary>
    //    public static bool ConvertDatabase()
    //    {
    //        bool Retval = true;
    //        DatabaseDetails Details;
    //        try { Details = GetDatabaseDetails(); }
    //        catch
    //        {
    //            Details = new DatabaseDetails { DatabaseVersion = 1, RequiredVersion = TmcDatabase.CURRENT_DATABASE_VERSION };
    //            Retval &= CreateTables100();
    //        }

    //        if (Details.DatabaseVersion == TmcDatabase.CURRENT_DATABASE_VERSION)
    //            return Retval;

    //        if (Retval && Details.DatabaseVersion < 2)
    //        {
    //            Details.DatabaseVersion = 1;
    //            Details.VersionRecords.Add(new DatabaseVersionRecord { Id = -1, Description = "new tables", Timestamp = DateTime.Now, Version = 2 });
    //            Retval &= CreateTablesv002();
    //            Retval &= AlterTablesv002();
    //            Retval &= AddDefaultValuesv002();
    //        }

    //        Retval &= UpdateDatabaseDetails(Details);

    //        return Retval;


    //    }

    //    /// <summary>
    //    /// Creates a database file
    //    /// </summary>
    //    public static SqlCeConnection CreateDatabase(string pathToDatabase)
    //    {
    //        if (File.Exists(pathToDatabase))
    //            File.Delete(pathToDatabase);

    //        string ConnStr = string.Format("Data Source = {0};", pathToDatabase);

    //        SqlCeEngine Engine = new SqlCeEngine(ConnStr);
    //        Engine.CreateDatabase();
    //        Engine.Dispose();

    //        return new SqlCeConnection(ConnStr);
    //    }

    //    /// <summary>
    //    /// Creates the tables for version 100
    //    /// </summary>
    //    private static bool CreateTables100()
    //    {
    //        string SqlQuery = "CREATE TABLE Genres ( gen_id INTEGER IDENTITY PRIMARY KEY, gen_label NVARCHAR NOT NULL UNIQUE )";
    //        Database.ExecuteSql(SqlQuery);

    //        SqlQuery = "CREATE TABLE Franchises (id INTEGER IDENTITY PRIMARY KEY, name NVARCHAR NOT NULL)";
    //        Database.ExecuteSql(SqlQuery);

    //        SqlQuery = "CREATE TABLE Series (id INTEGER IDENTITY PRIMARY KEY, name NVARCHAR(255) NOT NULL)";
    //        Database.ExecuteSql(SqlQuery);

    //        SqlQuery = "CREATE TABLE Videos ( id INTEGER IDENTITY PRIMARY KEY, id_imdb NVARCHAR(10) DEFAULT NULL, name NVARCHAR(255) NOT NULL, release DATETIME, play_count INTEGER DEFAULT 0 NOT NULL, " +
    //            "rating REAL DEFAULT -1 NOT NULL, rating_imdb REAL DEFAULT -1 NOT NULL, path NVARCHAR(255), last_play_location INTEGER NOT NULL default 0, runtime DATETIME, poster NVARCHAR(255) )";
    //        Database.ExecuteSql(SqlQuery);

    //        SqlQuery = "CREATE TABLE Movies ( id INTEGER PRIMARY KEY, franchise_id INTEGER, id_tmdb INTEGER," +
    //                     "FOREIGN KEY(id) REFERENCES Videos(id), " +
    //                     "FOREIGN KEY(franchise_id) REFERENCES Franchises(id))";
    //        Database.ExecuteSql(SqlQuery);

    //        SqlQuery = "CREATE TABLE Episodes ( id INTEGER PRIMARY KEY, serie_id INTEGER NOT NULL, season INTEGER NOT NULL, episode_number INTEGER DEFAULT -1 NOT NULL, " +
    //             "FOREIGN KEY(id) REFERENCES Videos(id)," +
    //             "FOREIGN KEY(serie_id) REFERENCES Series(id))";
    //        Database.ExecuteSql(SqlQuery);

    //        SqlQuery = "CREATE TABLE Videos_genres ( video_id INTEGER NOT NULL, genre_id INTEGER NOT NULL," +
    //            "CONSTRAINT UniqueConstraint UNIQUE (video_id, genre_id), " +
    //            "FOREIGN KEY(video_id) REFERENCES Videos(id), " +
    //            "FOREIGN KEY(genre_id) REFERENCES Genres(gen_id))";
    //        Database.ExecuteSql(SqlQuery);

    //        SqlQuery = "CREATE TABLE Database_version ( id INTEGER IDENTITY PRIMARY KEY, version INTEGER NOT NULL, timestamp DATETIME default GETDATE(), description NVARCHAR(255) )";
    //        Database.ExecuteSql(SqlQuery);

    //        SqlQuery = "CREATE TABLE Video_folders ( id INTEGER IDENTITY PRIMARY KEY, path NVARCHAR(255) NOT NULL UNIQUE, auto_update INTEGER DEFAULT 0 )";
    //        Database.ExecuteSql(SqlQuery);

    //        AddDefaultValuesVersion001();

    //        return true;
    //    }

    //    private static void AddDefaultValuesVersion001()
    //    {
    //        const string SQL_QUERY = "INSERT INTO Database_version (version, description) values (1, 'table creation')";
    //        Database.ExecuteSql(SQL_QUERY);
    //    }

    //    private static bool CreateTablesv002()
    //    {
    //        return true;
    //    }

    //    private static bool AlterTablesv002()
    //    {
    //        return true;
    //    }

    //    private static bool AddDefaultValuesv002()
    //    {
    //        return true;
    //    }

    //    public static DatabaseDetails GetDatabaseDetails()
    //    {

    //        DatabaseDetails RetVal = new DatabaseDetails();
    //        DsDatabaseVersion DataSet = new DsDatabaseVersion();

    //        Database_versionTableAdapter DatabaseVersionTableAdapter = new Database_versionTableAdapter { Connection = _conn };
    //        DatabaseVersionTableAdapter.Fill(DataSet.Database_version);

    //        int DatabaseVersion = 0;
    //        foreach (DsDatabaseVersion.Database_versionRow DatabaseVersionRow in DataSet.Database_version.Rows)
    //        {
    //            if (DatabaseVersionRow.version > DatabaseVersion)
    //                DatabaseVersion = DatabaseVersionRow.version;

    //            RetVal.VersionRecords.Add(new DatabaseVersionRecord
    //            {
    //                Id = DatabaseVersionRow.id,
    //                Version = DatabaseVersionRow.version,
    //                Description = DatabaseVersionRow.description,
    //                Timestamp = DatabaseVersionRow.timestamp

    //            });
    //        }
    //        RetVal.DatabaseVersion = DatabaseVersion;
    //        RetVal.RequiredVersion = TmcDatabase.CURRENT_DATABASE_VERSION;


    //        return RetVal;
    //    }

    //    private static bool UpdateDatabaseDetails(DatabaseDetails details)
    //    {
    //        bool RetVal = true;

    //        try
    //        {

    //            DsDatabaseVersion DataSet = new DsDatabaseVersion();
    //            Database_versionTableAdapter DatabaseVersionTableAdapter = new Database_versionTableAdapter();
    //            DatabaseVersionTableAdapter.Fill(DataSet.Database_version);

    //            foreach (DatabaseVersionRecord DatabaseVersionRecord in details.VersionRecords)
    //            {
    //                DsDatabaseVersion.Database_versionRow Row = null;
    //                if (DatabaseVersionRecord.Id != -1)
    //                    Row = DataSet.Database_version.FindByid(DatabaseVersionRecord.Id);

    //                bool AddRow = false;

    //                if (Row == null)
    //                {
    //                    Row = DataSet.Database_version.NewDatabase_versionRow();
    //                    AddRow = true;
    //                }

    //                if (Row != null)
    //                {
    //                    Row.version = DatabaseVersionRecord.Version;
    //                    Row.timestamp = DatabaseVersionRecord.Timestamp;
    //                    Row.description = DatabaseVersionRecord.Description;

    //                    if (AddRow)
    //                        DataSet.Database_version.AddDatabase_versionRow(Row);

    //                    DatabaseVersionTableAdapter.Update(DataSet);
    //                }
    //            }
    //        }
    //        catch
    //        {
    //            RetVal = false;
    //        }

    //        return RetVal;
    //    }
    }
}
