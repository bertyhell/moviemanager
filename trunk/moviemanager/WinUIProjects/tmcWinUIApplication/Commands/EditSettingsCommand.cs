using System;
using System.Collections.Generic;
using System.Windows.Input;
using MovieManager.APP.Panels.Settings;
using Tmc.WinUI.Application.Panels.Settings;

namespace Tmc.WinUI.Application.Commands
{
    class EditSettingsCommand : ICommand
    {
        private SettingsWindow _settingsWindow;
        private readonly List<SettingsPanelBase> _panels;
        public EditSettingsCommand()
        {
            _panels = new List<SettingsPanelBase>{
                new DatabaseSettingsPanel(),
                new FileRenameSettingsPanel(),
                new LoggingSettingsPanel(),
                new MediaPlayerSettingsPanel(),
                new VideoSearchPanel()
            };

        }

        public EditSettingsCommand(Type visiblePanel)
            : this()
        {
            Object Object =Activator.CreateInstance(visiblePanel);
            if(!(Object is SettingsPanelBase))
                throw new TypeInitializationException(this.GetType().FullName, new Exception("The given type " + visiblePanel.FullName + " does not implement interface 'SettingsPanelBase'"));

            SettingsPanelBase Panel = (SettingsPanelBase) Object;
            Panel.IsExpanded = true;
            Panel.IsSelected = true;
            _panels = new List<SettingsPanelBase> { Panel };
        }

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
            _settingsWindow = new SettingsWindow(_panels) { Owner = MainWindow.Instance };
            _settingsWindow.ShowDialog();
        }
    }
}
