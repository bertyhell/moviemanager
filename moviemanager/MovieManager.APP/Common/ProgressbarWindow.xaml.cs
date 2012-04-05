using System.ComponentModel;
using System.Windows;

namespace MovieManager.APP.Common
{
    /// <summary>
    /// Interaction logic for ProgressbarWindow.xaml
    /// </summary>
    public partial class ProgressbarWindow : INotifyPropertyChanged
    {
        public ProgressbarWindow()
        {
            InitializeComponent();
        }

        public string ProgressString
        {
            get { return progressBarControl1.ProgressString; }
            set
            {
                progressBarControl1.ProgressString = value;
                PropChanged("ProgressString");
            }
        }

        public bool IsIndeterminate
        {
            get { return progressBarControl1.IsIndeterminate; }
            set
            {
                progressBarControl1.IsIndeterminate = value;
                PropChanged("IsIndeterminate");
            }
        }

        public void PropChanged(string field)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(field));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
