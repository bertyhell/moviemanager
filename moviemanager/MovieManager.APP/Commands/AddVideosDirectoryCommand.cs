using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
using MovieManager.APP.Common;
using SQLite;

namespace MovieManager.APP.Commands
{
    class AddVideosDirectoryCommand : ICommand, INotifyPropertyChanged
    {
        private ProgressbarWindow _progressWindow;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        private int _progress = 0;

        public String ProgressString
        {
            get { return "Adding videos to database: " + _progress + "%"; }
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            FolderBrowserDialog Odd = new FolderBrowserDialog { SelectedPath = ConfigurationManager.AppSettings["defaultVideoLocation"] };
            if (Odd.ShowDialog() == DialogResult.OK)
            {
                _progressWindow = new ProgressbarWindow { Owner = MainWindow.Instance, IsIndeterminate = true };
                MovieFileReader FileReader = new MovieFileReader(new DirectoryInfo(Odd.SelectedPath), _progressWindow.ProgressString);
                FileReader.OnGetVideoCompleted += FileReader_OnGetVideoCompleted;
                FileReader.RunWorkerAsync();
                _progressWindow.Show();
            }
        }

        void FileReader_OnGetVideoCompleted(object sender, GetVideoCompletedEventArgs e)
        {
            _progressWindow.Close();
            _progressWindow = new ProgressbarWindow { Owner = MainWindow.Instance, DataContext = this, IsIndeterminate = true };
            _progressWindow.Show();
            MMDatabase.InsertVideosHDD(e.Videos);
            _progressWindow.Close();

        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
