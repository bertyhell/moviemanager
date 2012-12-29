using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Model;
using Tmc.SystemFrameworks.Common;

namespace Tmc.DataAccess.SqlCe
{
    public class DataRetriever
    {
        public static readonly int CURRENT_DATABASE_VERSION = 1;

        private static readonly TmcContext DB = new TmcContext();

        public static void Init(String pathToDatabase)
        {
        }


        public static List<Video> Videos
        {
            get { return new List<Video>(DB.Videos); }
            set { throw new NotImplementedException(); }
        }

        public static List<string> Genres
        {
            get { return null; }
        }

        public static List<Serie> Series
        {
            get { return null; }
            set { throw new NotImplementedException(); }
        }


        public static void AddSerie(Serie serie)
        {
        }

        public static IList<Video> InsertVideosHdd(IList<Video> videos)
        {
            return InsertVideosHdd(videos, false);
        }

        public static void InsertVideosHddWithDuplicates(ObservableCollection<Video> videos)
        {
            InsertVideosHdd(videos, true);
        }

        private static IList<Video> InsertVideosHdd(IList<Video> videos, bool insertDuplicates)
        {
            return null;
        }

        public static void EmptyVideoTables()
        {
        }


        public static bool UpdateVideos(List<Video> videos)
        {
            return false;
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
