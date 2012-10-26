using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Model;
using SQLite;

namespace MovieManager.APP.Commands
{
    class UpdateVideosCommand : ICommand
    {
        private List<Video> _video;

        public UpdateVideosCommand(List<Video> video)
        {
            _video = video;
        }

        public UpdateVideosCommand(Video video)
        {
            _video = new List<Video>() { video };
        }

        public void Execute(object parameter)
        {
            TmcDatabase.UpdateVideos(_video);
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void OnCanExecChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, new EventArgs());
        }
    }
}
