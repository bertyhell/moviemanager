using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.ComponentModel;

namespace Model
{
    //TODO 060 add 'toevoegdatum' voor videos

    public enum VideoTypeEnum { Video, Movie, Episode };

    public class Video : INotifyPropertyChanged, IEditableObject
    {
        private int _id;
        private String _idImdb;
        private String _name;
        private DateTime _release;
        private String _releaseYearGuess = "";
        private double _rating;
        private double _ratingImdb;
        private List<String> _genres;
        private String _path; //path to movie
        private ulong _lastPlayLocation;
        private bool _watchedToEnd;
        private ObservableCollection<Subtitle> _subs; //Subtitles of the formats .cdg, .idx, .srt, .sub, .utf, .ass, .ssa, .aqt, .jss, .psb, .rt and smi are supported. 
        //properties for searchresults
        private Uri _poster;
        private List<ImageInfo> _images;
        private String _plot;
        private bool _analyseCompleted;

        public Video()
        {
            _images = new List<ImageInfo>();
            _subs = new ObservableCollection<Subtitle>();
            _genres = new List<string>();
            _analyseCompleted = false;
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
            _path = brother.Path;
            _lastPlayLocation = brother.LastPlayLocation;
            _watchedToEnd = brother.WatchedToEnd;
            _subs = brother.Subs;
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
            IdImdb = string.IsNullOrEmpty(IdImdb) || overwrite ? brother.IdImdb : IdImdb;
            Name = string.IsNullOrEmpty(Name) || overwrite ? brother.Name : Name;
            Release = Release.Year == 1900 || overwrite ? brother.Release : Release;
            RatingImdb = RatingImdb < 0 || overwrite ? brother.RatingImdb : RatingImdb;
            Genres = Genres == null || Genres.Count == 0 || overwrite ? brother.Genres : Genres;
            Subs = Genres == null || Subs.Count == 0 || overwrite ? brother.Subs : Subs;
            Poster = Poster == null || overwrite ? brother.Poster : Poster;
            Images = Images == null ||Images.Count == 0 || overwrite ? brother.Images : Images;
            Plot = string.IsNullOrEmpty(Plot) || overwrite ? brother.Plot : Plot;
            Runtime = Runtime == 0 || overwrite ? brother.Runtime : Runtime;
            AnalyseCompleted = brother.AnalyseCompleted;
        }

        public virtual VideoTypeEnum VideoType
        {
            get { return VideoTypeEnum.Video; }
        }

        public static Video ConvertVideo(VideoTypeEnum resultingVideoType, Video video)
        {
            if (resultingVideoType == VideoTypeEnum.Movie)
            {
                return new Movie
                {
                    Id = video.Id,
                    IdImdb = video.IdImdb,
                    Name = video.Name,
                    Release = video.Release,
                    Rating = video.Rating,
                    RatingImdb = video.RatingImdb,
                    Path = video.Path,
                    LastPlayLocation = video.LastPlayLocation
                };

            }
            if (resultingVideoType == VideoTypeEnum.Episode)
            {
                return new Episode
                {
                    Id = video.Id,
                    IdImdb = video.IdImdb,
                    Name = video.Name,
                    Release = video.Release,
                    Rating = video.Rating,
                    RatingImdb = video.RatingImdb,
                    Path = video.Path,
                    LastPlayLocation = video.LastPlayLocation
                };
            }
            return new Video
            {
                Id = video.Id,
                IdImdb = video.IdImdb,
                Name = video.Name,
                Release = video.Release,
                Rating = video.Rating,
                RatingImdb = video.RatingImdb,
                Path = video.Path,
                LastPlayLocation = video.LastPlayLocation
            };
        }

        //return 
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

