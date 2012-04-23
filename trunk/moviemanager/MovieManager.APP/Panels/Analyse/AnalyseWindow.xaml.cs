using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

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
            _controller.ManualSearch(txtTitleGuess.Text, txtReleaseYearGuess.Number);
        }


    }
}
