using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using MovieManager.LOG.Model;

namespace MovieManager.LOG
{
    public class LoggingController
    {

        public LoggingController()
        {
            _logEntries = LogDatabase.GetLogEntries();
        }

        private ObservableCollection<LoggingEntry> _logEntries;
        public ObservableCollection<LoggingEntry> LogEntries
        {
            get { return _logEntries; }
            set { _logEntries = value; }
        }
    }
}
