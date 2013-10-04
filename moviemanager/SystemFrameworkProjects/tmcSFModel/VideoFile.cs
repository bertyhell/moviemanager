using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Tmc.SystemFrameworks.Common;
using Tmc.SystemFrameworks.Model.Annotations;

namespace Tmc.SystemFrameworks.Model
{
    public class VideoFile : INotifyPropertyChanged
    {
        private ObservableList<Subtitle> _subs; //Subtitles of the formats .cdg, .idx, .srt, .sub, .utf, .ass, .ssa, .aqt, .jss, .psb, .rt and smi are supported.
        private String _path; //path to movie


        public VideoFile()
        {
            _subs = new ObservableList<Subtitle>();
        }

        [Key]
        public int Id { get; set; }

        public String Path
        {
            get { return _path; }
            set
            {
                if (value == _path) return;
                _path = value;
                OnPropertyChanged();
            }
        }

        public ObservableList<Subtitle> Subs
        {
            get { return _subs; }
            set
            {
                if (Equals(value, _subs)) return;
                _subs = value;
                OnPropertyChanged();
            }
        }

	    public override string ToString()
	    {
		    return Path;
	    }

	    //public virtual Video Video { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
