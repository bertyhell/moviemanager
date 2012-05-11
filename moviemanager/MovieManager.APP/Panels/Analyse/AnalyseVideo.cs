using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media;
using Model;

namespace MovieManager.APP.Panels.Analyse
{
    class AnalyseVideo : INotifyPropertyChanged
    {
        public AnalyseVideo()
        {
            Candidates = new List<Video>();
            SelectedCandidateIndex = -1;
            MatchPercentage = -1;
        }

        private Video _video;
        public Video Video
        {
            get { return _video; }
            set
            {
                _video = value;
                PropChanged("Video");
            }
        }

        private IList<Video> _candidates;
        public IList<Video> Candidates
        {
            get { return _candidates; }
            set
            {
                _candidates = value;
                PropChanged("Candidates");
            }
        }

        private int _selectedCandidateIndex;
        public int SelectedCandidateIndex
        {
            get { return _selectedCandidateIndex; }
            set
            {
                _selectedCandidateIndex = value;
                PropChanged("SelectedCandidateIndex");
            }
        }

        private void PropChanged(string field)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(field));
            }
        }

        private int _matchPercentage;
        public int MatchPercentage
        {
            get
            {
                return _matchPercentage;//Brushes.BlueViolet; 
            }
            set
            {
                _matchPercentage = value;
                PropChanged("MatchPercentage");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
