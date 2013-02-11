using System.ComponentModel;
using Tmc.SystemFrameworks.Log;

namespace Tmc.WinUI.Application.Panels.Settings
{
    /// <summary>
    /// Interaction logic for LoggingSettingsPanel.xaml
    /// </summary>
    public partial class LoggingSettingsPanel : SettingsPanelBase, INotifyPropertyChanged
    {
        private bool _loggingEnabled;
        private LogLevelEnum _selectedLogLevel;

        public LoggingSettingsPanel()
        {
            InitializeComponent();

            LoggingEnabled = Properties.Settings.Default.Log_enabled;
            SelectedLogLevel = Properties.Settings.Default.Log_Level;
            _panelName = "Logging";
            _iconPath = "/Tmc.WinUI.Application;component/Images/log_32.png";
            DataContext = this;
        }

        public bool LoggingEnabled
        {
            get { return _loggingEnabled; }
            set
            {
                _loggingEnabled = value;
                OnPropChanged("LoggingEnabled");
            }
        }

        public LogLevelEnum SelectedLogLevel
        {
            get { return _selectedLogLevel; }
            set { _selectedLogLevel = value;
                OnPropChanged("SelectedLogLevel");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropChanged(string field)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(field));
        }

        public override bool SaveSettings()
        {
            Properties.Settings.Default.Log_enabled = LoggingEnabled;
            Properties.Settings.Default.Log_Level = SelectedLogLevel;
            return base.SaveSettings();
        }
    }
}
