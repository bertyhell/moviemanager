using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using MovieManager.APP.Panels.AddVideos;
using MovieManager.APP.Panels.RegularExpressions;

namespace MovieManager.APP.Commands
{
    class EditEpisodeRegExCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            EpisodeRegexEditor RegexEditor = new EpisodeRegexEditor();
            RegexEditor.Show();
        }
    }
}
