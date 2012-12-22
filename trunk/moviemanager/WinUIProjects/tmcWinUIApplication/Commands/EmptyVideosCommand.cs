using System;
using System.Windows.Input;
using Tmc.DataAccess.Sqlite;

namespace Tmc.WinUI.Application.Commands
{
    class EmptyVideosCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void OnExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, new EventArgs());
        }

        public void Execute(object parameter)
        {
            TmcDatabase.EmptyVideoTables();
        }
    }
}
