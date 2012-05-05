using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Model;
using MovieManager.WEB.Search;
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
            IList<Video> CandidatesTest = new List<Video> { new Video { Name = "test1" }, new Video { Name = "test2" }, new Video { Name = "test3" } };
            foreach (Video Video in Videos)
            {
                AnalyseVideos.Add(new AnalyseVideo { Video = Video, Candidates = CandidatesTest });
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
            AnalyseWorker.RunWorkerCompleted += AnalyseWorker_RunWorkerCompleted;
            AnalyseWorker.RunWorkerAsync();
        }

        void AnalyseWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("finished analysing :D");
        }

        public void PropChanged(string title)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(title));
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
