using System.Windows;
using Tmc.DataAccess.Sqlite.RegexSettings;

namespace MovieManager.APP.Panels.RegularExpressions
{
    /// <summary>
    /// Interaction logic for EpisodeRegexEditor.xaml
    /// </summary>
    public partial class EpisodeRegexEditor
    {
        private readonly EpisodeRegexEditorViewModel  _viewModel = new EpisodeRegexEditorViewModel();
        private AddRegex _regex;

        public EpisodeRegexEditor()
        {
            InitializeComponent();
            DataContext = _viewModel;
            _viewModel.RegularExpressions = RegexSettingsStorage.EpisodeRegularExpressions;
        }

        private void BtnSaveSettingsClick(object sender, RoutedEventArgs e)
        {
            RegexSettingsStorage.SaveSettings();
        }

        private void BtnUpClick(object sender, RoutedEventArgs e)
        {
            _viewModel.MoveRegExUp(_lstRegex.SelectedIndex);
        }

        private void BtnDownClick(object sender, RoutedEventArgs e)
        {
            _viewModel.MoveRegExDown(_lstRegex.SelectedIndex);
        }

        private void BtnNewClick(object sender, RoutedEventArgs e)
        {
            _regex = new AddRegex();
            _regex.Closing += RegexClosing;
            _regex.ShowDialog();
        }

        void RegexClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(_regex.DialogResult != null && (bool)_regex.DialogResult)
            {
                _viewModel.RegularExpressions.Add(_regex.RegularExpression);
            }
            _regex.Closing -= RegexClosing;
        }

        private void BtnRemoveClick(object sender, RoutedEventArgs e)
        {

            if (_viewModel.SelectedRegularExpression != null)
            {
                _viewModel.RegularExpressions.Remove(_viewModel.SelectedRegularExpression);
            }
        }


    }
}
