namespace Tmc.WinUI.Application.Common
{
    /// <summary>
    /// Interaction logic for ProgressbarWindow.xaml
    /// </summary>
    public partial class ProgressbarWindow
    {
        public ProgressbarWindow(object datacontext)
        {
            InitializeComponent();
            progressBarControl1.DataContext = datacontext;
        }

        //public bool IsIndeterminate
        //{
        //    get { return progressBarControl1.IsIndeterminate; }
        //    set
        //    {
        //        progressBarControl1.IsIndeterminate = value;
        //    }
        //}
    }
}
