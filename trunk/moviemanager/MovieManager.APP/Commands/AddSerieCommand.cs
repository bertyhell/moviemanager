using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
using Model;
using SQLite;

namespace MovieManager.APP.Commands
{
    class AddSerieCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            String Path = ConfigurationManager.AppSettings["defaultVideoLocation"];
            if(!new DirectoryInfo(Path).Exists)
            {
                Path = ConfigurationManager.AppSettings["defaultVideoLocation1"];
            }
            FolderBrowserDialog Odd = new FolderBrowserDialog { SelectedPath = Path };
            if (Odd.ShowDialog() == DialogResult.OK)
            {
                ObservableCollection<Video> LocalVideos = new ObservableCollection<Video>();
                MovieFileReader MovieFileReader = new MovieFileReader(new DirectoryInfo(Odd.SelectedPath));
                MovieFileReader.GetSerie(new DirectoryInfo(Odd.SelectedPath), "", "", LocalVideos);
                MMDatabase.InsertVideosHDD(LocalVideos);
            }
        }
    }
}
