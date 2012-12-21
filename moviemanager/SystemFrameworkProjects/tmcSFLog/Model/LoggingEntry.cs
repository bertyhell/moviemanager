using System;

namespace Tmc.SystemFrameworks.Log.Model
{
    public class LoggingEntry
    {
        public LoggingEntry()
        {
            
        }

        private long _id;
        public long Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private DateTime _timestamp;
        public DateTime Timestamp
        {
            get { return _timestamp; }
            set { _timestamp = value; }
        }

        private string _level;
        public string Level
        {
            get { return _level; }
            set { _level = value; }
        }

        private string _logger;
        public string Logger
        {
            get { return _logger; }
            set { _logger = value; }
        }

        private string _message;

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }
    }
}
