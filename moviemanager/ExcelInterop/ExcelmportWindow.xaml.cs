namespace ExcelInterop
{
    using System.Windows;

    public partial class ExcelImportWindow
    {


        private readonly ExcelImportController _controller;
        public ExcelImportWindow()
        {
            InitializeComponent();

            // Insert code required on object creation below this point.
            _controller = new ExcelImportController();
            DataContext = _controller;
        }

        private void BtnImportClick(object sender, RoutedEventArgs e)
        {

            _controller.Import();
            Close();
        }

        private void BtnAbortClick(object sender, RoutedEventArgs e)
        {

            Close();
        }

        private void BtnBrowseClick(object sender, RoutedEventArgs e)
        {
            _controller.GetImportFile();

        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            _controller.GetImportFile();
        }
    }
}
