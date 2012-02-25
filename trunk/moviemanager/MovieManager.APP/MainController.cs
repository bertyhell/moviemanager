﻿using System;
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
    public class MainController : IDisposable, INotifyPropertyChanged
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
            MMDatabase.OnVideosChanged += MMDatabaseOnVideoChanged;
             _videosView = CollectionViewSource.GetDefaultView(Videos);
        }



        private ICollectionView _videosView;
        public ICollectionView VideosView
        {
            get { return _videosView; }
            set { _videosView = value;
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
                PropChanged("Videos");
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

        void MMDatabaseOnVideoChanged()
        {
            _videos.Clear();
            MMDatabase.SelectAllVideos(_videos);
        }



        public void Dispose()
        {
            MMDatabase.OnVideosChanged -= MMDatabaseOnVideoChanged;
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