using System;
using System.ComponentModel;
using System.Windows.Input;
using System.IO;
using System.Configuration;
using Common;
using MovieManager.APP.Common;
using Ookii.Dialogs.Wpf;
using SQLite;

namespace MovieManager.APP.Commands
{
    class ChangeViewCommand : ICommand, INotifyPropertyChanged
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
            MainController.Instance.ToggleViews();//TODO 030 add the state of the view to the settings + resore on startup
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
