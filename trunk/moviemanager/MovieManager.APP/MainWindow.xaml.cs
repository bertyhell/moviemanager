using System.Windows;
using Model;
using MovieManager.APP.CommonControls;

namespace MovieManager.APP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly MainController _context;

        public MainWindow()
        {
            InitializeComponent();


            _context = MainController.Instance;
            this.DataContext = _context;
        }

        #region ContextMenu event handlers
        private void MenuItemProperties_Click(object sender, RoutedEventArgs e)
        {
            VideoEditor Editor = new VideoEditor();
            Editor.Video = (_videoGrid.SelectedItem as Video);
            Editor.Show();
        }

        private void MenuItemPlay_Click(object sender, RoutedEventArgs e)
        {
            VlcWindow Vlc = new VlcWindow();
            Vlc.Show();
        }
        #endregion
        
    }
}
