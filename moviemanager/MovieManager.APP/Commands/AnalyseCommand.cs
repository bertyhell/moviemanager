using System;
using System.Windows.Input;
using Model;
using MovieManager.APP.Search;
using MovieManager.BL.Search;

namespace MovieManager.APP.Commands
{
    class AnalyseCommand : ICommand
    {

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            foreach (Video Video in MainController.Instance.Videos)
            {
                if(!string.IsNullOrEmpty(Video.IdImdb))
                SearchIMDB.GetVideoInfo(Video);
                else
                {
                    //TODO 060: search for imdb id
                    
                }
            }
        }
    }
}
