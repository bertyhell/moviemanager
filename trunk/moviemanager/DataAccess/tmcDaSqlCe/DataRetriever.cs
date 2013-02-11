using System;
using System.Data.Entity;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Tmc.SystemFrameworks.Common;
using Tmc.SystemFrameworks.Model;

namespace Tmc.DataAccess.SqlCe
{
    public class DataRetriever
    {
        public static readonly int CURRENT_DATABASE_VERSION = 1;

        private static TmcContext DB;

        public static void Init(string connectionString)
        {
            DB = new TmcContext(connectionString);
        }


        public static IList<Video> Videos
        {
            get { return DB.Videos.Include(x => x.Files).ToList(); }
            set
            {
                UpdateVideos(value);
                OnVideosChanged();
            }
        }

        public static List<string> Genres
        {
            get
            {

                return null;
            }
        }

        public static List<Serie> Series
        {
            get { return null; }
            set { throw new NotImplementedException(); }
        }


        public static void AddSerie(Serie serie)
        {
        }

        private static void UpdateVideos(IEnumerable<Video> videos)
        {
            foreach (Video Video in videos)
            {
                   //check if video exists
                Video DbVideo = DB.Videos.FirstOrDefault(v => v.Id == Video.Id);
                if (DbVideo != null)
                {
                    DbVideo.CopyAnalyseVideoInfo(Video, true);
                }
                else
                {
                    List<Video> DbVideos = new List<Video>();
                    foreach (Video DBVideo in DB.Videos)
                    {
                        if (DBVideo.Files[0].Path == Video.Files[0].Path)
                        {
                            DbVideos.Add(DBVideo);
                        }
                    }
                    if (string.IsNullOrWhiteSpace(Video.Files[0].Path) || !DbVideos.Any())
                    {
                        DB.Videos.Add(Video);
                    }
                    else
                    {
                        //TODO 090: Implement this
                    }
                }
            }
            DB.SaveChanges();
            OnVideosChanged();
        }

        public static void EmptyVideoTables()
        {
            foreach (Video Video in DB.Videos)
            {
                DB.Videos.Remove(Video);
            }
            DB.SaveChanges();
            OnVideosChanged();
        }

        //public static bool UpdateVideo(Video video, DsVideos datasetVideos)
        //{
        //    return false;
        //}


        public static bool ConvertDatabase()
        {
            return false;
        }

        public static DatabaseDetails GetDatabaseDetails()
        {
            return null;
        }


        public static event OnInsertVideosProgress InsertVideosProgress;

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
