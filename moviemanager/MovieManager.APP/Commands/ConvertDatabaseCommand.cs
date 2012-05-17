using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            MMDatabaseCreation.ConvertDatabase(_path);
        }
    }
}
