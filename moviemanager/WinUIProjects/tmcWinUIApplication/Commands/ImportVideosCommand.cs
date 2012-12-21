using System;
using System.Windows.Input;
using ExcelInterop;

namespace MovieManager.APP.Commands
{
    class ImportVideosCommand : ICommand
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
            ExcelImportWindow ImportWindow = new ExcelImportWindow();
            ImportWindow.Show();
        }
    }
}
