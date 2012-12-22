using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Tmc.WinUI.Application.Panels.RegularExpressions
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

        public void MoveRegExUp(int regExUp)
        {
            if (regExUp > 0 && regExUp < _regularExpressions.Count)
            {
                string Help = _regularExpressions[regExUp];
                _regularExpressions[regExUp] = _regularExpressions[regExUp - 1];
                _regularExpressions[regExUp - 1] = Help;
                NotifyPropChanged("RegularExpressions");
                NotifyPropChanged("SelectedRegularExpressions");
            }
        }

        public void MoveRegExDown(int regExUp)
        {
            if (regExUp < _regularExpressions.Count - 1 && regExUp >= 0)
            {
                string Help = _regularExpressions[regExUp];
                _regularExpressions[regExUp] = _regularExpressions[regExUp + 1];
                _regularExpressions[regExUp + 1] = Help;
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
