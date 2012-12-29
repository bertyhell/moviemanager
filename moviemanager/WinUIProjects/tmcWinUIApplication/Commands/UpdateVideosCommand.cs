using System;
using System.Collections.Generic;
using System.Windows.Input;
using Model;
using Tmc.DataAccess.SqlCe;

namespace Tmc.WinUI.Application.Commands
{
    class UpdateVideosCommand : ICommand
    {
        private readonly List<Video> _video;

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
            DataRetriever.UpdateVideos(_video);
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
