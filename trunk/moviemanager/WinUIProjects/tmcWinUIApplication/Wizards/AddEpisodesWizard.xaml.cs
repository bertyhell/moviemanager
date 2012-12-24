using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using Model;
using Tmc.DataAccess.Sqlite;

namespace Tmc.WinUI.Application.Wizards
{
    /// <summary>
    /// Interaction logic for AddEpisodesWizard.xaml
    /// </summary>
    public partial class AddEpisodesWizard : Window
    {
        public AddEpisodesWizard()
        {
            InitializeComponent();
        }

        private void AddSeries()
        {


            String Path = ConfigurationManager.AppSettings["defaultVideoLocation"];
            if (!new DirectoryInfo(Path).Exists)
            {
                Path = ConfigurationManager.AppSettings["defaultVideoLocation1"];
            }
            FolderBrowserDialog Odd = new FolderBrowserDialog { SelectedPath = Path };
            if (Odd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ObservableCollection<Video> LocalVideos = new ObservableCollection<Video>();
                MovieFileReader MovieFileReader = new MovieFileReader(new DirectoryInfo(Odd.SelectedPath), Properties.Settings.Default.VideoInsertionSettings);
                MovieFileReader.GetSerie(new DirectoryInfo(Odd.SelectedPath), "", "", LocalVideos);
                TmcDatabase.InsertVideosHdd(LocalVideos);
            }
        }
    }
}
