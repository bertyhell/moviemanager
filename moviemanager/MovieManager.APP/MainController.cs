using System;
using System.Collections.Generic;
using Model;
using SQLite;
using System.ComponentModel;

namespace MovieManager.APP
{
    public class MainController: IDisposable, INotifyPropertyChanged
    {

        public MainController()
        {
            Videos = MMDatabase.SelectAllVideos();
            MMDatabase.OnVideosChanged += MMDatabase_onVideoChanged;
        }


        private List<Video> _videos;
        public List<Video> Videos
        {
            get
            {
                return _videos;
            }
            set
            {
                _videos = value;
                PropChanged("Videos");
            }
        }

        void MMDatabase_onVideoChanged()
        {
            Videos = MMDatabase.SelectAllVideos();
        }



        public void Dispose()
        {
            MMDatabase.OnVideosChanged -= MMDatabase_onVideoChanged;
        }

        public void PropChanged(String title)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(title));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
