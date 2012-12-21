using System;
using System.Windows.Input;
using MovieManager.APP.Panels.Search;

namespace MovieManager.APP.Commands
{
    class SearchCommand : ICommand
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
            SearchWindow Window = new SearchWindow();
            Window.Show();
        }
    }
}
