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
            FolderBrowserDialog Odd = new FolderBrowserDialog { SelectedPath = ConfigurationManager.AppSettings["defaultVideoLocation"] };
            if (Odd.ShowDialog() == DialogResult.OK)
            {
                ObservableCollection<Video> LocalVideos = new ObservableCollection<Video>();
                MovieFileReader MovieFileReader = new MovieFileReader(new DirectoryInfo(Odd.SelectedPath), null);
                MovieFileReader.GetSerie(new DirectoryInfo(Odd.SelectedPath), "", "", LocalVideos);
                MMDatabase.InsertVideosHDD(LocalVideos);
            }
        }
    }
}
