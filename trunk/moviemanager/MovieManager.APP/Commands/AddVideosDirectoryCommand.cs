using System;
using System.Windows.Input;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
using MovieManager.APP.Common;
using SQLite;

namespace MovieManager.APP.Commands
{
    class AddVideosDirectoryCommand : ICommand
    {
        private ProgressbarWindow _progressWindow;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            FolderBrowserDialog odd = new FolderBrowserDialog { SelectedPath = ConfigurationManager.AppSettings["defaultVideoLocation"] };
            if (odd.ShowDialog() == DialogResult.OK)
            {
                _progressWindow = new ProgressbarWindow {Title = "Getting Videos", Owner = MainWindow.Instance};

                MovieFileReader FileReader = new MovieFileReader(new DirectoryInfo(odd.SelectedPath));
                FileReader.OnGetVideoCompleted += FileReader_OnGetVideoCompleted;
                FileReader.RunWorkerAsync();
                _progressWindow.Show();
            }
        }

        void FileReader_OnGetVideoCompleted(object sender, GetVideoCompletedEventArgs e)
        {
            MMDatabase.InsertVideosHDD(e.Videos);
            _progressWindow.Close();
        }
    }
}
