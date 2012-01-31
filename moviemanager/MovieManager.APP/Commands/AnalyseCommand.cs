using System;
using System.Collections.Generic;
using System.Windows.Input;
using Model;
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
            foreach (Video video in MainController.Instance.Videos)
            {
                if (video is Movie)
                {
                    List<Movie> movies = SearchTMDB.GetVideoInfo(video.Name);
                    if (movies.Count > 0)
                    {
                        (video as Movie).IdImdb = movies[0].IdImdb;
                        SearchTMDB.GetExtraMovieInfo(movies[0].IdTmdb, (Movie)video);
                    }
                    //if(!string.IsNullOrEmpty(Video.IdImdb))
                    //SearchIMDB.GetVideoInfo(Video);
                    //else
                    //{
                    //TODO 060: search for imdb id

                    //}
                }
            }
        }
    }
}
