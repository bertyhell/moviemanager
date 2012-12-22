using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace Tmc.WinUI.Application.Panels.Settings
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : INotifyPropertyChanged, IDisposable
    {
        private readonly List<SettingsPanelBase> _settingsPanels;
        private SettingsPanelBase _visiblePanel;

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
            _settingsPanelHolder.Children.Add((UIElement)_trvSettingPanels.SelectedValue);
        }

        public List<SettingsPanelBase> SettingsPanels
        {
            get { return _settingsPanels; }
        }

        public SettingsPanelBase VisiblePanel
        {
            get { return _visiblePanel; }
            set
            {
                _visiblePanel = value;
                OnPropChanged("VisiblePanel");
            }
        }

        private void BtnSaveClick(object sender, RoutedEventArgs e)
        {
            foreach (SettingsPanelBase SettingsPanel in _settingsPanels)
            {
                SettingsPanel.SaveSettings();
            }
            Tmc.WinUI.Application.Properties.Settings.Default.Save();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


        public void Dispose()
        {
            _settingsPanelHolder.Children.Clear();
        }
    }
}
