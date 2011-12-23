using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Forms;
using System.IO;
using Model;
using System.Configuration;


namespace SQLite.Commands
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
            FolderBrowserDialog odd = new FolderBrowserDialog();
            odd.SelectedPath = ConfigurationManager.AppSettings["defaultVideoLocation"];
            if (odd.ShowDialog() == DialogResult.OK)
            {
                List<Video> Videos = new List<Video>();
                MovieFileReader.GetVideos(new DirectoryInfo(odd.SelectedPath), Videos);

                MMDatabase.insertVideosHDD(Videos);
            }
        }
    }
} 
