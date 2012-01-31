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

        public void Execute(object parameter)
        {
            SearchWindow window = new SearchWindow();
            window.Show();
        }
    }
}
