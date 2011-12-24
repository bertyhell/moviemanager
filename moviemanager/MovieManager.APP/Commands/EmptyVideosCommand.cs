using System;
using System.Windows.Input;
using SQLite;

namespace MovieManager.APP.Commands
{
    class EmptyVideosCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            MMDatabase.EmptyTable("videos");
        }
    }
}
