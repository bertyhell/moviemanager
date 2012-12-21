using System.Collections.ObjectModel;
using Tmc.SystemFrameworks.Log.Model;

namespace Tmc.SystemFrameworks.Log
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
