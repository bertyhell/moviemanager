using Model;
using System;
using System.Windows.Input;
using MovieManager.PLAYER.Logic;

namespace Tmc.WinUI.Application.Commands
{
    class PlayVideoCommand :ICommand
    {
        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                PlayerProcesses.StartVideo((Video) parameter,"VLC");
            }
        }

        public bool CanExecute(object parameter)
        {
            return parameter is Video;
        }

        public event EventHandler CanExecuteChanged;
        public void OnExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, new EventArgs());
        }
    }
}
