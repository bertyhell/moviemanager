using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
using Common;
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

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            String Path = ConfigurationManager.AppSettings["defaultVideoLocation"];
            if (!new DirectoryInfo(Path).Exists)
            {
                Path = ConfigurationManager.AppSettings["defaultVideoLocation1"];
            }
            FolderBrowserDialog Odd = new FolderBrowserDialog { SelectedPath = Path };
            if (Odd.ShowDialog() == DialogResult.OK)
            {
                _progressWindow = new ProgressbarWindow { Owner = MainWindow.Instance, IsIndeterminate = true, Message = "Searching videos: 0 found"};
                MovieFileReader FileReader = new MovieFileReader(new DirectoryInfo(Odd.SelectedPath));
                FileReader.FoundVideo += FileReader_OnVideoFoundProgress;
                FileReader.OnGetVideoCompleted += FileReader_OnGetVideoCompleted;
                FileReader.RunWorkerAsync();
                _progressWindow.Show();
            }
        }
        
        public void FileReader_OnVideoFoundProgress(object sender, ProgressArgs args)
        {
            _progressWindow.Message = "Searching videos: " + args.ProgressNumber + " found";
        }

        void FileReader_OnGetVideoCompleted(object sender, GetVideoCompletedEventArgs e)
        {
            _progressWindow.Close();
            _progressWindow = new ProgressbarWindow { Owner = MainWindow.Instance, IsIndeterminate = false, Message = "Adding to database: 0.0 %", Value = 0, Maximum = e.Videos.Count };
            BGWInsertVideos BGWInsertVideos = new BGWInsertVideos(e.Videos);
            MMDatabase.InsertVideos += MMDatabase_OnInsertVideosProgress;
            BGWInsertVideos.OnInsertVideosCompleted += BGWInsertVideos_OnInsertVideosCompleted;
            BGWInsertVideos.RunWorkerAsync();
            _progressWindow.Show();
        }

        public void MMDatabase_OnInsertVideosProgress(object sender, ProgressArgs args)
        {
            double Perectage = (double)args.ProgressNumber / args.MaxNumber;
            _progressWindow.Message = "Adding to database: " + Perectage + " %";
            _progressWindow.Value = args.ProgressNumber;
        }

        void BGWInsertVideos_OnInsertVideosCompleted(object sender, EventArgs e)
        {
            _progressWindow.Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
