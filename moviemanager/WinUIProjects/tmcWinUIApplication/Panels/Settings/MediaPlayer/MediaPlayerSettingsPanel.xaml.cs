using MovieManager.PLAYER.Logic;
using Tmc.WinUI.Application.Panels.Settings;
using Tmc.WinUI.Application.Panels.Settings.MediaPlayer;

namespace MovieManager.APP.Panels.Settings
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
            MediaPlayerSettings MediaPlayerSettings = Tmc.WinUI.Application.Properties.Settings.Default.MediaPlayerSettings;
            if(MediaPlayerSettings == null) MediaPlayerSettings = new MediaPlayerSettings();
            _model = new MediaPlayerSettingsViewModel
                                                     {
                                                         PlayOnDoubleCLick = Tmc.WinUI.Application.Properties.Settings.Default.MediaPlayerPlayOnDoubleClick,
                                                         MediaPlayerSettings = MediaPlayerSettings
                                                     };
            this.DataContext = _model;
            

            _settingsPanels.Add(new InternalMediaPlayerSettingsPanel());
        }


        public override bool SaveSettings()
        {
            Tmc.WinUI.Application.Properties.Settings.Default.MediaPlayerPlayOnDoubleClick = _model.PlayOnDoubleCLick;
            Tmc.WinUI.Application.Properties.Settings.Default.MediaPlayerSettings = _model.MediaPlayerSettings;
            return true;
        }
    }
}
