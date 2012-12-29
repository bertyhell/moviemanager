namespace Tmc.WinUI.Application.Panels.Settings.MediaPlayer
{
    /// <summary>
    /// Interaction logic for InternalMediaPlayerSettingsPanel.xaml
    /// </summary>
    public partial class InternalMediaPlayerSettingsPanel : SettingsPanelBase
    {

        public InternalMediaPlayerSettingsPanel()
        {
            InitializeComponent();

            //initialize base class variables;
            _panelName = "MMPlayer";
            _iconPath = "/MovieManager;component/Images/picto.png";
        }

        public override bool SaveSettings()
        {
            return true;
        }
    }
}
