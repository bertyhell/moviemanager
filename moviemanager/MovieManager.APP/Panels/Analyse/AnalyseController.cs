using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Common;
using Model;
using SQLite;

namespace MovieManager.APP.Panels.Analyse
{
    class AnalyseController : INotifyPropertyChanged
    {

        public AnalyseController()
        {
            //TODO 030 use videos from maincontroller, don't recollect from database
            IList<Video> Videos = new List<Video>();
            MMDatabase.SelectAllVideos(Videos);
            AnalyseVideos.Clear();
            foreach (Video Video in Videos)
            {
                AnalyseVideos.Add(new AnalyseVideo { Video = Video });
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

        public void BeginAnalyse()
        {
            //begin automatic analysis

            var AnalyseWorker = new AnalyseWorker(AnalyseVideos);
            IsIndeterminate = true;
            Message = "Contacting webservice...";
            AnalyseWorker.VideoInfoProgress += AnalyseWorker_VideoInfoProgress;
            AnalyseWorker.RunWorkerCompleted += AnalyseWorker_RunWorkerCompleted;
            AnalyseWorker.RunWorkerAsync();
        }

        public void AnalyseWorker_VideoInfoProgress(object sender, ProgressEventArgs args)
        {
            VideoProgressHandler(args);
        }

        private void VideoProgressHandler(ProgressEventArgs args)
        {
            IsIndeterminate = false;
            Message = "Analysing videos: " + args.ProgressNumber + " / " + args.MaxNumber;
            Maximum = args.MaxNumber;
            Value = args.ProgressNumber;
        }

        public void AnalyseWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //TODO 050 get posters of analysed videos
            Console.WriteLine("finished analysing :D");
        }

        public void PropChanged(string title)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(title));
            }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                PropChanged("Message");
            }
        }

        private int _value;
        public int Value
        {
            get { return _value; }
            set
            {
                _value = value;
                PropChanged("Value");
            }
        }

        private int _maximum = 100;
        public int Maximum
        {
            get { return _maximum; }
            set
            {
                _maximum = value;
                PropChanged("Maximum");
            }
        }

        private bool _isIndeterminate;
        public bool IsIndeterminate
        {
            get { return _isIndeterminate; }
            set
            {
                _isIndeterminate = value;
                PropChanged("IsIndeterminate");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        //public void ManualSearch(string text, string number)
        //{
        //    VideoInfos.Clear();
        //    foreach (var Video in SearchTMDB.GetVideoInfo(text))
        //    {
        //        VideoInfos.Add(Video);
        //    }
        //}


        //TODO 050 make analyse function multithreaded -> 1 thread for every movie lookup
    }
}
