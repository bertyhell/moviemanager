using System.Collections.Generic;
using System.ComponentModel;
using Model;

namespace MovieManager.APP.Panels.Analyse
{
    class AnalyseVideo : INotifyPropertyChanged
    {
        public AnalyseVideo()
        {
            Candidates = new List<Video>();
            SelectedCandidateIndex = -1;
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

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
