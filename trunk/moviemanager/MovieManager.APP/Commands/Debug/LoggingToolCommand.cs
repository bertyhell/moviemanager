using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using MovieManager.LOG;

namespace MovieManager.APP.Commands.Debug
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
