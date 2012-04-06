using System;
using System.Windows.Input;

namespace MovieManager.APP.Commands
{
    class RenameFileCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
