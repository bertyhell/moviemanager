using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tmc.DataAccess.SqlCe;
using Tmc.SystemFrameworks.Common;
using Tmc.SystemFrameworks.Model;
using Tmc.WinUI.Application.Cache;

namespace Tmc.WinUI.Application.Panels.Analyse
{
	class AddAnalyseVideosToDatabase : BackgroundWorker
	{
		private readonly List<AnalyseVideo> _analyseVideos;

		public AddAnalyseVideosToDatabase(ObservableList<AnalyseVideo> analyseVideos)
		{
			_analyseVideos = analyseVideos;
		}

		private int _progress;

		protected override void OnDoWork(DoWorkEventArgs e)
		{
			var Videos = new List<Video>();
			_progress = 1;
			foreach (var AnalyseVideo in _analyseVideos)
			{
				Video Video = AnalyseVideo.SelectedCandidate;

				if (Video != null)
				{
					AnalyseVideo.Video.CopyAnalyseVideoInfo(Video);

					var Images = new List<Uri>();
					foreach (ImageInfo ImageInfo in Video.Images)
					{
						if (ImageInfo.Uri != null)
						{
							Images.Add(new Uri(ImageInfo.Uri.AbsoluteUri));
						}
					}
					ApplicationCache.AddVideoImages(AnalyseVideo.Video.Id, Images, CacheImageType.Images, ImageQuality.Medium);

					Videos.Add(Video);
				}
				_progress++;
				OnUpdateVideosProgressInvokator(new ProgressEventArgs { ProgressNumber = _progress });
			}
			DataRetriever.UpdateVideosProgress += DataRetriever_UpdateVideosProgress;
			DataRetriever.Videos = Videos;
		}

		void DataRetriever_UpdateVideosProgress(object sender, ProgressEventArgs eventArgs)
		{
			_progress++;
			OnUpdateVideosProgressInvokator(new ProgressEventArgs { ProgressNumber = _progress });
		}

		public static event OnUpdateVideosProgress UpdateVideosProgress;

		private static void OnUpdateVideosProgressInvokator(ProgressEventArgs args)
		{
			if (UpdateVideosProgress != null) UpdateVideosProgress(null, args);
		}


		public delegate void OnUpdateVideosProgress(object sender, ProgressEventArgs args);
	}

}
