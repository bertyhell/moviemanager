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
    class AddVideosCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = ConfigurationManager.AppSettings["defaultVideoLocation"];
            ofd.Multiselect = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                List<Video> Videos = new List<Video>();
                foreach (String file in ofd.FileNames)
                {
                    MovieFileReader.GetVideos(new FileInfo(file), Videos);
                }
                MMDatabase.insertVideosHDD(Videos);
            }
        }
    }
}
