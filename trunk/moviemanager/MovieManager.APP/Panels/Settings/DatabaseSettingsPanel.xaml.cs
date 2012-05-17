using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SQLite;
using MovieManager.APP.Commands;
using Model;

namespace MovieManager.APP.Panels.Settings
{
    /// <summary>
    /// Interaction logic for DatabaseSettingsPanel.xaml
    /// </summary>
    public partial class DatabaseSettingsPanel
    {
        private SelectDatabaseFileCommand _fileCommand = new SelectDatabaseFileCommand();
        private DatabaseDetails _databaseDetails;
        

        public DatabaseSettingsPanel()
        {
            InitializeComponent();


            //initialize base class variables;
            _panelName = "Database";
            _iconPath = "/MovieManager;component/Images/database_32.png";

            _txtFilePath.DataContext = _fileCommand;
            _databaseDetails = MMDatabaseCreation.GetDatabaseDetails();
            _txtDatabaseVersion.DataContext = _databaseDetails;
            _txtDatabaseRequiredVersion.DataContext = _databaseDetails;
            _grdVersionDetails.DataContext = _databaseDetails;

        }

        private void _btnCreateDatabase_Click(object sender, RoutedEventArgs e)
        {
            ConvertDatabaseCommand ConvertCommand = new ConvertDatabaseCommand();
            ConvertCommand.Execute(null);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _fileCommand.Execute(null);
        }
    }
}
