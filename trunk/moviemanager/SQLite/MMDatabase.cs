using System;
using System.Collections.Generic;
using Model;
using System.Data.SQLite;
using System.Globalization;


namespace SQLite
{
    public delegate void VideosChanged();

    public class MMDatabase
    {
        public static event VideosChanged OnVideosChanged;

        public static List<Video> SelectAllVideos()
        {
            List<Video> Videos = new List<Video>();

            try
            {
                //get all videos from  database
                DsVideos DatasetVideos = new DsVideos();
                FillDatasetWithAllVideos(DatasetVideos);

                //to parse dates
                DateTimeFormatInfo Format = new DateTimeFormatInfo { FullDateTimePattern = "G" };

                //Convert to List<Video>
                foreach (DsVideos.videosRow Row in DatasetVideos.Tables["Videos"].Rows)
                {
                    string SRelease = (Row["release"] == DBNull.Value ? null : Convert.ToString(Row["release"]));
                    DateTime Release = (!string.IsNullOrEmpty(SRelease) ? DateTime.Parse(SRelease, Format) : new DateTime());
                    Video Video = new Video
                                      {
                                          Id = Convert.ToInt32(Row["id"]),
                                          IdImdb = (Row["id_imdb"] == DBNull.Value ? null : Convert.ToString(Row["id_imdb"])),
                                          Name = Convert.ToString(Row["name"]),
                                          Release = Release,
                                          Rating = (Row["rating"] == DBNull.Value ? -1 : Convert.ToDouble(Row["rating"])),
                                          RatingImdb = (Row["rating_imdb"] == DBNull.Value ? -1 : Convert.ToDouble(Row["rating_imdb"])),
                                          //TODO 015: Maak koppeltabel voor genres 
                                          Genres = new List<String> { (Row["genre"] == DBNull.Value ? "" : Convert.ToString(Row["genre"])) },
                                          Path = (Row["path"] == DBNull.Value ? null : Convert.ToString(Row["path"])),
                                          LastPlayLocation = Convert.ToInt32(Row["last_play_location"])
                                      };
                    DsVideos.moviesRow MoviesRow = DatasetVideos.Tables["Movies"].Rows.Find(Video.Id) as DsVideos.moviesRow;
                    if (MoviesRow != null)
                    {
                        Video = Video as Movie;
                    }
                    else
                    {
                        DsVideos.episodesRow EpisodeRow = DatasetVideos.Tables["Episodes"].Rows.Find(Video.Id) as DsVideos.episodesRow;
                        if (EpisodeRow != null)
                        {
                            Video = Video as Episode;
                        }
                    }
                    Videos.Add(Video);
                }
            }
            catch { }

            return Videos;
        }

        public static void InsertVideosHDD(List<Video> videos)
        {

            SQLiteDataAdapter Adap = Database.GetAdapter("select * from videos");
            SQLiteCommandBuilder Builder = new SQLiteCommandBuilder(Adap);
            DsVideos DatasetVideos = new DsVideos();
            foreach (Video Video in videos)
            {
                DsVideos.videosRow Row = DatasetVideos.videos.NewvideosRow();
                Row.path = Video.Path;
                Row.name = Video.Name;
                DatasetVideos.videos.AddvideosRow(Row);
            }

            Adap.Update(DatasetVideos, "videos");

            if (OnVideosChanged != null)
                OnVideosChanged();
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
            Dictionary<String, String> Tables = new Dictionary<String, String>();
            Tables.Add(datasetVideos.Tables["Videos"].TableName, "SELECT * FROM Videos");
            Tables.Add(datasetVideos.Tables["Movies"].TableName, "SELECT * FROM Movies");
            Tables.Add(datasetVideos.Tables["Episodes"].TableName, "SELECT * FROM Episodes");

            Database.FillDataset(datasetVideos, Tables);
        }

        #endregion
    }
}
