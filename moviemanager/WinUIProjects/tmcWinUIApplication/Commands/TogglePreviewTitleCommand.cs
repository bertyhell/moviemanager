using System;
using System.Windows;
using System.Windows.Input;

namespace Tmc.WinUI.Application.Commands
{
    class TogglePreviewTitleCommand : ICommand
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
            MainController.Instance.PreviewTitleVisibility = MainController.Instance.PreviewTitleVisibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;//TODO 003 make this work
        }
    }
}
