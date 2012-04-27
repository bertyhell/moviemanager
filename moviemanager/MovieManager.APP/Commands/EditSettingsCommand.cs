using System;
using System.Collections.Generic;
using System.Windows.Input;
using MovieManager.APP.Panels.Settings;

namespace MovieManager.APP.Commands
{
    class EditSettingsCommand : ICommand
    {
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
            List<SettingsPanelBase> Panels = new List<SettingsPanelBase>{
                new FileRenameSettingsPanel(),
                new MediaPlayerSettingsPanel(),
                new VideoSearchPanel()
            };

            SettingsWindow SettingsWindow = new SettingsWindow(Panels) { Owner = MainWindow.Instance };
            SettingsWindow.ShowDialog();
        }
    }
}
