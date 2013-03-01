using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tmc.SystemFrameworks.Model
{
    public class EpisodeInfo : INotifyPropertyChanged
    {
        private int _season = 1;
        private int _episodeNumber = 1;
        private int _serieId;

        [Key]
        public int Id { get; set; }

        public int EpisodeNumber
        {
            get { return _episodeNumber; }
            set
            {
                _episodeNumber = value;
                OnPropertyChanged("EpisodeNumber");
            }
        }

        public int Season
        {
            get { return _season; }
            set
            {
                _season = value;
                OnPropertyChanged("Season");
            }
        }

        public int SerieId
        {
            get { return _serieId; }
            set
            {
                _serieId = value;
                OnPropertyChanged("SerieId");
            }
        }

		//public virtual Video Video { get; set; }

        protected void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
