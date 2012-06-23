using System.Windows;

namespace ExcelInterop
{
    public partial class ExcelExportWindow
    {

        private readonly ExcelExportController _controller;
        public ExcelExportWindow()
        {

            InitializeComponent();

            // Insert code required on object creation below this point.
            _controller = new ExcelExportController();
            DataContext = _controller;
        }
        
        private void BtnSelectAllNoneClick(object sender, RoutedEventArgs e)
        {
            _controller.SelectAllNone();
        }

        private void BtnAbortClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnExportClick(object sender, RoutedEventArgs e)
        {
            //export to excel file
            _controller.Export();
            Close();

        }

        private void BtnBrowseClick(object sender, RoutedEventArgs e)
        {
            _controller.Browse();
        }
    }
}
