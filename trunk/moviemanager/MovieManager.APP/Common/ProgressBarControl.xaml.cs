using System.ComponentModel;

namespace MovieManager.APP.Common
{
    /// <summary>
    /// Interaction logic for ProgressBarControl.xaml
    /// </summary>
    public partial class ProgressBarControl : INotifyPropertyChanged
    {
        public ProgressBarControl()
        {
            InitializeComponent();
        }

        private string _progressString;
        public string ProgressString
        {
            get { return _progressString; }
            set { _progressString = value; 
            PropertyChanged(this, new PropertyChangedEventArgs("ProgressString"));}
        }

        public bool IsIndeterminate { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
