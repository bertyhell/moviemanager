using System;
using System.Windows.Input;
using SQLite;

namespace MovieManager.APP.Commands
{
    class EmptyVideosCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            MMDatabase.EmptyVideoTables();

            for (int i = 0; i < MainController.Instance.Videos.Count; i++)
            {
                MainController.Instance.Videos.RemoveAt(0);
            }
            MainController.Instance.UpdateVideos();
        }
    }
}
