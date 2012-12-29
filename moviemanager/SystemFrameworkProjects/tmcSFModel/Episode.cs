namespace Tmc.SystemFrameworks.Model
{
    public class Episode : Video
    {
        public override VideoTypeEnum VideoType
        {
            get { return VideoTypeEnum.Episode; }
        }

        private int _episodeNumber;
        public int EpisodeNumber
        {
            get { return _episodeNumber; }
            set
            {
                _episodeNumber = value;
                OnPropertyChanged("EpisodeNumber");
            }
        }

        private int _season;
        public int Season
        {
            get { return _season; }
            set
            {
                _season = value;
                OnPropertyChanged("Season");
            }
        }

        private int _serieId;
        public int SerieId
        {
            get { return _serieId; }
            set
            {
                _serieId = value;
                OnPropertyChanged("SerieId");
            }
        }
    }
}
