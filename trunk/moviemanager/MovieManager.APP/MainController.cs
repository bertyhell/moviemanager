﻿using System;
using System.Collections.Generic;
using System.Windows.Data;
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
            Videos = MMDatabase.SelectAllVideos();
            MMDatabase.OnVideosChanged += MMDatabase_onVideoChanged;
             _videosView = CollectionViewSource.GetDefaultView(Videos);
        }



        private ICollectionView _videosView;
        public ICollectionView VideosView
        {
            get { return _videosView; }
            set { _videosView = value;
                PropChanged("Videos");
            }
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
