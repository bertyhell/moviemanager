using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Tmc.SystemFrameworks.Model
{
    public class AnalyseVideo : INotifyPropertyChanged
    {
        public AnalyseVideo()
        {
            Candidates = new List<Video>();
            SelectedCandidateIndex = -1;
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

        //filled by a couple of title guesses derived from the folder and filename of the video
        private List<string> _titleGuesses;
        public List<string> TitleGuesses
        {
            get { return _titleGuesses; }
            set
            {
                _titleGuesses = value;
                PropChanged("TitleGuesses");
            }
        }

        private List<Video> _candidates;
        public List<Video> Candidates
        {
            get { return _candidates; }
            set
            {
                _candidates = value;
                if (Candidates.Count > 0) SelectedCandidateIndex = 0;
                PropChanged("Candidates");
                PropChanged("MatchPercentage");
                PropChanged("SelectedCandidateIndex");
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
                PropChanged("MatchPercentage");
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
                return Candidates[SelectedCandidateIndex];
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

        public double MatchPercentage
        {
            get
            {
                if (AnalyseNeeded) return -1;
                if (Candidates.Count > 0)
                {
                    return Math.Sin(Candidates[SelectedCandidateIndex].TitleMatchRatio * Math.PI - Math.PI / 2) / 2 + 0.5; //good devision between red and green (not much matchrations close to 0 or 0.3 --> sin pushes values more to 0 and 1 --> colors are more extreme)
                }
                return 0;//red
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
