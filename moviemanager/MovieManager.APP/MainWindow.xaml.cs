using System.Windows;
using Model;
using MovieManager.APP.CommonControls;
using MovieManager.APP.Panels;
using VlcPlayer;
using System.Windows.Data;

namespace MovieManager.APP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly MainController _controller;

        public MainWindow()
        {
            InitializeComponent();


            _controller = MainController.Instance;


            DataContext = _controller;
            Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _controller.FilterEditor = _FilterEditor;
        }

        #region ContextMenu event handlers
        private void MenuItemProperties_Click(object sender, RoutedEventArgs e)
        {
            Video SelectedVideo = (_videoGrid.SelectedItem as Video);

            VideoEditor Editor = new VideoEditor();
            Editor.DataContext = SelectedVideo;

            Binding VideoBinding = new Binding();
            VideoBinding.Source = SelectedVideo;
            VideoBinding.Path = new PropertyPath(".");
            VideoBinding.Mode = BindingMode.TwoWay;

            Editor.SetBinding(VideoEditor.VideoProperty, VideoBinding);
            Editor.Show();
        }

        private void MenuItemPlay_Click(object sender, RoutedEventArgs e)
        {
            string Path = (_videoGrid.SelectedItem as Video).Path;

            if (Path != null)
            {
                VlcWinForm Vlc = new VlcWinForm();
                Vlc.Show();
                Vlc.PlayVideo(Path);
            }
        }
        #endregion

    }
}

//TODO 100: Filter Movies
//TODO 090: gezochte folders bijhouden + (automatische refresh in background)
//TODO 070: apparte settingsfile
//TODO 070: verschillende video types: episode, movie, ...
//TODO 050: tmdb search engine
