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


            _context = new MainController();
            this.DataContext = _context;
        }

        private void MenuItemProperties_Click(object sender, RoutedEventArgs e)
        {
            VideoEditor Editor = new VideoEditor();
            Editor.Video = (_videoGrid.SelectedItem as Video);
            Editor.Show();
        }
    }
}