        public String Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public String Path
        {
            get { return _path; }
            set
            {
                _path = value;
                if (string.IsNullOrEmpty(_name))
                {
                    _name = _path.Substring(_path.LastIndexOf("/") + 1, _path.LastIndexOf("."));
                    OnPropertyChanged("Path");
                }
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

        public ObservableCollection<Subtitle> Subs
        {
            get { return _subs; }
            set
            {
                _subs = value;
                OnPropertyChanged("Subs");
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

        public bool WatchedToEnd
        {
            get { return _watchedToEnd; }
            set
            {
                _watchedToEnd = value;
                OnPropertyChanged("WatchedToEnd");
            }
        }

        public void MarkAsSeen(ulong movieLength, ulong iCurrentTimestamp, bool bWatchedToEnd)
        {
            if (bWatchedToEnd)
            {
                //mark as seen
                _watchedToEnd = true;
            }
            else if (movieLength - (iCurrentTimestamp) < movieLength * 10 / 100)
            {
                MessageBoxResult Result = MessageBox.Show("Do you want to mark this video as seen?\n"
                    + "Press \"yes\" to mark this video as seen.\n"
                    + "Press \"no\" to save the current timestamp of the video.", "Choose Option", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                if (Result == MessageBoxResult.Yes)
                {
                    _watchedToEnd = true;
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
                return new List<string> { "Id", "IdImdb", "Name", "Release", "Rating", "RatingImdb", "Genres", "Path", "LastPlayLocation", "WatchedToEnd", "Subs", "Poster", "Images", "Plot", "Runtime" };
            }
        } 

        public override string ToString()
        {
            return Name;
        }

        #region properties for search results

        public Uri Poster
        {
            get { return _poster; }
            set
            {
                _poster = value;
                OnPropertyChanged("Poster");
            }
        }

        public List<ImageInfo> Images
        {
            get { return _images; }
            set
            {
                _images = value;
                OnPropertyChanged("Images");
            }
        }

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
            if (PropertyChanged != null && !_editInProgress)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        #region ieditableobject

        private int _oldid;
        private String _oldidImdb;
        private String _oldname;
        private DateTime _oldrelease;
        private string _oldreleaseyearguess;
        private double _oldrating;
        private double _oldratingImdb;
        private List<String> _oldgenres;
        private String _oldpath; //path to movie
        private ulong _oldlastPlayLocation;
        private bool _oldwatchedToEnd;
        private ObservableCollection<Subtitle> _oldsubs; //Subtitles of the formats .cdg, .idx, .srt, .sub, .utf, .ass, .ssa, .aqt, .jss, .psb, .rt and smi are supported. 
        //properties for searchresults
        private Uri _oldposter;
        private List<ImageInfo> _oldimages;
        private String _oldplot;
        private long _oldRuntime;

        private bool _editInProgress;

        public void BeginEdit()
        {
            if (!_editInProgress)
            {
                _editInProgress = false;
                _oldid = _id;
                _oldidImdb = _idImdb;
                _oldname = _name;
                _oldrelease = _release;
                _oldreleaseyearguess = _releaseYearGuess;
                _oldrating = _rating;
                _oldratingImdb = _ratingImdb;
                _oldgenres = _genres;
                _oldlastPlayLocation = _lastPlayLocation;
                _oldwatchedToEnd = _watchedToEnd;
                _oldsubs = _subs;
                _oldposter = _poster;
                _oldimages = _images;
                _oldplot = _plot;
                _oldRuntime = _runtime;
            }
        }

        public void EndEdit()
        {
            if (_editInProgress)
            {
                _editInProgress = false;
                _oldidImdb = null;
                _oldname = null;
                _oldpath = null;
                _oldgenres = null;
                _oldposter = null;
                _oldimages = null;
                _oldplot = null;
                OnPropertyChanged("Id");
                OnPropertyChanged("IdImdb");
                OnPropertyChanged("Name");
                OnPropertyChanged("Release");
                OnPropertyChanged("ReleaseYearGuess");
                OnPropertyChanged("Rating");
                OnPropertyChanged("RatingImdb");
                OnPropertyChanged("Genres");
                OnPropertyChanged("Path");
                OnPropertyChanged("LastPlayLocation");
                OnPropertyChanged("WatchedToEnd");
                OnPropertyChanged("Poster");
                OnPropertyChanged("Images");
                OnPropertyChanged("Plot");
                OnPropertyChanged("Runtime");
            }
        }

        public void CancelEdit()
        {
            if (_editInProgress)
            {
                _editInProgress = false;
                _id = _oldid;
                _idImdb = _oldidImdb;
                _name = _oldname;
                _release = _oldrelease;
                _releaseYearGuess = _oldreleaseyearguess;
                _rating = _oldrating;
                _ratingImdb = _oldratingImdb;
                _genres = _oldgenres;
                _path = _oldpath;
                _lastPlayLocation = _oldlastPlayLocation;
                _watchedToEnd = _oldwatchedToEnd;
                _subs = _oldsubs;
                _poster = _oldposter;
                _images = _oldimages;
                _plot = _oldplot;
                _runtime = _oldRuntime;
            }
        }
        #endregion
    }
}
