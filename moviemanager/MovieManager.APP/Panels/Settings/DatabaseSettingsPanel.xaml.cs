using System.IO;
using System.Windows;
using SQLite;
using MovieManager.APP.Commands;
using Model;
using System.ComponentModel;

namespace MovieManager.APP.Panels.Settings
{
    /// <summary>
    /// Interaction logic for DatabaseSettingsPanel.xaml
    /// </summary>
    public partial class DatabaseSettingsPanel : INotifyPropertyChanged
    {
        private SelectDatabaseFileCommand _fileCommand = new SelectDatabaseFileCommand();
        private DatabaseDetails _databaseDetails;
        private string _pathToDatabase;

        public DatabaseSettingsPanel()
        {
            InitializeComponent();

            //initialize base class variables;
            _panelName = "Database";
            _iconPath = "/MovieManager;component/Images/database_32.png";

            this.DataContext = this;
            _pathToDatabase = Properties.Settings.Default.DatabasePath;

            if (File.Exists(_pathToDatabase))
            {
                MMDatabaseCreation.Init(Properties.Settings.Default.ConnectionString.Replace("{path}", _pathToDatabase));
                DatabaseDetails = MMDatabaseCreation.GetDatabaseDetails();
            }
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

        private void _btnCreateDatabase_Click(object sender, RoutedEventArgs e)
        {
            ConvertDatabaseCommand ConvertCommand = new ConvertDatabaseCommand(_txtFilePath.Text);
            ConvertCommand.Execute(null);
            if (File.Exists(_pathToDatabase))
            {
                MMDatabaseCreation.Init(Properties.Settings.Default.ConnectionString.Replace("{path}", _pathToDatabase));
                DatabaseDetails = MMDatabaseCreation.GetDatabaseDetails();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _fileCommand.Execute(null);
            PathToDatabase = _fileCommand.PathToFile;
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
