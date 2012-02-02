using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Model;
using System.Data.SQLite;
using System.Globalization;
using System.Data.Common;


namespace SQLite
{
    public delegate void VideosChanged();

    public class MMDatabase
    {
        public static event VideosChanged OnVideosChanged;

        public static ObservableCollection<Video> SelectAllVideos()
        {
            ObservableCollection<Video> videos = new ObservableCollection<Video>();

            try
            {
                //get all videos from  database
                DsVideos datasetVideos = new DsVideos();
                FillDatasetWithAllVideos(datasetVideos);

                //to parse dates
                DateTimeFormatInfo format = new DateTimeFormatInfo { FullDateTimePattern = "G" };

                //Convert to ObservableCollection<Video>
                foreach (DsVideos.videosRow row in datasetVideos.Tables["Videos"].Rows)
                {
                    string sRelease = (row["release"] == DBNull.Value ? null : Convert.ToString(row["release"]));
                    DateTime release = (!string.IsNullOrEmpty(sRelease) ? DateTime.Parse(sRelease, format) : new DateTime());
                    Video video = new Video
                                      {
                                          Id = Convert.ToInt32(row["id"]),
                                          IdImdb = (row["id_imdb"] == DBNull.Value ? null : Convert.ToString(row["id_imdb"])),
                                          Name = Convert.ToString(row["name"]),
                                          Release = release,
                                          Rating = (row["rating"] == DBNull.Value ? -1 : Convert.ToDouble(row["rating"])),
                                          RatingImdb = (row["rating_imdb"] == DBNull.Value ? -1 : Convert.ToDouble(row["rating_imdb"])),
                                          //TODO 015: Maak koppeltabel voor genres 
                                          Genres = new ObservableCollection<string> { (row["genre"] == DBNull.Value ? "" : Convert.ToString(row["genre"])) },
                                          Path = (row["path"] == DBNull.Value ? null : Convert.ToString(row["path"])),
                                          LastPlayLocation = Convert.ToInt32(row["last_play_location"])
                                      };
                    DsVideos.moviesRow moviesRow = datasetVideos.Tables["Movies"].Rows.Find(video.Id) as DsVideos.moviesRow;
                    if (moviesRow != null)
                    {
                        video = video as Movie;
                    }
                    else
                    {
                        DsVideos.episodesRow episodeRow = datasetVideos.Tables["Episodes"].Rows.Find(video.Id) as DsVideos.episodesRow;
                        if (episodeRow != null)
                        {
                            video = video as Episode;
                        }
                    }
                    videos.Add(video);
                }
            }
            catch
            { }

            return videos;
        }

        public static DbDataReader GetVideosDataReader()
        {
            return Database.GetReader("select * from videos");
        }

        public static ObservableCollection<Video> InsertVideosHDD(ObservableCollection<Video> videos)
        {
            return InsertVideosHDD(videos,false);
        }
        public static void InsertVideosHDDWithDuplicates(ObservableCollection<Video> videos)
        {
            InsertVideosHDD(videos, true);
        }

        private static ObservableCollection<Video> InsertVideosHDD(IEnumerable<Video> videos, bool insertDuplicates)
        {
            ObservableCollection<Video> duplicates = new ObservableCollection<Video>();
            SQLiteDataAdapter adap = Database.GetAdapter("select * from videos"); 
            DsVideos datasetVideos = new DsVideos();
            FillDatasetWithAllVideos(datasetVideos);
            foreach (Video video in videos)
            {
                if (insertDuplicates || datasetVideos.videos.Select(datasetVideos.videos.pathColumn.ColumnName + " = '" + video.Path + "'").Length == 0)
                {

                    DsVideos.videosRow row = datasetVideos.videos.NewvideosRow();
                    row.path = video.Path;
                    row.name = video.Name;
                    datasetVideos.videos.AddvideosRow(row);
                }
                else
                {
                    duplicates.Add(video);
                }
            }

            adap.Update(datasetVideos, "videos");

            if (OnVideosChanged != null)
                OnVideosChanged();

            //return the duplicates that are not inserted in the table
            return duplicates;
        }
        public static void EmptyTable(String tableName)
        {
            Database.ExecuteSQL("DELETE FROM " + tableName);
            if (OnVideosChanged != null)
                OnVideosChanged();
        }


        # region fill dataset

        private static void FillDatasetWithAllVideos(DsVideos datasetVideos)
        {
            Dictionary<String, String> tables = new Dictionary<String, String>();
            tables.Add(datasetVideos.Tables["Videos"].TableName, "SELECT * FROM Videos");
            tables.Add(datasetVideos.Tables["Movies"].TableName, "SELECT * FROM Movies");
            tables.Add(datasetVideos.Tables["Episodes"].TableName, "SELECT * FROM Episodes");

            Database.FillDataset(datasetVideos, tables);
        }

        #endregion

        public static void InsertVideoHDD(Video video)
        {
            throw new NotImplementedException();
        }

        public static List<String> GetMovieGenres()
        {
            List<String> genres = new List<string>();
            DbDataReader reader = Database.GetReader("SELECT gen_label FROM genres");
            while(reader.Read())
            {
                genres.Add((string) reader[0]);
            }
            return genres;
        } 
    }
}
