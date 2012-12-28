using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;
using Tmc.DataAccess.Sqlite;
using Tmc.SystemFrameworks.Common;
using Tmc.SystemFrameworks.Log;
using Tmc.WinUI.Application.Commands;
using Tmc.WinUI.Application.Panels.Settings;
using Tmc.WinUI.Application.Properties;

namespace Tmc.WinUI.Application
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        void DispatcherUnhandledExceptionEventHandler(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
#if RELEASE
            // Process unhandled exception
            new ExceptionMessageBox(e.Exception, "Unhandled exeption has occured, please send this info to info.taxrebel@gmail.com").Show();


            GlobalLogger.Instance.MovieManagerLogger.Error(e.Exception.Message);

            // Prevent default unhandled exception processing
            e.Handled = true;
#endif
        }

        protected override void OnStartup(System.Windows.StartupEventArgs e)
        {
            //TODO 050: implement option to disable logging (already stored in settings) 
            GlobalLogger.Instance.LogLevel = Settings.Default.Log_Level;
            GlobalLogger.Instance.MovieManagerLogger.Info("Program started");
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            //Check if database exists
            string DatabasePath = Settings.Default.DatabasePath.Replace("%APPDATA%", DefaultValues.PATH_USER_APPDATA);
            TmcDatabase.Init(DatabasePath);
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            GlobalLogger.Instance.MovieManagerLogger.Info("Program exited normally");
            base.OnExit(e);
        }
    }
}
