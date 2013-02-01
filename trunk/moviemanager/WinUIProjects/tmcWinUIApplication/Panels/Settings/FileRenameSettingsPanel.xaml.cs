using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Tmc.WinUI.Application.Panels.Settings
{
    /// <summary>
    /// Interaction logic for FileRenameSettingsPanelBase.xaml
    /// </summary>
    public partial class FileRenameSettingsPanel
    {


        public FileRenameSettingsPanel()
        {
            InitializeComponent();

            //initialize base class variables;
            _panelName = "File rename";
            _iconPath = "/MovieManager;component/Images/disk.png";

            //MovieInfo string builder
            parameteredStringBuilderMovie.Parameters = new List<FrameworkElement> { new TextBox { Text = "Custom" }, new Button { Content = "MovieName" }, new Button { Content = "Year" } };

            //Serie string builder
            parameteredStringBuilderEpisode.Parameters = new List<FrameworkElement> { new TextBox { Text = "Custom" }, new Button { Content = "EpisodeName" }, new Button { Content = "Extension" }, new Button { Content = "Season" }, new Button { Content = "EpisodeNumber" } };

            LoadSettings();
        }

        private void LoadSettings()
        {
            parameteredStringBuilderEpisode.ParameteredString = Properties.Settings.Default.RenamingEpisodeFileSequence;
            parameteredStringBuilderMovie.ParameteredString = Properties.Settings.Default.RenamingMovieFileSequence;
        }

        public override bool SaveSettings()
        {
            Properties.Settings.Default.RenamingMovieFileSequence = parameteredStringBuilderMovie.ParameteredString;
            Properties.Settings.Default.RenamingEpisodeFileSequence = parameteredStringBuilderEpisode.ParameteredString;
            return true;
        }
    }
}
