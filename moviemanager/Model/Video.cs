﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace Model
{
    //TODO 060 add 'toevoegdatum' voor videos

    public enum VideoTypeEnum { Video, Movie, Episode };

    public class Video
    {
        private int _id;
        private String _idImdb;
        private String _name;
        private DateTime _release;
        private double _rating;
        private double _ratingImdb;
        private List<String> _genres;
        private String _path; //path to movie
        private int _lastPlayLocation = 0;
        private bool _watchedToEnd = false;
        private ObservableCollection<Subtitle> _subs; //Subtitles of the formats .cdg, .idx, .srt, .sub, .utf, .ass, .ssa, .aqt, .jss, .psb, .rt and smi are supported. 
        //properties for searchresults
        private Uri _poster;
        private List<ImageInfo> _images;
        private String _plot;

        public Video()
        {
            _images =new List<ImageInfo>();
            _subs = new ObservableCollection<Subtitle>();
        }

        public virtual VideoTypeEnum VideoType
        {
            get { return VideoTypeEnum.Video; }
        }

        public static Video ConvertVideo(VideoTypeEnum resultingVideoType, Video video)
        {
            if (resultingVideoType == VideoTypeEnum.Movie)
            {
                return new Movie()
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
            else if (resultingVideoType == VideoTypeEnum.Episode)
            {
                return new Episode()
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
            else
            {
                return new Video()
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
        }

        //return 
        public List<String> Genres
        {
            get { return _genres; }
            set { _genres = value; }
        }

        public String GetGenresString()
        {
            if (_genres.Count > 0)
            {
                String GenresString = _genres[0];
                for (int i = 1; i < _genres.Count; i++)
                {
                    GenresString += ", " + _genres[i];
                }
                return GenresString;
            }
            else
            {
                return "";
            }
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public String IdImdb
        {
            get { return _idImdb; }
            set { _idImdb = value; }
        }

        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public String Path
        {
            get { return _path; }
            set
            {
                _path = value;
                if (string.IsNullOrEmpty(_name))
                    _name = _path.Substring(_path.LastIndexOf("/") + 1, _path.LastIndexOf("."));
            }
        }

        public double Rating
        {
            get { return _rating; }
            set { _rating = value; }
        }

        public double RatingImdb
        {
            get { return _ratingImdb; }
            set { _ratingImdb = value; }
        }

        public DateTime Release
        {
            get { return _release; }
            set { _release = value; }
        }

        public ObservableCollection<Subtitle> Subs
        {
            get { return _subs; }
            set { _subs = value; }
        }

        public int LastPlayLocation
        {
            get { return _lastPlayLocation; }
            set { _lastPlayLocation = value; }
        }

        public bool WatchedToEnd
        {
            get { return _watchedToEnd; }
            set { _watchedToEnd = value; }
        }

        public void MarkAsSeen(int movieLength, int iCurrentTimestamp, bool bWatchedToEnd)
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

        #region properties for search results

        public Uri Poster
        {
            get { return _poster; }
            set { _poster = value; }
        }

        public List<ImageInfo> Images
        {
            get { return _images; }
            set { _images = value; }
        }

        public String Plot
        {
            get { return _plot; }
            set { _plot = value; }
        }

        #endregion
    }
}
