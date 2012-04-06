using System;
using System.Windows.Input;
using ExcelInterop;

namespace MovieManager.APP.Commands
{
    class ExportVideosCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            ExcelExportWindow ExportWindow = new ExcelExportWindow();
            ExportWindow.Show();

        }
    }
}
