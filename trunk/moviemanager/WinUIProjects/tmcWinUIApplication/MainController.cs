using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Data;
using Model;
using System.ComponentModel;
using Tmc.DataAccess.Sqlite;
using Tmc.SystemFrameworks.Common;
using Tmc.WinUI.Application.Commands;
using Tmc.WinUI.Application.Panels.Filter;
using Tmc.WinUI.Application.Panels.Settings;
using Tmc.WinUI.Application.Properties;
using Tmc.SystemFrameworks.Log;

namespace Tmc.WinUI.Application
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

            _videos = new ObservableCollection<Video>();
            ReloadVideos();
            _videosView = CollectionViewSource.GetDefaultView(Videos);

            TmcDatabase.VideosChanged += MMDatabaseVideosChanged;

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

        private ObservableCollection<Video> _videos;
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

        private List<Video> _videosList;
        public List<Video> VideosList
        {
            get { return _videosList; }
            set
            {
                _videosList = value;
            }
        }

        private FilterEditor _filterEditor;

        public FilterEditor FilterEditor
        {
            get { return _filterEditor; }
            set
            {
                if (_filterEditor != value)
                {
                    _filterEditor = value;
                    _videosView.Filter += FilterEditor.FilterVideo;
                }
            }
        }

        private MainWindow _windowInstance;

        public MainWindow WindowInstance
        {
            set { _windowInstance = value; }
        }

        public void ReloadVideos()
        {
            try
            {
                //Videos = new AsyncVirtualizingCollection<T>(new ItemsProvider(), 100, 1000);//TODO 020 adjust pagesize to zoom level video preview items
                _videosList = (List<Video>) TmcDatabase.SelectAllVideos();
                _videos.Clear();
                foreach (Video Video in _videosList)
                {
                    _videos.Add(Video);
                }
            }
            catch (Exception Ex)
            {
                GlobalLogger.Instance.MovieManagerLogger.Fatal(GlobalLogger.FormatExceptionForLog("MainWindow", "ReloadVideos", Ex));
            }
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
        public Visibility IsDetailViewVisible
        {
            get { return _isDetailViewVisible; }
            set
            {
                if (_isDetailViewVisible != value)
                {
                    _isDetailViewVisible = value;
                    PropChanged("IsDetailViewVisible");
                }
            }
        }

        private Visibility _isIconsViewVisible;

        public Visibility IsIconsViewVisible
        {
            get { return _isIconsViewVisible; }
            set
            {
                if (_isIconsViewVisible != value)
                {
                    _isIconsViewVisible = value;
                    PropChanged("IsIconsViewVisible");
                }
            }
        }

        public void ChangeView(ViewStates requestedViewState)
        {
            if (requestedViewState == ViewStates.Details)
            {
                IsDetailViewVisible = Visibility.Visible;
                IsIconsViewVisible = Visibility.Collapsed;
            }
            else
            {
                IsDetailViewVisible = Visibility.Collapsed;
                IsIconsViewVisible = Visibility.Visible;
                int NewPreviewWidth = _previewWidth;
                switch (requestedViewState)
                {
                    case ViewStates.BigIcons:
                        NewPreviewWidth = DefaultValues.PREVIEW_MAX_WIDTH;
                        break;
                    case ViewStates.MediumIcons:
                        NewPreviewWidth = (DefaultValues.PREVIEW_MIN_WIDTH + DefaultValues.PREVIEW_MAX_WIDTH) / 2;
                        break;
                    case ViewStates.SmallIcons:
                        NewPreviewWidth = DefaultValues.PREVIEW_MIN_WIDTH;
                        break;
                    default:
                        break;
                }

                if (_previewWidth != NewPreviewWidth)
                {
                    _previewWidth = NewPreviewWidth;
                    PropChanged("PreviewWidth");
                    PropChanged("PreviewHeight");
                }
            }
        }

        private int _previewItemMargin = 5;
        public int PreviewItemMargin
        {
            get { return _previewItemMargin; }
            set
            {
                if (_previewItemMargin != value)
                {
                    _previewItemMargin = value;
                    PropChanged("PreviewItemMargin");
                }
            }
        }

        #region zooming + events


        private int _previewWidth = 200;

        public int PreviewWidth
        {
            get { return _previewWidth; }
            set
            {
                //TODO 020 zoomout on details and then zoom in --> slow or error?
                int NewPreviewWidth = _previewWidth;
                //if change needed to detail view
                if (value < DefaultValues.PREVIEW_MIN_WIDTH && _previewWidth >= DefaultValues.PREVIEW_MIN_WIDTH)
                {
                    ChangeView(ViewStates.Details);
                    NewPreviewWidth = DefaultValues.PREVIEW_MIN_WIDTH - DefaultValues.PREVIEW_ZOOM_STEP;
                }
                //if change needed to poster view
                else if (value >= DefaultValues.PREVIEW_MIN_WIDTH && _previewWidth < DefaultValues.PREVIEW_MIN_WIDTH)
                {
                    ChangeView(ViewStates.SmallIcons);
                    NewPreviewWidth = DefaultValues.PREVIEW_MIN_WIDTH;
                }
                //if posters become to big
                else if (value > DefaultValues.PREVIEW_MAX_WIDTH)
                {
                    NewPreviewWidth = DefaultValues.PREVIEW_MAX_WIDTH;
                }
                //if normal poster resize
                else if (value >= DefaultValues.PREVIEW_MIN_WIDTH && value <= DefaultValues.PREVIEW_MAX_WIDTH)
                {
                    NewPreviewWidth = value;
                }

                //check if value has changed and if so commit value
                if (_previewWidth != NewPreviewWidth)
                {
                    _previewWidth = NewPreviewWidth;
                    PropChanged("PreviewWidth");
                    PropChanged("PreviewHeight");
                }
            }
        }

        public int PreviewHeight
        {
            get { return (int)(_previewWidth * 1.5); }
        }

        private Visibility _previewTitleVisibility;
        public Visibility PreviewTitleVisibility
        {
            get { return _previewTitleVisibility; }
            set
            {
                if (_previewTitleVisibility != value)
                {
                    _previewTitleVisibility = value;
                    PropChanged("PreviewTitleVisibility");
                }
            }
        }

        public void Zoom(bool zoomIn)
        {
            if (zoomIn)
            {
                PreviewWidth += DefaultValues.PREVIEW_ZOOM_STEP;
            }
            else
            {
                PreviewWidth -= DefaultValues.PREVIEW_ZOOM_STEP;
            }
        }

        #endregion


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
