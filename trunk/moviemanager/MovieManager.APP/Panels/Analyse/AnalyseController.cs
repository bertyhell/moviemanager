using System.Collections.ObjectModel;
using Model;
using SQLite;

namespace MovieManager.APP.Panels.Analyse
{
    class AnalyseController
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
    }
}
