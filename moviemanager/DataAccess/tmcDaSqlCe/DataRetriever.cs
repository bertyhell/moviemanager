using System;
using System.Data.Entity;
using System.Linq;
using System.Collections.Generic;
using Tmc.SystemFrameworks.Common;
using Tmc.SystemFrameworks.Model;

namespace Tmc.DataAccess.SqlCe
{
    public class DataRetriever
    {
        public static readonly int CURRENT_DATABASE_VERSION = 1;

        private static TmcContext _db;

        public static void Init(string connectionString)
        {
            _db = new TmcContext(connectionString);
        }

        public static IList<Video> Videos
        {
            get
            {
                List<Video> Vids = _db.Videos.Include(x => x.Files).Include(x => x.Images).ToList();
                return Vids;
            }
            set
            {
                UpdateVideos(value);
                OnVideosChanged();
            }
        }

        public static List<Genre> Genres
        {
            get { return _db.Genres.ToList(); }
            set
            {
                foreach (Genre Genre in value)
                {
                    Genre DbGenre = _db.Genres.FirstOrDefault(g => g.Name == Genre.Name);
                    if (DbGenre == null)
                    {
                        //add to database
                        _db.Genres.Add(Genre);
                    }
                    else
                    {
                        //update tmdb id
                        DbGenre.TmdbId = Genre.TmdbId;
                    }
                }
            }
        }

        public static List<Serie> Series
        {
            get { return _db.Series.ToList(); }
            set { UpdateSeries(value); }
        }

        private static void UpdateAndRemoveSeries(IList<Serie> series)
        {
            foreach (Serie Serie in _db.Series)
            {
                Serie SerieFromList = series.FirstOrDefault(s => s.Id == Serie.Id);
                if (SerieFromList == null)
                {
                    RemoveSerie(Serie);
                }
                else
                {
                    UpdateSerie(SerieFromList);
                }
            }
        }

        public static void UpdateSeries(IList<Serie> series)
        {
            foreach (Serie Serie in series)
            {
                UpdateSerie(Serie);
            }
        }

        public static void UpdateSerie(Serie serie)
        {

            Serie ExistingSerie = _db.Series.FirstOrDefault(s => s.Id == serie.Id);
            if (ExistingSerie == null)
            {
                _db.Series.Add(serie);
            }
            else
            {
                //TODO 090: copy information to the one in the database
            }
        }

        public static void RemoveSerie(Serie serie)
        {
            _db.Series.Remove(serie);
        }


        private static void UpdateVideos(IList<Video> videos)
        {
            //int Progress = 0;
            foreach (Video NewVideo in videos)
            {
                //check if video exists
                Video ExistingVideo = null;
                if (_db.Videos.Any(v => v.Id == NewVideo.Id))
                {
                    //database contains this video (match by id)
                    ExistingVideo = _db.Videos.First(v => v.Id == NewVideo.Id);
                }
                else
                {
                    foreach (Video DbVideo in _db.Videos)
                    {
                        if (DbVideo.Files.Any(p => p.Path == NewVideo.Files[0].Path))
                        {
                            //database already contains this video (match by video file path)
                            ExistingVideo = DbVideo;
                        }
                    }
                }
                if (ExistingVideo != null)
                {
                    //video already in database
                    ExistingVideo.CopyAnalyseVideoInfo(NewVideo);
                }
                else
                {
                    //new video --> add to database
                    _db.Videos.Add(NewVideo);
                }
                //raise event update/insert video progress
                //Progress++;
                //OnUpdateVideosProgress(new ProgressEventArgs { MaxNumber = videos.Count, ProgressNumber = Progress, Message = "Video " + Progress + " / " + videos.Count });
            }
            _db.SaveChanges();
            OnVideosChanged();
        }

        public static void EmptyVideoTables()
        {
            foreach (Video Video in _db.Videos)
            {
                _db.Videos.Remove(Video);
            }
            _db.SaveChanges();
            OnVideosChanged();
        }

        public static void UpdateVideo(Video video)
        {
            UpdateVideos(new List<Video> { video });
        }

        public static bool ConvertDatabase()
        {
            return false;
        }

        public static DatabaseDetails GetDatabaseDetails()
        {
            return null;
        }


        public static event OnInsertVideosProgress UpdateVideosProgress;

        public static void OnUpdateVideosProgress(ProgressEventArgs eventargs)
        {
            if (UpdateVideosProgress != null) UpdateVideosProgress(null, eventargs);
        }

        public delegate void OnInsertVideosProgress(object sender, ProgressEventArgs eventArgs);

        public delegate void VideosChangedDel();

        public static event VideosChangedDel VideosChanged;

        private static void OnVideosChanged()
        {
            if (VideosChanged != null)
                VideosChanged();
        }

    }
}
