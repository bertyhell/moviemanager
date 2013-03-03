using System.ComponentModel;
using System.Runtime.CompilerServices;
using Tmc.SystemFrameworks.Model.Annotations;

namespace Tmc.SystemFrameworks.Model
{
    public class Genre : INotifyPropertyChanged
    {
        private int _id;
        private string _name;
        private int _tmdbId;

        public int Id
        {
            get { return _id; }
            set
            {
                if (value == _id) return;
                _id = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        public int TmdbId
        {
            get { return _tmdbId; }
            set
            {
                if (value == _tmdbId) return;
                _tmdbId = value;
                OnPropertyChanged();
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
