using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
using Tmc.DataAccess.SqlCe;
using Tmc.SystemFrameworks.Common;
using Tmc.WinUI.Application.Common;

namespace Tmc.WinUI.Application.Commands
{
    class AddVideosCommand : ICommand, INotifyPropertyChanged
    {
        private ProgressbarWindow _progressWindow;

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
            String Path = ConfigurationManager.AppSettings["defaultVideoFilesLocation"];

            //FolderBrowserDialog Odd = new FolderBrowserDialog { SelectedPath = Path };
            OpenFileDialog Ofd = new OpenFileDialog
                                     {
                                         InitialDirectory = Path,
                                         Multiselect = true
                                     };
            if (Ofd.ShowDialog() == DialogResult.OK)
            {
                Message = "Searching videos: 0 found";
                Value = 0;
                Maximum = Ofd.FileNames.Count();
                IsIndeterminate = false;
                _progressWindow = new ProgressbarWindow(this) { Owner = MainWindow.Instance, DataContext = this };

                MovieFileReader FileReader = new MovieFileReader(Ofd.FileNames.Select(file => new FileInfo(file)).ToList(), Properties.Settings.Default.VideoInsertionSettings);
                FileReader.FoundVideo += FileReader_OnVideoFilesProcessingProgress;
                FileReader.OnGetVideoCompleted += FileReader_OnGetVideoCompleted;
                FileReader.RunWorkerAsync();
                _progressWindow.Show();
            }
        }

        public void FileReader_OnVideoFilesProcessingProgress(object sender, ProgressEventArgs e)
        {
            Value = e.ProgressNumber;
            Message = "Searching videos: " + e.ProgressNumber + " files processed";
        }

        void FileReader_OnGetVideoCompleted(object sender, GetVideoCompletedEventArgs e)
        {
            _progressWindow.Close();
            Message = "Adding videos to database...";
            Value = 0;
            Maximum = e.Videos.Count;
            IsIndeterminate = true;
            _progressWindow = new ProgressbarWindow(this) { Owner = MainWindow.Instance, DataContext = this };
            BgwInsertOrUpdateVideos BgwInsertVideos = new BgwInsertOrUpdateVideos(e.Videos);
            DataRetriever.UpdateVideosProgress += FileReaderOnUpdateVideosProgress;
            BgwInsertVideos.RunWorkerCompleted += BGWInsertVideos_OnInsertVideosCompleted;
            BgwInsertVideos.RunWorkerAsync();
            _progressWindow.ShowDialog();
        }

        private void FileReaderOnUpdateVideosProgress(object sender, ProgressEventArgs e)
        {
            Value = e.ProgressNumber;
            Message = "Adding videos to database...";
        }

        void BGWInsertVideos_OnInsertVideosCompleted(object sender, EventArgs e)
        {
            _progressWindow.Close();
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                PropChanged("Message");
            }
        }

        private int _value;
        public int Value
        {
            get { return _value; }
            set
            {
                _value = value;
                PropChanged("Value");
            }
        }

        private int _maximum;
        public int Maximum
        {
            get { return _maximum; }
            set
            {
                _maximum = value;
                PropChanged("Maximum");
            }
        }

        private bool _isIndeterminate;
        public bool IsIndeterminate
        {
            get { return _isIndeterminate; }
            set
            {
                _isIndeterminate = value;
                PropChanged("IsIndetermined");
            }
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
