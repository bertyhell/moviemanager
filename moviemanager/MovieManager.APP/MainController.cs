using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Data;
using Common;
using Model;
using MovieManager.APP.Cache;
using MovieManager.APP.Commands;
using MovieManager.APP.Panels.Settings;
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
            //init database
            string DatabasePath = Properties.Settings.Default.DatabasePath;
            bool DatabaseUpdated = false;
            if (!File.Exists(DatabasePath))
            {
                EditSettingsCommand EditSettingsCommand = new EditSettingsCommand(typeof(DatabaseSettingsPanel));
                EditSettingsCommand.Execute(null);
                DatabaseUpdated = true;
            }
            string ConnectionString = Properties.Settings.Default.ConnectionString.Replace("{path}", DatabasePath);
            MMDatabase.Init(ConnectionString);
            if (!DatabaseUpdated)
                MMDatabaseCreation.ConvertDatabase(ConnectionString);

            Videos.Clear();
            MMDatabase.SelectAllVideos(Videos);
            _videosView = CollectionViewSource.GetDefaultView(Videos);

            MMDatabase.VideosChanged += MMDatabaseVideosChanged;
            //ApplicationCache.AddVideoImages(66, new List<Uri>() { new Uri("http://www.cathedral-design.be/upload/google-logo-voor-nieuws-5178_google_logo.jpg") }, CacheImageType.Images, ImageQuality.High);

            IsIconsViewVisible = Visibility.Visible;//TODO 020 check if any of the videos has been analysed --> none == hide iconsview
            IsDetailViewVisible = Visibility.Collapsed;
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

        private MainWindow _windowInstance;

        public MainWindow WindowInstance
        {
            set { _windowInstance = value; }
        }

        public void ReloadVideos()
        {
            Videos.Clear();
            MMDatabase.SelectAllVideos(Videos);
        }

        private delegate void ReloadVideosDelegate();

        void MMDatabaseVideosChanged()
        {
            _windowInstance.Dispatcher.Invoke(Delegate.CreateDelegate(typeof(ReloadVideosDelegate), this, "ReloadVideos"));
        }

        public void Refresh()
        {
            VideosView.Refresh();
        }

        private Visibility _isDetailViewVisible;
        public Visibility  IsDetailViewVisible
        {
            get { return _isDetailViewVisible; }
            set { _isDetailViewVisible = value;
            PropChanged("IsDetailViewVisible");
            }
        }

        private Visibility _isIconsViewVisible;
        public Visibility IsIconsViewVisible
        {
            get { return _isIconsViewVisible; }
            set
            {
                _isIconsViewVisible = value;
                PropChanged("IsIconsViewVisible");
            }
        }

        public void ToggleViews()
        {
            if(IsDetailViewVisible == Visibility.Collapsed)
            {
                IsDetailViewVisible = Visibility.Visible;
                IsIconsViewVisible = Visibility.Collapsed;
            }else
            {
                IsDetailViewVisible = Visibility.Collapsed;
                IsIconsViewVisible = Visibility.Visible;
                
            }
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
