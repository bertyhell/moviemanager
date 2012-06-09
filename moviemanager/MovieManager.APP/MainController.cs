using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using Model;
using SQLite;
using System.ComponentModel;
using MovieManager.APP.Panels.Filter;

namespace MovieManager.APP
{
    public class MainController : INotifyPropertyChanged
    {
        private static readonly MainController INSTANCE = new MainController();

        public static MainController Instance
        {
            get { return INSTANCE; }
        }

        private MainController()
        {
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            Videos.Clear();
            MMDatabase.SelectAllVideos(Videos);
            _videosView = CollectionViewSource.GetDefaultView(Videos);

            MMDatabase.VideosChanged += new MMDatabase.VideosChangedDel(MMDatabase_VideosChanged);

            //init log
            //new GlobalLogger().EnableLogger();
        }

        void MMDatabase_VideosChanged()
        {
            ReloadVideos();
        }
        
        private ICollectionView _videosView;
        public ICollectionView VideosView
        {
            get { return _videosView; }
            set
            {
                _videosView = value;
                PropChanged("VideosView");
            }
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

        private FilterEditor _filterEditor;
        public FilterEditor FilterEditor
        {
            get { return _filterEditor; }
            set
            {
                _filterEditor = value;
                _videosView.Filter += FilterEditor.FilterVideo;
            }
        }

        public void ReloadVideos()
        {
            Videos.Clear();
            MMDatabase.SelectAllVideos(Videos);
        }

        public void Refresh()
        {
            VideosView.Refresh();
        }

        public void PropChanged(String field)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(field));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
