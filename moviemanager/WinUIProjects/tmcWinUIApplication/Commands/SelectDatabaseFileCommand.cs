using System;
using System.Windows.Forms;
using System.Windows.Input;
using System.ComponentModel;
using Tmc.WinUI.Application.Localization;

namespace Tmc.WinUI.Application.Commands
{
    class SelectDatabaseFileCommand : ICommand, INotifyPropertyChanged
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
            OpenFileDialog Ofd = new OpenFileDialog { CheckFileExists = false, Filter = Resource.SqlServerCeFileFilter };
            if (Ofd.ShowDialog() == DialogResult.OK)
            {
                _pathToFile = Ofd.FileName;
                OnPropertyChanged("PathToFile");
            }
        }

        private string _pathToFile = "";
        public string PathToFile
        {
            get { return _pathToFile; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string fieldName)
        {
            if( PropertyChanged !=null)
                PropertyChanged(this,new PropertyChangedEventArgs(fieldName));
        }
    }
}
