using System;
using System.Windows.Input;

namespace Tmc.WinUI.Application.Commands
{
    class RenameFileCommand : ICommand
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
            throw new NotImplementedException();
        }
    }
}
