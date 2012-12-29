using System;
using System.Windows.Input;
using Tmc.SystemFrameworks.Model;
using Tmc.WinUI.Player.Logic;

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
