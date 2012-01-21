using System.Windows;

namespace ExcelInterop
{
    public partial class ExcelExportWindow
    {

        private readonly ExcelExportController _controller;
        public ExcelExportWindow(bool isFuifProductExport, int fuifId = -1)
            : base()
        {

            InitializeComponent();

            // Insert code required on object creation below this point.
            _controller = new ExcelExportController(isFuifProductExport, fuifId);
            DataContext = _controller;
        }



        private void BtnAbortClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnSelectAllNoneClick(object sender, RoutedEventArgs e)
        {
            _controller.SelectAllNone();
        }

        private void BtnExportClick(object sender, RoutedEventArgs e)
        {
            //export to excel file
            _controller.Export();
            Close();
        }
    }
}
