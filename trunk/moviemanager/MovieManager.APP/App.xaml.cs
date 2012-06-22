using System;
using System.Windows.Threading;
using APP;

namespace MovieManager.APP
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        void DispatcherUnhandledExceptionEventHandler(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // Process unhandled exception
            new ExceptionMessageBox(e.Exception, "Unhandled exeption has occured, please send this info to info.taxrebel@gmail.com").Show();


            GlobalLogger.Instance.MovieManagerLogger.Error(e.Exception.Message);

            // Prevent default unhandled exception processing
            e.Handled = true;
        }
    }
}
