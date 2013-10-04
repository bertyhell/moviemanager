using System;
using System.Windows;
using System.Windows.Input;

namespace Tmc.WinUI.Application.Commands
{
    class TogglePreviewMarginCommand : ICommand
    {
	    private const ushort NO_MARGIN = 0;
	    private const ushort MARGIN = 5;

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
			MainController.Instance.PreviewItemMargin = new Thickness(MainController.Instance.PreviewItemMargin.Bottom > 1 ? MARGIN : NO_MARGIN);
        }
    }
}
