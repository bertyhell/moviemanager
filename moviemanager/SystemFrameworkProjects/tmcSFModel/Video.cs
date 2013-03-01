using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Windows;
using System.ComponentModel;
using Tmc.SystemFrameworks.Common;
using Tmc.SystemFrameworks.Model.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tmc.SystemFrameworks.Model
{
    //TODO 060 add 'toevoegdatum' voor videos

    public enum VideoTypeEnum { Video, Movie, Episode };

    public class Video : INotifyPropertyChanged, IPreviewInfoRetriever
    {
        private int _id;
        private String _idImdb;
        private String _name;
        private DateTime _release = new DateTime(1800, 1, 1);
        private double _rating;
        private double _ratingImdb;
        private List<String> _genres;
        private ulong _lastPlayLocation;
        private uint _playCount;
        //properties for searchresults
        private ImageInfo _poster;
        //private List<ImageInfo> _images;
        private String _plot;
        private bool _analyseCompleted;
        private VideoTypeEnum _videoType;
        private List<VideoFile> _files;

        private MovieInfo _movieInfo;
        private EpisodeInfo _episodeInfo;

        public Video(VideoTypeEnum videoType = VideoTypeEnum.Video)
            : this()
        {
            VideoType = videoType;
            if (VideoType == VideoTypeEnum.Movie)
            {
                MovieInfo = new MovieInfo();
            }
            else if (VideoType == VideoTypeEnum.Episode)
            {
                EpisodeInfo = new EpisodeInfo();
            }
        }

        public Video()
        {
			_images = new List<ImageInfo>();
            _genres = new List<string>();
            _analyseCompleted = false;
            _files = new List<VideoFile>();
        }

        public Video(string path, params Subtitle[] subs)
            : this()
        {
            Files.Add(new VideoFile { Path = path, Subs = new ObservableList<Subtitle>(subs) });
        }

        public Video(Video brother)
        {
            _id = brother.Id;
            _idImdb = brother.IdImdb;
            _name = brother.Name;
            _release = brother.Release;
            _rating = brother.Rating;
            _ratingImdb = brother.RatingImdb;
            _genres = brother.Genres;
            _files = brother.Files;
            _lastPlayLocation = brother.LastPlayLocation;
            _playCount = brother.PlayCount;
            _poster = brother.Poster;
            _images = brother.Images;
            _plot = brother.Plot;
            _runtime = brother.Runtime;
            _analyseCompleted = brother.AnalyseCompleted;
        }

        public void CopyAnalyseVideoInfo(Video brother, Boolean overwrite = true)
        {
            //TODO 040 default null values opslaan in centrale locatie
            //TODO 080 also do this for movie and episode
            //Id = Id == 0 || overwrite ? brother.Id : Id;
            IdImdb = string.IsNullOrEmpty(IdImdb) || overwrite ? brother.IdImdb : IdImdb;
            Name = string.IsNullOrEmpty(Name) || overwrite ? brother.Name : Name;
            Release = Release.Year == 1900 || overwrite ? brother.Release : Release;
            RatingImdb = RatingImdb < 0 || overwrite ? brother.RatingImdb : RatingImdb;
            Genres = Genres == null || Genres.Count == 0 || overwrite ? brother.Genres : Genres;
            Files = Files == null || overwrite ? brother.Files : Files;
            Poster = Poster == null || overwrite ? brother.Poster : Poster;

            if (Images == null || Images.Count == 0 || overwrite)
            {
                if (Images == null)
                    Images = new List<ImageInfo>();
				else
                {
					//Images.Clear(); --> might cause problems with EF
					for (int I = Images.Count-1; I >= 0; I--)
	                {
		                Images.RemoveAt(I);
	                }
                }
                //brother.Images.ForEach(i => i.VideoId = this.Id);
                Images.AddRange(brother.Images);
            }
            Plot = string.IsNullOrEmpty(Plot) || overwrite ? brother.Plot : Plot;
            Runtime = Runtime == 0 || overwrite ? brother.Runtime : Runtime;
            AnalyseCompleted = brother.AnalyseCompleted;
        }

		[ForeignKey("Id")]
        public MovieInfo MovieInfo
        {
            get { return _movieInfo; }
            set { _movieInfo = value; }
        }

		[ForeignKey("Id")]
        public EpisodeInfo EpisodeInfo
        {
            get { return _episodeInfo; }
            set { _episodeInfo = value; }
        }

        public List<VideoFile> Files
        {
            get { return _files; }
            set { _files = value; }
        }

        //error occured because image objects get added during analyse

	    private List<ImageInfo> _images;// = new List<ImageInfo> { new ImageInfo { UriString = "http://upload.wikimedia.org/wikipedia/commons/e/e7/Mozilla_Firefox_3.5_logo_256.png" } };
		public virtual List<ImageInfo> Images
        {
            get { return _images; }
            set
            {
                _images = value;
                OnPropertyChanged("Images");
                if (_poster == null)
                    OnPropertyChanged("Poster");
            }
        }

        public List<String> Genres
        {
            get { return _genres; }
            set
            {
                _genres = value;
                OnPropertyChanged("Genres");
            }
        }

        public bool AnalyseCompleted
        {
            get { return _analyseCompleted; }
            set { _analyseCompleted = value; OnPropertyChanged("AnalyseCompleted"); }
        }

        [Key]
        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        public String IdImdb
        {
            get { return _idImdb; }
            set
            {
                _idImdb = value;
                OnPropertyChanged("IdImdb");
            }
        }
		[NotMapped]
        public ImageInfo Thumbnail
        {
            get
            {
                if (_images.Count > 0) return _images[0];
                return null;
            }
            set { _images.Insert(0, value); }
        }

        public DateTime Year
        {
            get { return _release; }
            set { _release = value; }
        }

        public String Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public double Rating
        {
            get { return _rating; }
            set
            {
                _rating = value;
                OnPropertyChanged("Rating");
            }
        }

        public double RatingImdb
        {
            get { return _ratingImdb; }
            set
            {
                _ratingImdb = value;
                OnPropertyChanged("RatingImdb");
            }
        }

        public DateTime Release
        {
            get { return _release; }
            set
            {
                _release = value;
                OnPropertyChanged("Release");
            }
        }

        private long _runtime;
        public long Runtime
        {
            get { return _runtime; }
            set { _runtime = value; }
        }

        public String ReleaseYearGuess
        {
            get
            {
                var RegEx1 = new Regex(".+[^0-9a-zA-Z]?([0-9]{4})[^0-9a-zA-Z]?");
                var Match = RegEx1.Match(Name);
                if (Match.Success)
                {
                    var ReleaseYearGuessString = Match.Groups[1].Value;
                    var ReleaseYearGuessInt = Int32.Parse(ReleaseYearGuessString);
                    if (ReleaseYearGuessInt > 1800 && ReleaseYearGuessInt < DateTime.Today.Year + 20)//TODO 001 make this an option?
                    {
                        return ReleaseYearGuessString;
                    }
                }
                return "";
            }
        }

        public ulong LastPlayLocation
        {
            get { return _lastPlayLocation; }
            set
            {
                _lastPlayLocation = value;
                OnPropertyChanged("LastPlayLocation");
            }
        }

        public uint PlayCount
        {
            get { return _playCount; }
            set
            {
                _playCount = value;
                OnPropertyChanged("PlayCount");
            }
        }

        public VideoTypeEnum VideoType
        {
            get
            {
                return _videoType;
            }
            set
            {
                if (_videoType != value)
                {
                    _videoType = value;
                    OnPropertyChanged("VideoType");
                }
            }
        }

        public void CheckVideoSeen(ulong movieLength, ulong iCurrentTimestamp, bool bWatchedToEnd)
        {
            if (bWatchedToEnd)
            {
                //mark as seen
                _playCount++;
            }
            else if (movieLength - (iCurrentTimestamp) < movieLength * 10 / 100)
            {
                MessageBoxResult Result = MessageBox.Show("Do you want to mark this video as seen?\n"
                    + "Press \"yes\" to mark this video as seen.\n"
                    + "Press \"no\" to save the current timestamp of the video.", "Choose Option", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                if (Result == MessageBoxResult.Yes)
                {
                    _playCount++;
                }
                else
                {
                    _lastPlayLocation = iCurrentTimestamp;
                }
            }
            else
            {
                //save current timestamp
                _lastPlayLocation = iCurrentTimestamp;
            }
        }

        public static List<string> ExportableProperties
        {
            get
            {
                return new List<string> { "Id", "IdImdb", "Name", "Release", "Rating", "RatingImdb", "Genres", "Path", "LastPlayLocation", "PlayCount", "Subs", "Poster", "Images", "Plot", "Runtime" };
            }
        }

        public double TitleMatchRatio { get; set; }



        public override string ToString()
        {
            return Name;
        }

        #region properties for search results

		[NotMapped]
        public ImageInfo Poster
        {
            get
            {
                if (_poster == null && _images != null && _images.Count > 0)
                    return _images[0];
                return _poster;
            }
            set
            {
                _poster = value;
                OnPropertyChanged("Poster");
            }
        }

        //[NotMapped]
        //public List<ImageInfo> Images
        //{
        //    get { return _images; }
        //    set
        //    {
        //        _images = value;
        //        OnPropertyChanged("Images");
        //        if (_poster == null)
        //            OnPropertyChanged("Poster");
        //    }
        //}

        public String Plot
        {
            get { return _plot; }
            set
            {
                _plot = value;
                OnPropertyChanged("Plot");
            }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }


        //TODO 040: Replace backup props with backup Video object

    }
}
