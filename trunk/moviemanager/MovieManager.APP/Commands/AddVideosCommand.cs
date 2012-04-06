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
            //FolderBrowserDialog Odd = new FolderBrowserDialog { SelectedPath = Path };
            OpenFileDialog Ofd = new OpenFileDialog
                                     {
                                         InitialDirectory = Path,
                                         Multiselect = true
                                     };
            if (Ofd.ShowDialog() == DialogResult.OK)
            {
                new ObservableCollection<Video>();
                foreach (String File in Ofd.FileNames)
                {
                    _progressWindow = new ProgressbarWindow(this) {Owner = MainWindow.Instance};
                    _progressWindow.ShowDialog(); 

                    MovieFileReader FileReader = new MovieFileReader(new FileInfo(File));
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
