using System;
using System.Windows.Input;

namespace MovieManager.APP.Commands
{
    class TogglePreviewMarginCommand : ICommand
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
            MainController.Instance.PreviewItemMargin = MainController.Instance.PreviewItemMargin > 0 ? 0 : 5;//TODO 003 make this work
        }
    }
}
