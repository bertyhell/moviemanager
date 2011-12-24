using System.Windows;

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
    }
}
