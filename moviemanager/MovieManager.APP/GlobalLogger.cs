﻿using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.IO;
using MovieManager.APP.Common;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Filter;
using log4net.Layout;

namespace MovieManager.APP
{
    class GlobalLogger
    {
        private static readonly GlobalLogger INSTANCE = new GlobalLogger();

        private GlobalLogger()
        {
            BasicConfigurator.Configure(GetSqliteAppender());
        }

        public static GlobalLogger Instance
        {
            get { return INSTANCE; }
        }

        public Level LogLevel
        {
            get { return LogManager.GetRepository().Threshold; }
            set { LogManager.GetRepository().Threshold = value; }
        }

        public ILog MovieManagerLogger
        {
            get { return LogManager.GetLogger("MovieManager"); }
        }

        public ILog VlcPlayerLogger
        {
            get { return LogManager.GetLogger("VlcPlayer"); }
        }

        public static string FormatExceptionForLog(string className, string functionName, string exception)
        {
            return string.Format("Class: {0} \nFunction: {1} \nMessage: {2}", className, functionName, exception);
        }

        private IAppender GetSqliteAppender()
        {
            // setup the appender
            string LogFilename = DefaultValues.LOG_FILENAME;
            LogFilename = LogFilename.Replace(':', '_');// log source name may have a colon - if soreplace with underscore
            LogFilename = Path.Combine(DefaultValues.LogDirectory, LogFilename);

            var dbFile = new FileInfo(LogFilename);

            if (!dbFile.Exists)
            {
                CreateLogDb(dbFile);
            }
            // create the appender
            var DatabaseAdapter = new AdoNetAppender
            {
                BufferSize = 1,
                ConnectionType = "System.Data.SQLite.SQLiteConnection, System.Data.SQLite",
                ConnectionString = "Data Source=" + LogFilename,
                CommandText = "INSERT INTO Log (Date, Level, Logger, Message) VALUES (@Date, @Level, @Logger, @Message)"
            };

            const string LOG4_NET_LAYOUT_STRING = "%timestamp [%thread] %level %logger %ndc - %message%newline";
            PatternLayout PatternLayout = new PatternLayout(LOG4_NET_LAYOUT_STRING);
            DatabaseAdapter.Layout = PatternLayout;

            DatabaseAdapter.AddParameter(new AdoNetAppenderParameter
            {
                ParameterName = "@Date",
                DbType = DbType.DateTime,
                Layout = new RawTimeStampLayout()

            });

            DatabaseAdapter.AddParameter(new AdoNetAppenderParameter
            {
                ParameterName = "@Level",
                DbType = DbType.String,
                Layout = new Layout2RawLayoutAdapter(new PatternLayout("%level"))
            });

            DatabaseAdapter.AddParameter(new AdoNetAppenderParameter
            {
                ParameterName = "@Logger",
                DbType = DbType.String,
                Layout = new Layout2RawLayoutAdapter(new PatternLayout("%logger"))
            });

            DatabaseAdapter.AddParameter(new AdoNetAppenderParameter
            {
                ParameterName = "@Message",
                DbType = DbType.String,
                //Layout = new RawPropertyLayout { Key = "MessageData" }
                Layout = new Layout2RawLayoutAdapter(new PatternLayout("%message"))
            });

            DatabaseAdapter.ActivateOptions();
            return DatabaseAdapter;
        }

        private void CreateLogDb(FileInfo file)
        {
            using (var Conn = new SQLiteConnection())
            {
                Conn.ConnectionString = string.Format("Data Source={0};New=True;Compress=True;Synchronous=Off", file.FullName);
                Conn.Open();
                var Cmd = Conn.CreateCommand();

                Cmd.CommandText =
                                 @"CREATE TABLE Log(
                            LogId     INTEGER PRIMARY KEY,
                            Date      DATETIME NOT NULL,
                            Level     VARCHAR(50) NOT NULL,
                            Logger    VARCHAR(255) NOT NULL,
                            Message   TEXT DEFAULT NULL
                        );";

                Cmd.ExecuteNonQuery();
                Cmd.Dispose();
                Conn.Close();
            }
        }

        void RemoveAppender(ILog logger)
        {
            // check if we have one
            //if (databaseAdapter != null)
            //{
            //    // cast to required interface
            //    IAppenderAttachable ConnectionAppender = (IAppenderAttachable)logger.Logger;
            //    // remove the appendier
            //    ConnectionAppender.RemoveAppender(databaseAdapter);
            //    // set to null
            //    databaseAdapter = null;
            //}
        }
    }

}
