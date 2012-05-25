using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using Common;
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
            this.DataContext = this;
            progressbar.DataContext = this;
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

        private Video _selectedCandidate;
        public Video SelectedCandidate
        {
            get { return _selectedCandidate; }
            set
            {
                _selectedCandidate = value;
                PropChanged("SelectedCandidate");
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
    }
}
