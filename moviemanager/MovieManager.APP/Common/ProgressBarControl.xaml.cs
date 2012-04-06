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

        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                PropChanged("Message");
            }

        }

        private bool _isIndeterminate;
        public bool IsIndeterminate
        {
            get { return _isIndeterminate; }
            set
            {
                _isIndeterminate = value;
                PropChanged("IsIndeterminate");
            }
        }

        private int _maximum;
        public int Maximum
        {
            get { return _maximum; }
            set
            {
                _maximum = value;
                PropChanged("Maximum");
            }
        }

        private int _progressValue;
        public int ProgressValue
        {
            get { return _progressValue; }
            set
            {
                _progressValue = value;
                PropChanged("Value");
            }
        }

        public void PropChanged(string field)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("field"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
