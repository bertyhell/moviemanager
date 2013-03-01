using System;
using System.Windows.Input;
using Tmc.BusinessRules.ExportImport;

namespace Tmc.WinUI.Application.Commands
{
    class ExportVideosCommand : ICommand
    {
        public bool CanExecute(object parameter)
		{
			return MainController.Instance != null && MainController.Instance.Videos != null && MainController.Instance.Videos.Count > 0;
        }

        public event EventHandler CanExecuteChanged;

        public void OnExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, new EventArgs());
        }

		internal void OnExecuteChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			OnExecuteChanged();
		}

        public void Execute(object parameter)
        {
            ExcelExportWindow ExportWindow = new ExcelExportWindow();
            ExportWindow.Show();

        }
	}
}
