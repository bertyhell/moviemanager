using System;
using System.Collections.Generic;
using System.Windows.Input;
using Tmc.WinUI.Application.Panels.Settings;
using Tmc.WinUI.Application.Panels.Settings.MediaPlayer;

namespace Tmc.WinUI.Application.Commands
{
    class EditSettingsCommand : ICommand
    {
        private SettingsWindow _settingsWindow;
        private List<SettingsPanelBase> _panels;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void OnExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, new EventArgs());
        }

        public void Execute(object parameter)
        {

            _panels = new List<SettingsPanelBase>
                    {
                        new DatabaseSettingsPanel(),
                        new FileRenameSettingsPanel(),
                        new LoggingSettingsPanel(),
                        new MediaPlayerSettingsPanel(),
                        new VideoSearchPanel()
                    };

            _settingsWindow = new SettingsWindow(_panels) { Owner = MainWindow.Instance };
            if (parameter != null)
            {
                Type Type = (Type)parameter;
                ShowSelectedPanel(Type, _panels);
            }
            _settingsWindow.ShowDialog();
            _settingsWindow.Closed += _settingsWindow_Closed;
        }

        private bool ShowSelectedPanel(Type type, IEnumerable<SettingsPanelBase> panels)
        {
            foreach (SettingsPanelBase SettingsPanelBase in panels)
            {
                //check if panel is selected
                if (SettingsPanelBase.GetType() == type)
                {
                    SettingsPanelBase.IsSelected = true;
                    SettingsPanelBase.IsExpanded = true;
                    return true;
                }

                //check if one of his children is selected
                if (ShowSelectedPanel(type, SettingsPanelBase.ChildPanels))
                {
                    return true;
                }
            }
            //no panel selected
            return false;
        }

        void _settingsWindow_Closed(object sender, EventArgs e)
        {
            _settingsWindow = null;
            _panels = null;
        }
    }
}
