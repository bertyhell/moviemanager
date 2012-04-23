using System.Collections.Generic;
using System.Windows;

namespace MovieManager.APP.Panels.Settings
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow
    {
        private readonly List<SettingsPanelBase> _settingsPanels;

        public SettingsWindow(List<SettingsPanelBase> settingsPanels)
        {
            InitializeComponent();
            _settingsPanels = settingsPanels;
            _trvSettingPanels.DataContext = settingsPanels;
            _trvSettingPanels.SelectedItemChanged += TrvSettingPanelsSelectedItemChanged;
        }

        void TrvSettingPanelsSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
          _settingsPanelHolder.Children.Clear();
            _settingsPanelHolder.Children.Add((UIElement) _trvSettingPanels.SelectedValue);
        }

        public List<SettingsPanelBase> SettingsPanels
        {
            get { return _settingsPanels; }
        }

        private void BtnSaveClick(object sender, RoutedEventArgs e)
        {
            foreach (SettingsPanelBase SettingsPanel in _settingsPanels)
            {
                SettingsPanel.SaveSettings();
            }
        }
    }
}
