using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Model;
using SQLite;

namespace MovieManager.APP.Panels.Analyse
{
    class AnalyseController : INotifyPropertyChanged
    {
        public AnalyseController()
        {
            Videos.Clear();
            MMDatabase.SelectAllVideos(Videos);
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

        private Video _selectedItem;
        public Video SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                PropChanged("SelectedItem");
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
    }
}
