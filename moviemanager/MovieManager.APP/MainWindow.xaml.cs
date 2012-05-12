using System.Collections.Generic;
using System.IO;
using System.Windows;
using Model;
using MovieManager.APP.Panels;
using System.Windows.Data;

namespace MovieManager.APP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        readonly MainController _controller;

        private static MainWindow _instance;
        public static MainWindow Instance
        {
            get { return _instance; }
        }
        public MainWindow()
        {
            InitializeComponent();

            _controller = MainController.Instance;


            DataContext = _controller;
            Loaded += MainWindowLoaded;

            _instance = this;
        }

        void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            _controller.FilterEditor = _FilterEditor;
        }

        #region ContextMenu event handlers
        private void MenuItemPropertiesClick(object sender, RoutedEventArgs e)
        {
            Video SelectedVideo = (_videoGrid.SelectedItem as Video);

            VideoEditor Editor = new VideoEditor { DataContext = SelectedVideo };

            Binding VideoBinding = new Binding { Source = SelectedVideo, Path = new PropertyPath("."), Mode = BindingMode.TwoWay };

            Editor.SetBinding(VideoEditor.VIDEO_PROPERTY, VideoBinding);
            Editor.Show();
        }

        private void MenuItemPlayClick(object sender, RoutedEventArgs e)
        {
            var Video = _videoGrid.SelectedItem as Video;
            if (Video != null && Video.Path != null)
            {
//<<<<<<< .mine
//                //TODO 099 enable this again after remake of vlc
//                //VlcWinForm Vlc = new VlcWinForm();
//                //Vlc.Show();
//                //Vlc.PlayVideo(Video);
//=======
//                MMPlayer Vlc = new MMPlayer();
//                Vlc.Show();
//                Vlc.PlayVideo(Video);
//>>>>>>> .r76
            }
        }

        private void MenuItemRenameFileClick(object sender, RoutedEventArgs e)
        {
            if (_videoGrid.SelectedItems != null)
            {
                foreach (Video Video in _videoGrid.SelectedItems)
                {
                    try
                    {
                        string NewVideoName = Path.GetFileName(Video.Path); ;
                        string VideoDir = Path.GetDirectoryName(Video.Path);
                        if (Video is Movie)
                        {
                            string ParString = Properties.Settings.Default.RenamingMovieFileSequence;
                            NewVideoName = ParString.Replace("{{MovieName}}", Video.Name);
                            NewVideoName = NewVideoName.Replace("{{Year}}", Video.Release.Year.ToString());
                        }

                        else if (Video is Episode)
                        {
                            string ParString = Properties.Settings.Default.RenamingEpisodeFileSequence;
                            //TODO 030: implement renaming for episodes
                        }

                        if (!string.IsNullOrEmpty(VideoDir) && File.Exists(Video.Path) && Directory.Exists(VideoDir))
                        {
                            string NewPath = Path.Combine(VideoDir, NewVideoName + Path.GetExtension(Video.Path));
                            File.Move(Video.Path, NewPath);
                            Video.Path = NewPath;
                        }

                        _videoGrid.Items.Refresh();
                    }
                    catch
                    {

                    }
                }
            }
        }

        #endregion

        private void VideoGridMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MenuItemPlayClick(sender, e);
        }


    }
}

//TODO 100: Filter Movies
//TODO 090: gezochte folders bijhouden + (automatische refresh in background)
//TODO 070: apparte settingsfile
//TODO 070: verschillende video types: episode, movie, ...
//TODO 050: tmdb search engine
