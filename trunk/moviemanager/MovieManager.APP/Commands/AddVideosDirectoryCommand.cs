using System;
using System.Windows.Input;
using System.Windows.Forms;
using System.IO;
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
            FolderBrowserDialog odd = new FolderBrowserDialog { SelectedPath = ConfigurationManager.AppSettings["defaultVideoLocation"] };
            if (odd.ShowDialog() == DialogResult.OK)
            {
                MovieFileReader.GetVideos(new DirectoryInfo(odd.SelectedPath), MainController.Instance.Videos);

                MMDatabase.InsertVideosHDD(MainController.Instance.Videos);
            }
        }
    }
}
