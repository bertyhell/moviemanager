using System.Collections.ObjectModel;

namespace Tmc.SystemFrameworks.Log.Model
{
    class LogDatabase
    {
        //private const string CONNECTION_STRING = @"data source=C:\ProgramData\MovieManager\Log\moviemanager_log.sqlite";
        //private static SQLiteConnection _conn;

        static LogDatabase()
        {
            //GetConnection();
        }

        public static ObservableCollection<LoggingEntry> GetLogEntries()
        {
            //ObservableCollection<LoggingEntry> RetVal = new ObservableCollection<LoggingEntry>();
            //DsLogging LoggingDataset = new DsLogging();
            //if (FillLoggingDataset(LoggingDataset))
            //{
            //    foreach (DsLogging.LogRow LogRow in LoggingDataset.Log.Rows)
            //    {
            //        RetVal.Add(new LoggingEntry
            //            {
            //                Id = LogRow.LogId,
            //                Level = LogRow.Level,
            //                Logger = LogRow.Logger,
            //                Message = LogRow.Message,
            //                Timestamp = LogRow.Date
            //            });
            //    }
            //}
            //return RetVal;
            return null;
        } 

        //private static bool FillLoggingDataset(DsLogging dataSet)
        //{
        //    //bool Retval = true;
        //    //try
        //    //{
        //    //    SQLiteDataAdapter Adap = GetAdapter("SELECT * FROM Log");
        //    //    Adap.Fill(dataSet, "Log");
        //    //}
        //    //catch
        //    //{
        //    //    Retval = false;
        //    //}

        //    //return Retval;
        //}

        //private static SQLiteDataAdapter GetAdapter(string sql)
        //{
        //    //SQLiteDataAdapter Adapter = null;
        //    //try
        //    //{
        //    //    SQLiteCommand Cmd = GetCommand(_conn, sql);
        //    //    Adapter = new SQLiteDataAdapter(Cmd);
        //    //}
        //    //catch (Exception E)
        //    //{
        //    //    Console.WriteLine(Resources.ExceptionInDatabase + E.Message);
        //    //}
        //    //return Adapter;
        //}

        #region helper methods


        //private static SQLiteCommand GetCommand(SQLiteConnection conn, string sql)
        //{
        //    //return new SQLiteCommand(sql) { Connection = conn };
        //}

        private static void GetConnection()
        {
            //if (_conn == null)
            //{
            //    _conn = new SQLiteConnection(CONNECTION_STRING);
            //    _conn.Open();
            //}
        }

        #endregion
    }
}
