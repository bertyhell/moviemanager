using System.Windows;
using MovieManager.APP.Panels.AddVideos;
using SQLite.RegexSettings;

namespace MovieManager.APP.Panels.RegularExpressions
{
    /// <summary>
    /// Interaction logic for EpisodeRegexEditor.xaml
    /// </summary>
    public partial class EpisodeRegexEditor : Window
    {
        private EpisodeRegexEditorViewModel  _viewModel = new EpisodeRegexEditorViewModel();
        private AddRegex _regex;

        public EpisodeRegexEditor()
        {
            InitializeComponent();
            this.DataContext = _viewModel;
            _viewModel.RegularExpressions = RegexSettingsStorage.EpisodeRegularExpressions;
        }

        private void _btnSaveSettings_Click(object sender, RoutedEventArgs e)
        {
            RegexSettingsStorage.SaveSettings();
        }

        private void _btnUp_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.MoveRegExUp(_lstRegex.SelectedIndex);
        }

        private void _btnDown_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.MoveRegExDown(_lstRegex.SelectedIndex);
        }

        private void _btnNew_Click(object sender, RoutedEventArgs e)
        {
            _regex = new AddRegex();
            _regex.Closing += new System.ComponentModel.CancelEventHandler(Regex_Closing);
            _regex.ShowDialog();
        }

        void Regex_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(_regex.DialogResult != null && (bool)_regex.DialogResult)
            {
                _viewModel.RegularExpressions.Add(_regex.RegularExpression);
            }
            _regex.Closing -= new System.ComponentModel.CancelEventHandler(Regex_Closing);
        }

        private void _btnRemove_Click(object sender, RoutedEventArgs e)
        {

            if (_viewModel.SelectedRegularExpression != null)
            {
                _viewModel.RegularExpressions.Remove(_viewModel.SelectedRegularExpression);
            }
        }


    }
}
