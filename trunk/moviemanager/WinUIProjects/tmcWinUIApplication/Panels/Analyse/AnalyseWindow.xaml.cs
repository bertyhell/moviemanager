using System.Windows;
using System.Windows.Controls;

namespace Tmc.WinUI.Application.Panels.Analyse
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
            progressbarTotal.DataContext = _controller.ProgressBarInfoTotal;
            progressbarPass.DataContext = _controller.ProgressBarInfoPass;
        }

        private void BtnAnalyseClick(object sender, RoutedEventArgs e)
        {
            _controller.BeginAnalyse();
        }
        
        private void BtnSaveClick(object sender, RoutedEventArgs e)
        {
            _controller.SaveVideos();
            Close();
        }

        private void DgrVideoFileListRowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            _controller.SelectedVideoFile.AnalyseNeeded = true;
        }

        private void BtnDetailsClick(object sender, RoutedEventArgs e)
        {
            SuggestionsWindow Window = new SuggestionsWindow(_controller.SelectedVideoFile);
            Window.Show();
        }

        private void BtnQuickAnalyseClick(object sender, RoutedEventArgs e)
        {
            _controller.BeginAnalyse(false);
        }
    }
}
