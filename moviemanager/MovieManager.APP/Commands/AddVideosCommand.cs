using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Forms;
using System.IO;
using Model;
using System.Configuration;
using MovieManager.APP.Common;
using SQLite;

namespace MovieManager.APP.Commands
{
    class AddVideosCommand : ICommand
    {
        private ObservableCollection<Video> _videos;
        private ProgressbarWindow _progressWindow;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            OpenFileDialog ofd = new OpenFileDialog
                                     {
                                         InitialDirectory = ConfigurationManager.AppSettings["defaultVideoLocation"],
                                         Multiselect = true
                                     };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                _videos = new ObservableCollection<Video>();
                foreach (String file in ofd.FileNames)
                {
                    _progressWindow = new ProgressbarWindow();
                    _progressWindow.Title = "Getting Videos";
                    _progressWindow.Owner = MainWindow.Instance;
                    _progressWindow.ShowDialog(); 

                    MovieFileReader FileReader = new MovieFileReader(new FileInfo(file));
                    FileReader.OnGetVideoCompleted += FileReader_OnGetVideoCompleted;
                    FileReader.RunWorkerAsync();
                }
            }
        }

        void FileReader_OnGetVideoCompleted(object sender, GetVideoCompletedEventArgs e)
        {
            MMDatabase.InsertVideosHDD(e.Videos);
        }

    }
}
