using System.IO;
using System.Windows;
using System.ComponentModel;
using Tmc.DataAccess.SqlCe;
using Tmc.SystemFrameworks.Model;
using Tmc.WinUI.Application.Commands;

namespace Tmc.WinUI.Application.Panels.Settings
{
    /// <summary>
    /// Interaction logic for DatabaseSettingsPanel.xaml
    /// </summary>
    public partial class DatabaseSettingsPanel : INotifyPropertyChanged
    {
        private readonly SelectDatabaseFileCommand _fileCommand = new SelectDatabaseFileCommand();
        private DatabaseDetails _databaseDetails;
        private string _pathToDatabase;

        public DatabaseSettingsPanel()
        {
            InitializeComponent();

            //initialize base class variables;
            _panelName = "Database";
            _iconPath = "/Tmc.WinUI.Application;component/Images/database_32.png";

            DataContext = this;
            _pathToDatabase = Properties.Settings.Default.DatabasePath;

            InitDatabaseVersionControl();

        }

        public string PathToDatabase
        {
            get { return _pathToDatabase; }
            set
            {
                _pathToDatabase = value;
                OnPropChanged("PathToDatabase");
            }
        }

        public DatabaseDetails DatabaseDetails
        {
            get { return _databaseDetails; }
            set
            {
                _databaseDetails = value;
                OnPropChanged("DatabaseDetails");
            }
        }

        private void BtnCreateDatabaseClick(object sender, RoutedEventArgs e)
        {
            ConvertDatabaseCommand ConvertCommand = new ConvertDatabaseCommand();
            ConvertCommand.Execute(null);
            InitDatabaseVersionControl();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            _fileCommand.Execute(null);
            PathToDatabase = _fileCommand.PathToFile;
            InitDatabaseVersionControl();
        }

        private void InitDatabaseVersionControl()
        {
            if (File.Exists(_pathToDatabase))
            {
                DatabaseDetails = DataRetriever.GetDatabaseDetails();
            }
            else
            {
                DatabaseDetails = new DatabaseDetails { DatabaseVersion = 0, RequiredVersion = DataRetriever.CURRENT_DATABASE_VERSION };
            }
        }

        public override bool SaveSettings()
        {
            Properties.Settings.Default.DatabasePath = _pathToDatabase;
            return base.SaveSettings();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropChanged(string field)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(field));
        }
    }
}
