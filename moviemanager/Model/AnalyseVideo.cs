using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Model
{
    public class AnalyseVideo : INotifyPropertyChanged
    {
        public AnalyseVideo()
        {
            Candidates = new ObservableCollection<Video>();
            SelectedCandidateIndex = -1;
            MatchPercentage = -1;
            AnalyseNeeded = true;
        }

        private bool _analyseNeeded;
        public bool AnalyseNeeded
        {
            get { return _analyseNeeded; }
            set
            {
                _analyseNeeded = value;
                PropChanged("AnalyseNeeded");
            }
        }

        private Video _video;
        public Video Video
        {
            get { return _video; }
            set
            {
                _video = value;
                if (SearchString == null)
                {
                    SearchString = _video.Name;
                }
                PropChanged("Video");
            }
        }

        private ObservableCollection<Video> _candidates;

        public ObservableCollection<Video> Candidates
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
                PropChanged("SelectedCandidate");
            }
        }

        public Video SelectedCandidate
        {
            get
            {
                if (SelectedCandidateIndex == -1)
                {
                    return null;
                }
                else
                {
                    return Candidates[SelectedCandidateIndex];
                }
            }
        }

        private string _searchString;
        public string SearchString
        {
            get { return _searchString; }
            set
            {
                _searchString = value;
                PropChanged("SearchString");
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
