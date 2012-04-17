using System;
using System.Collections.Generic;
using System.Windows.Input;
using Model;
using MovieManager.APP.Panels.Analyse;
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
            //foreach (Video Video in MainController.Instance.Videos)
            //{
            //    if (Video is Movie)
            //    {
            //        List<Movie> Movies = SearchTMDB.GetVideoInfo(Video.Name);
            //        if (Movies.Count > 0)
            //        {
            //            (Video as Movie).IdImdb = Movies[0].IdImdb;
            //            SearchTMDB.GetExtraMovieInfo(Movies[0].IdTmdb, (Movie)Video);
            //        }
            //        //if(!string.IsNullOrEmpty(Video.IdImdb))
            //        //SearchIMDB.GetVideoInfo(Video);
            //        //else
            //        //{
            //        //TODO 060: search for imdb id

            //        //}
            //    }
            //}
            AnalyseWindow AnalyseWindow = new AnalyseWindow(){Owner = MainWindow.Instance};
            AnalyseWindow.ShowDialog();
        }
    }
}
