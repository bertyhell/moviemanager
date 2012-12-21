using System;
using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;
using APP;
using MovieManager.APP.Properties;
using Tmc.SystemFrameworks.Log;
using log4net.Core;

namespace MovieManager.APP
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
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            GlobalLogger.Instance.MovieManagerLogger.Info("Program exited normally");
            base.OnExit(e);
        }
    }
}
