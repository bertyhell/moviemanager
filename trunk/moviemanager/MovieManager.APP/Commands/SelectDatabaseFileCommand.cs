using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;
using SQLite;
using System.ComponentModel;

namespace MovieManager.APP.Commands
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
            OpenFileDialog Ofd = new OpenFileDialog() { CheckFileExists = false, Filter = "SQLite files (*.sqlite)|*.sqlite" };
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
