using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MovieManager.APP.Panels.AddVideos
{
    class EpisodeRegexEditorViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<string> _regularExpressions;

        public ObservableCollection<string> RegularExpressions
        {
            get { return _regularExpressions; }
            set { _regularExpressions = value; NotifyPropChanged("RegularExpressions");
            }
        }

        private ObservableCollection<string> _usedRegularExpressions;

        public ObservableCollection<string> UsedRegularExpressions
        {
            get { return _usedRegularExpressions; }
            set
            {
                _usedRegularExpressions = value; NotifyPropChanged("SelectedRegularExpressions");
            }
        }

        private string _selectedRegularExpression;
        public string SelectedRegularExpression { get { return _selectedRegularExpression; } set { _selectedRegularExpression = value; NotifyPropChanged("SelectedRegularExpression"); } }

        public void MoveRegExUp(int RegExUp)
        {
            if (RegExUp > 0 && RegExUp < _regularExpressions.Count)
            {
                string Help = _regularExpressions[RegExUp];
                _regularExpressions[RegExUp] = _regularExpressions[RegExUp - 1];
                _regularExpressions[RegExUp - 1] = Help;
                NotifyPropChanged("RegularExpressions");
                NotifyPropChanged("SelectedRegularExpressions");
            }
        }

        public void MoveRegExDown(int RegExUp)
        {
            if (RegExUp < _regularExpressions.Count - 1 && RegExUp >= 0)
            {
                string Help = _regularExpressions[RegExUp];
                _regularExpressions[RegExUp] = _regularExpressions[RegExUp + 1];
                _regularExpressions[RegExUp + 1] = Help;
                NotifyPropChanged("RegularExpressions");
                NotifyPropChanged("SelectedRegularExpressions");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropChanged(string prop)
        {
            if(PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
