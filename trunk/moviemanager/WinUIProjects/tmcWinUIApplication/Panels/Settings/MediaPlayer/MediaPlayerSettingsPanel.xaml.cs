using Tmc.WinUI.Player.Logic;

namespace Tmc.WinUI.Application.Panels.Settings.MediaPlayer
{
    /// <summary>
    /// Interaction logic for MediaPlayerSettingsPanel.xaml
    /// </summary>
    public partial class MediaPlayerSettingsPanel : SettingsPanelBase
    {
        private readonly MediaPlayerSettingsViewModel _model;
        public MediaPlayerSettingsPanel()
        {
            InitializeComponent();

            //initialize base class variables;
            _panelName = "Media player";
            _iconPath = "/MovieManager;component/Images/MediaPlayer_32.png";
            MediaPlayerSettings MediaPlayerSettings = Properties.Settings.Default.MediaPlayerSettings ?? new MediaPlayerSettings();
	        _model = new MediaPlayerSettingsViewModel
                                                     {
                                                         PlayOnDoubleCLick = Properties.Settings.Default.MediaPlayerPlayOnDoubleClick,
                                                         MediaPlayerSettings = MediaPlayerSettings
                                                     };
            DataContext = _model;
            

            _settingsPanels.Add(new InternalMediaPlayerSettingsPanel());
        }


        public override bool SaveSettings()
        {
            Properties.Settings.Default.MediaPlayerPlayOnDoubleClick = _model.PlayOnDoubleCLick;
            Properties.Settings.Default.MediaPlayerSettings = _model.MediaPlayerSettings;
            return true;
        }
    }
}
