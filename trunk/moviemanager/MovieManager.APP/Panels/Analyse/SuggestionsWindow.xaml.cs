using System.ComponentModel;
using System.Windows;
using Model;

namespace MovieManager.APP.Panels.Analyse
{
    //TODO 005 make controller and move logic code

    /// <summary>
    /// Interaction logic for SuggestionsWindow.xaml
    /// </summary>
    public partial class SuggestionsWindow : Window, INotifyPropertyChanged
    {

        public SuggestionsWindow(AnalyseVideo analyseVideo)
        {
            AnalyseVideo = analyseVideo;
            InitializeComponent();
            DataContext = this;
            //progressbar.DataContext = this;
        }

        private AnalyseVideo _analyseVideo;
        public AnalyseVideo AnalyseVideo
        {
            get { return _analyseVideo; }
            set
            {
                _analyseVideo = value;
                PropChanged("AnalyseVideo");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void PropChanged(string field)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(field));
        }

        private void SearchButtonClick(object sender, RoutedEventArgs e)
        {
            //search for videos with searchtext
            AnalyseVideo.SearchString = txtSearchString.Text;
            var AnalyseWorker = new AnalyseWorker(AnalyseVideo);
            AnalyseWorker.RunWorkerAsync();
        }

        private void SearchDetailsButtonClick(object sender, RoutedEventArgs e)
        {
            GetDetailWorker DetailWorker = new GetDetailWorker(_analyseVideo.Candidates);
            DetailWorker.RunWorkerAsync();
        }
        

        
        private void BtnCancelClick(object sender, RoutedEventArgs e)
        {
            AnalyseVideo.SelectedCandidateIndex = -1;
            this.Close();
        }

        private void BtnOkClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
