using System;
using System.Windows.Input;
using Tmc.WinUI.Application.Panels.Analyse;

namespace Tmc.WinUI.Application.Commands
{
	class AnalyseCommand : ICommand
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
			//TODO 030 if no videos in database --> don't show analyse window --> show popup or statusbar error



			//foreach (Video Video in MainController.Instance.Videos)
			//{
			//    if (Video is MovieInfo)
			//    {
			//        List<MovieInfo> Movies = SearchTMDB.GetVideoInfo(Video.Name);
			//        if (Movies.Count > 0)
			//        {
			//            (Video as MovieInfo).IdImdb = Movies[0].IdImdb;
			//            SearchTMDB.GetExtraMovieInfo(Movies[0].IdTmdb, (MovieInfo)Video);
			//        }
			//        //if(!string.IsNullOrEmpty(Video.IdImdb))
			//        //SearchIMDB.GetVideoInfo(Video);
			//        //else
			//        //{
			//        //TODO 060: search for imdb id

			//        //}
			//    }
			//}
			AnalyseWindow AnalyseWindow = new AnalyseWindow { Owner = MainWindow.Instance };
			AnalyseWindow.ShowDialog();

		}
	}
}
