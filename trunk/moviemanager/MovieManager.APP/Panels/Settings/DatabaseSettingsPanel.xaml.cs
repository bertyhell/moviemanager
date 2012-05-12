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

namespace MovieManager.APP.Panels.Settings
{
    /// <summary>
    /// Interaction logic for DatabaseSettingsPanel.xaml
    /// </summary>
    public partial class DatabaseSettingsPanel
    {
        public DatabaseSettingsPanel()
        {
            InitializeComponent();


            //initialize base class variables;
            _panelName = "Database";
            _iconPath = "/MovieManager;component/Images/database_32.png";
        }

        private void _btnCreateDatabase_Click(object sender, RoutedEventArgs e)
        {
            CreateDatabaseCommand Command =new CreateDatabaseCommand();
            Command.Execute(null);
        }
    }
}
