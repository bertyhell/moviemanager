using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using SQLite;
using System.Data.SQLite;
using System.ComponentModel;

namespace MovieManager.APP
{
    public class MainController: IDisposable, INotifyPropertyChanged
    {

        public MainController()
        {
            Videos = MMDatabase.selectAllVideos();
            MMDatabase.onVideosChanged += new VideosChanged(MMDatabase_onVideoChanged);
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
                propChanged("Videos");
            }
        }

        void MMDatabase_onVideoChanged()
        {
            Videos = MMDatabase.selectAllVideos();
        }



        public void Dispose()
        {
            MMDatabase.onVideosChanged -= new VideosChanged(MMDatabase_onVideoChanged);
        }

        public void propChanged(String title)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(title));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
