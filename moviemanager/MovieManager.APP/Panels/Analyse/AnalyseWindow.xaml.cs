namespace MovieManager.APP.Panels.Analyse
{
    /// <summary>
    /// Interaction logic for AnalysePanel.xaml
    /// </summary>
    public partial class AnalyseWindow
    {
        private readonly AnalyseController _controller;

        public AnalyseWindow()
        {
            InitializeComponent();
            _controller = new AnalyseController();
            DataContext = _controller;
        }
        
        private void BtnAnalyseClick(object sender, System.Windows.RoutedEventArgs e)
        {
            _controller.BeginAnalyse();
        }
    }
}
