using System;
using System.Configuration;
using System.Data.EntityClient;
using System.Globalization;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;
using Tmc.BusinessRules.Web.Search;
using Tmc.DataAccess.SqlCe;
using Tmc.SystemFrameworks.Common;
using Tmc.SystemFrameworks.Log;
using Tmc.WinUI.Application.Common;
using Tmc.WinUI.Application.Properties;

namespace Tmc.WinUI.Application
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private void DispatcherUnhandledExceptionEventHandler(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            UnUnhandleExceptionHandler(e.Exception);
            // Prevent default unhandled exception processing
            e.Handled = true;
        }

        private void DispatcherUnhandledExceptionEventHandler(object sender, UnhandledExceptionEventArgs e)
        {
            UnUnhandleExceptionHandler((Exception) e.ExceptionObject);
        }

        private void UnUnhandleExceptionHandler(Exception ex)
        {
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                // Process unhandled exception
                new ExceptionMessageBox(ex, "Unhandled exeption has occured, please send this info to info.taxrebel@gmail.com").Show();

                GlobalLogger.Instance.MovieManagerLogger.Error(ex.Message);
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(DispatcherUnhandledExceptionEventHandler);
            //TODO 050: implement option to disable logging (already stored in settings) 
            GlobalLogger.Instance.LogLevel = Settings.Default.Log_Level;
            GlobalLogger.Instance.MovieManagerLogger.Info("Program started");
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            InitDatabase();

            base.OnStartup(e);
        }

        protected void InitDatabase()
        {
            //Check if database exists
            string DatabasePath = Settings.Default.DatabasePath.Replace("%APPDATA%", DefaultValues.PATH_USER_APPDATA);

            string DatabaseFolderPath = Path.GetDirectoryName(DatabasePath);
            if (!Directory.Exists(DatabaseFolderPath))
            {
                //create folder if not already there
                Directory.CreateDirectory(DatabaseFolderPath);
            }

            string ConnectionString = string.Format("Data Source = {0}", DatabasePath);
            EntityConnectionStringBuilder ConnectionStringBuilder = new EntityConnectionStringBuilder
            {
                Provider = "System.Data.SqlServerCe.4.0",
                ProviderConnectionString = ConnectionString
            }; //TODO 040 unused variable? --> remove or fix me
            //ConnectionStringBuilder.Metadata = "res://*/VideoModel.csdl|res://*/VideoModel.ssdl|res://*/VideoModel.msl";
            DataRetriever.Init(ConnectionString);

			try
			{
				if (DataRetriever.Genres.Count == 0) DataRetriever.Genres = SearchTmdb.GetGenres();
			}catch(TypeInitializationException Ex)
			{
				if(Ex.InnerException.GetType() == typeof(WebException))
				{
					MessageBox.Show("A problem occured while attempting to retrieve the movie genres.", "Error", MessageBoxButton.OK,
					                MessageBoxImage.Exclamation);
				}
			}
        }

        protected override void OnExit(ExitEventArgs e)
        {
            GlobalLogger.Instance.MovieManagerLogger.Info("Program exited normally");
            base.OnExit(e);
        }
    }
}
