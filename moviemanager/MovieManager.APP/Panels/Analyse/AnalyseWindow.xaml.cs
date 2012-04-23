using System;
using Model;

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

        private void BtnManualSearchClick(object sender, System.Windows.RoutedEventArgs e)
        {
            _controller.ManualSearch(txtTitleGuess.Text, txtReleaseYearGuess.Text);
        }
        
        private void DgrVideoFileListSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            txtReleaseYearGuess.Text = ((Video)dgrVideoFileList.SelectedItem).ReleaseYearGuess;
        }
    }
}
