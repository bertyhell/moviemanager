using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Forms;
using System.IO;
using Model;
using System.Configuration;
using SQLite;

namespace MovieManager.APP.Commands
{
    class AddVideosCommand : ICommand
    {
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
                ObservableCollection<Video> videos = new ObservableCollection<Video>();
                foreach (String file in ofd.FileNames)
                {
                    MovieFileReader.GetVideos(new FileInfo(file), videos);
                }
                MMDatabase.InsertVideosHDD(videos);
            }
        }
    }
}
