using System;
using System.Windows.Input;
using SQLite;

namespace MovieManager.APP.Commands
{
    class ConvertDatabaseCommand : ICommand
    {
        private string _path;

        public ConvertDatabaseCommand(string pathToFile)
        {
            _path = pathToFile;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            string DatabasePath = Properties.Settings.Default.DatabasePath;
            string ConnectionString = Properties.Settings.Default.ConnectionString.ToString().Replace("{path}", DatabasePath);

            MMDatabaseCreation.ConvertDatabase(ConnectionString);
        }
    }
}
