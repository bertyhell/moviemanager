using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Forms;
using System.IO;
using Model;
using System.Configuration;
using SQLite;

namespace MovieManager.APP.Commands
{
    class AddVideosDirectoryCommand : ICommand
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
                List<Video> Videos = new List<Video>();
                MovieFileReader.GetVideos(new DirectoryInfo(Odd.SelectedPath), Videos);

                MMDatabase.InsertVideosHDD(Videos);
            }
        }
    }
}
