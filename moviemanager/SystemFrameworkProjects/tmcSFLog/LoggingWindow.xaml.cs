using System.Windows;

namespace Tmc.SystemFrameworks.Log
{
    /// <summary>
    /// Interaction logic for LoggingWindow.xaml
    /// </summary>
    public partial class LoggingWindow : Window
    {
        private readonly LoggingController _controller = new LoggingController();
        public LoggingWindow()
        {
            InitializeComponent();
            DataContext = _controller;
        }
    }
}
