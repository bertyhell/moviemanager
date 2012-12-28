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
            ObservableCollection<Uri> Folders = _folderSelectionControl.Folders;
            if (Folders == null || Folders.Count == 0)
                throw new NullReferenceException("No folders specified");
            string SelectedPath = Folders[0].AbsolutePath;


            //add serie if necessary
            Serie Serie = _serieSelectionControl.Serie;
            if (Serie.Id == 0)
                TmcDatabase.AddSerie(Serie);

            //add videos
            ObservableCollection<Video> LocalVideos = new ObservableCollection<Video>();
            MovieFileReader MovieFileReader = new MovieFileReader(new DirectoryInfo(SelectedPath), Properties.Settings.Default.VideoInsertionSettings);
            MovieFileReader.GetEpisodesForSerie(new DirectoryInfo(SelectedPath), Serie, LocalVideos, "", "");
            TmcDatabase.InsertVideosHdd(LocalVideos);
        }

        private void Wizard_OnFinish(object sender, RoutedEventArgs e)
        {
            AddSeries();
        }
    }
}
