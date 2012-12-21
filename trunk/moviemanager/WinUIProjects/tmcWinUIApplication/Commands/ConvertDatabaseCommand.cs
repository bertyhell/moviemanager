using System;
using System.Windows.Input;
using MovieManager.APP.Properties;
using Tmc.DataAccess.Sqlite;

namespace Tmc.WinUI.Application.Commands
{
    class ConvertDatabaseCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            string DatabasePath = Settings.Default.DatabasePath;
            string ConnectionString = Settings.Default.ConnectionString.Replace("{path}", DatabasePath);

            TmcDatabaseCreation.ConvertDatabase(ConnectionString);
        }
    }
}
