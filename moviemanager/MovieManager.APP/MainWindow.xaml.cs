using System.Windows;
using Model;
using MovieManager.APP.Panels;
using VlcPlayer;
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
            Video selectedVideo = (_videoGrid.SelectedItem as Video);

            VideoEditor editor = new VideoEditor();
            editor.DataContext = selectedVideo;

            Binding videoBinding = new Binding();
            videoBinding.Source = selectedVideo;
            videoBinding.Path = new PropertyPath(".");
            videoBinding.Mode = BindingMode.TwoWay;

            editor.SetBinding(VideoEditor.VideoProperty, videoBinding);
            editor.Show();
        }

        private void MenuItemPlayClick(object sender, RoutedEventArgs e)
        {
            string path = (_videoGrid.SelectedItem as Video).Path;

            if (path != null)
            {
                VlcWinForm vlc = new VlcWinForm();
                vlc.Show();
                vlc.PlayVideo(path);
            }
        }
        #endregion

        private void _videoGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
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
