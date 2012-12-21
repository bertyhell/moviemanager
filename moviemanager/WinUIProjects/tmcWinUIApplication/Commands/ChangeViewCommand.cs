using System;
using System.ComponentModel;
using System.Windows.Input;

namespace Tmc.WinUI.Application.Commands
{
    public enum ViewStates
    {
        BigIcons, MediumIcons, SmallIcons, Details
    }

    public class ChangeViewCommand : ICommand, INotifyPropertyChanged
    {

        private readonly ViewStates _requestedViewState;


        public ChangeViewCommand(ViewStates state)
        {
            _requestedViewState = state;
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
            MainController.Instance.ChangeView(_requestedViewState);//TODO 030 add the state of the view to the settings + resore on startup
        }

        public void PropChanged(string field)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(field));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
