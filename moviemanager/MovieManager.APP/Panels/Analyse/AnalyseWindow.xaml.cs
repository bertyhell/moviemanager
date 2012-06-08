using System.Windows.Controls;
using Model;
using SQLite;

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

        private void dgrVideoFileList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            AnalyseVideo AnalyseVideo = (AnalyseVideo) ((DataGrid) sender).SelectedItem;
            SuggestionsWindow Window = new SuggestionsWindow(AnalyseVideo);
            Window.ShowDialog();
        }

        private void BtnSaveClick(object sender, System.Windows.RoutedEventArgs e)
        {
            _controller.SaveVideos();
            this.Close();
        }
    }
}
