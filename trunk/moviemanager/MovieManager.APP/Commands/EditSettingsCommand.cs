using System;
using System.Windows.Input;
using MovieManager.APP.Panels.RegularExpressions;
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

        public void Execute(object parameter)
        {
            SettingsWindow SettingsWindow = new SettingsWindow {Owner = MainWindow.Instance};
            SettingsWindow.ShowDialog();
        }
    }
}
