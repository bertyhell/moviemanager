using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MovieManager.LOG;

namespace MovieManager.APP.Panels.Settings
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

            LoggingEnabled = APP.Properties.Settings.Default.Log_enabled;
            SelectedLogLevel = APP.Properties.Settings.Default.Log_Level;
            _panelName = "Logging";
            _iconPath = "/MovieManager;component/Images/log_32.png";
            this.DataContext = this;
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
            APP.Properties.Settings.Default.Log_enabled = LoggingEnabled;
            APP.Properties.Settings.Default.Log_Level = SelectedLogLevel;
            return base.SaveSettings();
        }
    }
}
