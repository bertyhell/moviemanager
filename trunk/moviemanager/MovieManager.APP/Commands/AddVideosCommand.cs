﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Forms;
using System.IO;
using Common;
using Model;
using System.Configuration;
using MovieManager.APP.Common;
using SQLite;

namespace MovieManager.APP.Commands
{
    class AddVideosCommand : ICommand, INotifyPropertyChanged
    {
        private ProgressbarWindow _progressWindow;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

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
                new ObservableCollection<Video>();
                Message = "Searching videos: 0 found";
                Value = 0;
                Maximum = Ofd.FileNames.Count();

                _progressWindow = new ProgressbarWindow(this) { Owner = MainWindow.Instance, IsIndeterminate = false, DataContext = this };

                MovieFileReader FileReader = new MovieFileReader(Ofd.FileNames.Select(file => new FileInfo(file)).ToList());
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
            Message = "Adding videos to database: 0.0 %";
            Value = 0;
            Maximum = e.Videos.Count;
            _progressWindow = new ProgressbarWindow(this) { Owner = MainWindow.Instance, IsIndeterminate = false, DataContext = this };
            BGWInsertVideos BGWInsertVideos = new BGWInsertVideos(e.Videos);
            MMDatabase.InsertVideosProgress += FileReader_OnInsertVideosProgress;
            BGWInsertVideos.RunWorkerCompleted += BGWInsertVideos_OnInsertVideosCompleted;
            BGWInsertVideos.RunWorkerAsync();
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
            MainController.Instance.UpdateVideos();
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