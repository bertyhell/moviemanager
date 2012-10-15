using System.Windows.Controls;

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
            progressbar.DataContext = _controller;
        }

        private void BtnAnalyseClick(object sender, System.Windows.RoutedEventArgs e)
        {
            _controller.BeginAnalyse();
        }
        
        private void BtnSaveClick(object sender, System.Windows.RoutedEventArgs e)
        {
            _controller.SaveVideos();
            Close();
        }

        private void DgrVideoFileListRowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            _controller.SelectedVideoFile.AnalyseNeeded = true;
        }

        private void BtnDetailsClick(object sender, System.Windows.RoutedEventArgs e)
        {
            SuggestionsWindow Window = new SuggestionsWindow(_controller.SelectedVideoFile);
            Window.Show();
        }
    }
}
