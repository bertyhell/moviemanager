using System;
using System.ComponentModel;
using System.Windows.Input;
using System.IO;
using System.Configuration;
using Ookii.Dialogs.Wpf;
using Tmc.DataAccess.SqlCe;
using Tmc.SystemFrameworks.Common;
using Tmc.WinUI.Application.Common;
using Tmc.WinUI.Application.Localization;

namespace Tmc.WinUI.Application.Commands
{
    class AddVideosDirectoryCommand : ICommand, INotifyPropertyChanged
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
            String Path = ConfigurationManager.AppSettings["defaultVideoLocation"];
            if (!new DirectoryInfo(Path).Exists)
            {
                Path = ConfigurationManager.AppSettings["defaultVideoLocation1"];
            }
            if (!new DirectoryInfo(Path).Exists)
            {
                Path = ConfigurationManager.AppSettings["defaultVideoLocation2"];
            }




            var Dialog = new VistaFolderBrowserDialog { Description = Resource.PleaseSelectAFolder, UseDescriptionForTitle = true, SelectedPath = Path };
            // ReSharper disable PossibleInvalidOperationException
            if ((bool)Dialog.ShowDialog(MainWindow.Instance))
            // ReSharper restore PossibleInvalidOperationException
            {
                Message = "Searching videos: 0 found";
                IsIndeterminate = true;
                _progressWindow = new ProgressbarWindow(this) { Owner = MainWindow.Instance, DataContext = this };
                var FileReader = new MovieFileReader(new DirectoryInfo(Dialog.SelectedPath), Properties.Settings.Default.VideoInsertionSettings);
                FileReader.FoundVideo += FileReader_OnVideoFoundProgress;
                FileReader.OnGetVideoCompleted += FileReader_OnGetVideoCompleted;
                FileReader.RunWorkerAsync();
                _progressWindow.ShowDialog();
            }
        }

        public void FileReader_OnVideoFoundProgress(object sender, ProgressEventArgs eventArgs)
        {
            Message = "Searching videos: " + eventArgs.ProgressNumber + " found";
        }

        void FileReader_OnGetVideoCompleted(object sender, GetVideoCompletedEventArgs e)
        {
            _progressWindow.Close();
            Message = "Adding videos to database: 0.0 %";
            Value = 0;
            Maximum = e.Videos.Count;
            IsIndeterminate = false;
            _progressWindow = new ProgressbarWindow(this) { Owner = MainWindow.Instance,DataContext = this };
            var BgwInsertVideos = new BgwInsertVideos(e.Videos);
            DataRetriever.InsertVideosProgress += FileReader_OnInsertVideosProgress;
            BgwInsertVideos.RunWorkerCompleted += BGWInsertVideos_OnInsertVideosCompleted;
            BgwInsertVideos.RunWorkerAsync();
            _progressWindow.ShowDialog();
        }

        private void FileReader_OnInsertVideosProgress(object sender, ProgressEventArgs e)
        {
            Value = e.ProgressNumber;
            Message = "Adding videos to database: " + Math.Round(e.ProgressNumber * 100.0 / e.MaxNumber, 1).ToString("N1") + " %";
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
