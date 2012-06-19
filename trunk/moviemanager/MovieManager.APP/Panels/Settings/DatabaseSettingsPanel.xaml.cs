using System.Windows;
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

            //TODO 050: Initialize textbox with current database file
            _txtFilePath.DataContext = _fileCommand;
            _databaseDetails = MMDatabaseCreation.GetDatabaseDetails();
            _txtDatabaseVersion.DataContext = _databaseDetails;
            _txtDatabaseRequiredVersion.DataContext = _databaseDetails;
            _grdVersionDetails.DataContext = _databaseDetails;

        }

        private void _btnCreateDatabase_Click(object sender, RoutedEventArgs e)
        {
            ConvertDatabaseCommand ConvertCommand = new ConvertDatabaseCommand(_txtFilePath.Text);
            ConvertCommand.Execute(null);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _fileCommand.Execute(null);
        }
    }
}
