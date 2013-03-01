using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Tmc.DataAccess.SqlCe;
using Tmc.SystemFrameworks.Common;
using Tmc.SystemFrameworks.Log;
using Tmc.SystemFrameworks.Model;
using Tmc.WinUI.Application.Cache;
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
					AnalyseVideos.Add(new AnalyseVideo { Video = Video, TitleGuesses = new List<string> {Video.Name} });
				}
            }
        }

        private ObservableCollection<AnalyseVideo> _analyseVideos = new ObservableCollection<AnalyseVideo>();
        public ObservableCollection<AnalyseVideo> AnalyseVideos
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
                PropChanged("SelectedVideoFile");
            }
        }

        public void BeginAnalyse(bool fullAnalyse = true)
        {
            //begin automatic analysis

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
            //TODO 050 get posters of analysed videos

            GlobalLogger.Instance.MovieManagerLogger.Info(GlobalLogger.FormatExceptionForLog("AnalyseController", "AnalyseWorkerRunWorkerCompleted", "Analyse completed"));
        }

        public ProgressBarInfo ProgressBarInfoTotal { get; set; }
        public ProgressBarInfo ProgressBarInfoPass { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void PropChanged(string title)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(title));
            }
        }

        //TODO 050 make analyse function multithreaded -> 1 thread for every MovieInfo lookup
        public void SaveVideos()//TODO 070 do this in a backgroundworker with progressbar
        {
            var Videos = new List<Video>();
            foreach (var AnalyseVideo in AnalyseVideos)
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
                    ApplicationCache.AddVideoImages(AnalyseVideo.Video.Id, Images, CacheImageType.Images, ImageQuality.Medium);//TODO 070 move this to a seperate backgroundworker


                    Videos.Add(Video); //changed this
                }
            }
            DataRetriever.Videos = Videos;
        }
    }
}
