namespace MovieManager.APP.Common
{
    /// <summary>
    /// Interaction logic for ProgressbarWindow.xaml
    /// </summary>
    public partial class ProgressbarWindow
    {
        public ProgressbarWindow()
        {
            InitializeComponent();
        }

        public string Message
        {
            get { return progressBarControl1.Message; }
            set
            {
                progressBarControl1.Message = value;
            }
        }

        public bool IsIndeterminate
        {
            get { return progressBarControl1.IsIndeterminate; }
            set
            {
                progressBarControl1.IsIndeterminate = value;
            }
        }

        public int Maximum
        {
            get { return progressBarControl1.Maximum; }
            set
            {
                progressBarControl1.Maximum = value;
            }
        }

        public int Value
        {
            get { return progressBarControl1.ProgressValue; }
            set
            {
                progressBarControl1.ProgressValue = value;
            }
        }
    }
}
