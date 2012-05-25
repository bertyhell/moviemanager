using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Filter;
using log4net.Layout;

namespace MovieManager.APP
{
    class LogInitializer
    {
        private static ILog _log;

        public void EnableLogger()
        {
            //BasicConfigurator.Configure();

            _log = LogManager.GetLogger("default");
            AddAppender(LogManager.GetLogger("default"));


            _log.Debug("THis is sadi's world!");

            _log.Info("How beautyful the console looks like");

            _log.Warn("You are great you did this");

            _log.Error("Who make you know is the best");

            _log.Fatal("sadi the great");


        }
        //public void DisableLogger()
        //{
            

        //}

        void AddAppender(ILog logger)
        {
            // get the interface
            IAppenderAttachable ConnectionAppender = (IAppenderAttachable)logger.Logger;
            // get the layout string
            const string LOG4_NET_LAYOUT_STRING = "%d%n   %m%n  %P MessageData}%n%n";
            // get logging path
            const string LOG4_NET_PATH = ".\\";
            // create the appender
            var DatabaseAdapter = new AdoNetAppender
                                         {
                                             BufferSize = 100, 
                                             ConnectionType = "Finisar.SQLite.SQLiteConnection, SQLite.NET, Version=0.21.1869.3794, Culture=neutral, PublicKeyToken=c273bd375e695f9c"//,
                                             //connectionString = 
                                         };


            // setup the appender
            string AppenderPath = "moviemanager.log";
            // log source name may have a colon - if soreplace with underscore
            AppenderPath = AppenderPath.Replace(':', '_');
            // now add to log4net path
            AppenderPath = Path.Combine(LOG4_NET_PATH, AppenderPath);
            // update file property of appender
            //DatabaseAdapter.File = AppenderPath;
            // add the layout
            PatternLayout PatternLayout = new PatternLayout(LOG4_NET_LAYOUT_STRING);
            DatabaseAdapter.Layout = PatternLayout;
            //// now add the deny all filter to end of the chain
            //DenyAllFilter denyAllFilter = new DenyAllFilter();
            //this._rollingFileAppender.AddFilter(denyAllFilter);
            // activate the options
            DatabaseAdapter.ActivateOptions();
            // add the appender
            ConnectionAppender.AddAppender(DatabaseAdapter);
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
