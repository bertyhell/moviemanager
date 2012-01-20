using System;
using System.Collections.Generic;
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
            OpenFileDialog Ofd = new OpenFileDialog
                                     {
                                         InitialDirectory = ConfigurationManager.AppSettings["defaultVideoLocation"],
                                         Multiselect = true
                                     };
            if (Ofd.ShowDialog() == DialogResult.OK)
            {
                ObservableCollection<Video> Videos = new ObservableCollection<Video>();
                foreach (String File in Ofd.FileNames)
                {
                    MovieFileReader.GetVideos(new FileInfo(File), Videos);
                }
                MMDatabase.InsertVideosHDD(Videos);
            }
        }
    }
}
