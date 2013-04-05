using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using Tmc.DataAccess.SqlCe;
using Tmc.SystemFrameworks.Common;
using Tmc.SystemFrameworks.Log;
using Tmc.SystemFrameworks.Model;
using Tmc.WinUI.Application.Common;

namespace Tmc.WinUI.Application.Panels.Analyse
{
	class AnalyseController : INotifyPropertyChanged
	{
		//TODO 095 add progressbar for saving videoinfo after analyse
		//TODO 100 add progressbar for downloading poster images to cache after analyse

		public AnalyseController()
		{
			ProgressBarInfoTotal = new ProgressBarInfo();
			ProgressBarInfoPass = new ProgressBarInfo();
			List<Video> Videos = MainController.Instance.VideosList;
			AnalyseVideos.Clear();
			foreach (Video Video in Videos)
			{
				if (Video.Files.Count > 0)
				{
					AnalyseVideos.Add(new AnalyseVideo { Video = Video, TitleGuesses = VideoTitleExtractor.GetTitleGuessesFromPath(Video.Files[0].Path) });
				}
				else
				{
					AnalyseVideos.Add(new AnalyseVideo { Video = Video, TitleGuesses = new List<string> { Video.Name } });
				}
			}
		}

		private ObservableList<AnalyseVideo> _analyseVideos = new ObservableList<AnalyseVideo>();
		public ObservableList<AnalyseVideo> AnalyseVideos
		{
			get
			{
				return _analyseVideos;
			}
			set
			{
				_analyseVideos = value;
			}
		}

		private AnalyseVideo _selectedVideoFile;

		public AnalyseVideo SelectedVideoFile
		{
			get { return _selectedVideoFile; }
			set
			{
				_selectedVideoFile = value;
				OnPropertyChanged("SelectedVideoFile");
			}
		}

		public void BeginAnalyse(bool fullAnalyse = true)
		{
			//begin automatic analysis
			IsSaveEnabled = false;

			var AnalyseWorker = new AnalyseWorker(AnalyseVideos, fullAnalyse);
			ProgressBarInfoTotal.IsIndeterminate = true;
			ProgressBarInfoTotal.Message = "Contacting webservice...";
			AnalyseWorker.TotalProgress += AnalyseWorkerTotalProgress;
			AnalyseWorker.PassProgress += AnalyseWorkerPassProgress;
			AnalyseWorker.RunWorkerCompleted += AnalyseWorkerRunWorkerCompleted;
			AnalyseWorker.RunWorkerAsync();
		}

		public void AnalyseWorkerTotalProgress(object sender, ProgressEventArgs args)
		{
			ProgressBarInfoTotal.IsIndeterminate = false;
			ProgressBarInfoTotal.Message = args.Message;
			ProgressBarInfoTotal.Maximum = args.MaxNumber;
			ProgressBarInfoTotal.Value = args.ProgressNumber;
		}

		public void AnalyseWorkerPassProgress(object sender, ProgressEventArgs args)
		{
			ProgressBarInfoPass.IsIndeterminate = false;
			ProgressBarInfoPass.Message = "Analysing videos: " + args.ProgressNumber + " / " + args.MaxNumber;
			ProgressBarInfoPass.Maximum = args.MaxNumber;
			ProgressBarInfoPass.Value = args.ProgressNumber;
		}

		public void AnalyseWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			GlobalLogger.Instance.MovieManagerLogger.Info(GlobalLogger.FormatExceptionForLog("AnalyseController", "AnalyseWorkerRunWorkerCompleted", "Analyse completed"));

			IsSaveEnabled = true;
		}

		public ProgressBarInfo ProgressBarInfoTotal { get; set; }
		public ProgressBarInfo ProgressBarInfoPass { get; set; }


		//TODO 050 make analyse function multithreaded -> 1 thread for every MovieInfo lookup
		public void SaveVideos()//TODO 070 do this in a backgroundworker with progressbar
		{
			Message = "Downloading movie posters";
			Value = 0;
			Maximum = AnalyseVideos.Count * 2;
			IsIndeterminate = false;
			_progressWindow = new ProgressbarWindow(this) { Owner = MainWindow.Instance, DataContext = this };
			AddAnalyseVideosToDatabase AddAnalyseVideosToDatabase = new AddAnalyseVideosToDatabase(AnalyseVideos);
			AddAnalyseVideosToDatabase.UpdateVideosProgress += AddAnalyseVideosToDatabaseProgressChanged;
			AddAnalyseVideosToDatabase.RunWorkerCompleted += BgwInsertVideosRunWorkerCompleted;
			AddAnalyseVideosToDatabase.RunWorkerAsync();
			_progressWindow.ShowDialog();
		}

		void AddAnalyseVideosToDatabaseProgressChanged(object sender, ProgressEventArgs e)
		{
			if (Value < Maximum / 2)
			{
				Value = e.ProgressNumber;
				Message = "Downloading video posters: " +
						  Math.Round(e.ProgressNumber * 100.0 / Maximum * 2, 1).ToString(CultureInfo.InvariantCulture) + " %";
			}
			else
			{
				Message = "Saving videos to database...";
				IsIndeterminate = true; //when all posters are downloaded --> saving to database starts --> undeterminate how much time this will take
			}
		}

		void BgwInsertVideosRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			_progressWindow.Close();
		}

		private ProgressbarWindow _progressWindow;
		private int _value;
		private int _maximum;
		private string _message;
		private bool _isSaveEnabled;
		public bool IsIndeterminate { get; set; }

		public int Value
		{
			get { return _value; }
			set
			{
				_value = value;
				OnPropertyChanged("Value");
			}
		}

		public int Maximum
		{
			get { return _maximum; }
			set
			{
				_maximum = value;
				OnPropertyChanged("Maximum");
			}
		}

		public string Message
		{
			get { return _message; }
			set
			{
				_message = value;
				OnPropertyChanged("Message");
			}
		}

		public Boolean IsSaveEnabled
		{
			get { return _isSaveEnabled; }
			set
			{
				_isSaveEnabled = value;
				OnPropertyChanged("IsSaveEnabled");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
