using System.Collections.Generic;
using System.Windows;

namespace MovieManager.APP.Panels.Settings
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow
    {
        private List<SettingsPanelBase> _settingsPanels;

        public SettingsWindow(List<SettingsPanelBase> settingsPanels)
        {
            InitializeComponent();
            _settingsPanels = settingsPanels;
            _trvSettingPanels.DataContext = settingsPanels;
            _trvSettingPanels.SelectedItemChanged += new System.Windows.RoutedPropertyChangedEventHandler<object>(_trvSettingPanels_SelectedItemChanged);
        }

        void _trvSettingPanels_SelectedItemChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<object> e)
        {
          _settingsPanelHolder.Children.Clear();
            _settingsPanelHolder.Children.Add((UIElement) _trvSettingPanels.SelectedValue);
        }

        public List<SettingsPanelBase> SettingsPanels
        {
            get { return _settingsPanels; }
        }

        private void _btnSave_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            foreach (SettingsPanelBase SettingsPanel in _settingsPanels)
            {
                SettingsPanel.SaveSettings();
            }
        }
    }
}
