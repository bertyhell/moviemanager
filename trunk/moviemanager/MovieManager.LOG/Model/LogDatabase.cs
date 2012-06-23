using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace MovieManager.LOG.Model
{
    class LogDatabase
    {
        private static string _connectionString = @"data source=C:\ProgramData\MovieManager\Log\moviemanager_log.sqlite";
        private static SQLiteConnection _conn;

        static LogDatabase()
        {
            GetConnection();
        }

        public static ObservableCollection<LoggingEntry> GetLogEntries()
        {
            ObservableCollection<LoggingEntry> RetVal = new ObservableCollection<LoggingEntry>();
            DsLogging LoggingDataset = new DsLogging();
            FillLoggingDataset(LoggingDataset);
            foreach (DsLogging.LogRow LogRow in LoggingDataset.Log.Rows)
            {
                RetVal.Add(new LoggingEntry
                               {
                                   Id = LogRow.LogId,
                                   Level = LogRow.Level,
                                   Logger = LogRow.Logger,
                                   Message =  LogRow.Message,
                                   Timestamp = LogRow.Date
                               });
            }
            return RetVal;
        } 

        private static bool FillLoggingDataset(DsLogging dataSet)
        {
            bool Retval = true;
            try
            {
                SQLiteDataAdapter Adap = GetAdapter("SELECT * FROM Log");
                Adap.Fill(dataSet, "Log");
            }
            catch
            {
                Retval = false;
            }

            return Retval;
        }

        private static SQLiteDataAdapter GetAdapter(string sql)
        {
            SQLiteDataAdapter Adapter = null;
            try
            {
                SQLiteCommand Cmd = GetCommand(_conn, sql);
                Adapter = new SQLiteDataAdapter(Cmd);
            }
            catch (Exception E)
            {
                Console.WriteLine("Exception in Database: " + E.Message);
            }
            return Adapter;
        }

        #region helper methods


        private static SQLiteCommand GetCommand(SQLiteConnection conn, string sql)
        {
            return new SQLiteCommand(sql) { Connection = conn };
        }

        private static SQLiteConnection GetConnection()
        {
            if (_conn == null)
            {
                _conn = new SQLiteConnection(_connectionString);
                _conn.Open();
            }
            return _conn;
        }

        #endregion
    }
}
