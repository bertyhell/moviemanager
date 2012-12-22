using System;
using System.Windows.Input;
using Tmc.SystemFrameworks.Log;

namespace Tmc.WinUI.Application.Commands.Debug
{
    class LoggingToolCommand : ICommand
    {
        public void Execute(object parameter)
        {
            LoggingWindow Window = new LoggingWindow();
            Window.Show();
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void OnCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, new EventArgs());
        }
    }
}
