using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Common;
using Model;
using MovieManager.BL.Search;
using SQLite;

namespace MovieManager.APP.Panels.Analyse
{
    class AnalyseController : INotifyPropertyChanged
    {
        public AnalyseController()
        {
            Videos.Clear();
            MMDatabase.SelectAllVideos(Videos);
            
            VideoInfos.Add(new Movie { Name = "test1" });
            VideoInfos.Add(new Movie { Name = "test2" });
            VideoInfos.Add(new Movie { Name = "test3" });
        }

        private ObservableCollection<Video> _videos = new ObservableCollection<Video>();
        public ObservableCollection<Video> Videos
        {
            get
            {
                return _videos;
            }
            set
            {
                _videos = value;
            }
        }

        private Video _selectedVideoFile;
        public Video SelectedVideoFile
        {
            get { return _selectedVideoFile; }
            set
            {
                _selectedVideoFile = value;
                PropChanged("SelectedVideoFile");
            }
        }

        public void PropChanged(string title)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(title));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void ManualSearch(string text, string number)
        {
            VideoInfos.Clear();
            foreach (var Video in SearchTMDB.GetVideoInfo(text))
            {
                VideoInfos.Add(Video);
            }
        }

        private object _selectedMovieInfo;
        public object SelectedMovieInfo
        {
            get { return _selectedMovieInfo; }
            set
            {
                _selectedMovieInfo = value;
                PropChanged("SelectedMovieInfo");
            }
        }
        private ObservableCollection<Video> _videoInfos = new ObservableCollection<Video>();
        public ObservableCollection<Video> VideoInfos
        {
            get
            {
                return _videoInfos;
            }
            set
            {
                _videoInfos = value;
            }
        }

        //TODO 050 make analyse function multithreaded -> 1 thread for every movie lookup
    }
}
