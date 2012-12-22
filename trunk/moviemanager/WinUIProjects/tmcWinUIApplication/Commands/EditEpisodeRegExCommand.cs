using System;
using System.Windows.Input;
using Tmc.WinUI.Application.Panels.RegularExpressions;

namespace Tmc.WinUI.Application.Commands
{
    class EditEpisodeRegExCommand : ICommand
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
            EpisodeRegexEditor RegexEditor = new EpisodeRegexEditor();
            RegexEditor.Show();
        }
    }
}
