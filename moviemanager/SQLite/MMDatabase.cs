using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using System.Data.SQLite;
using System.Globalization;


namespace SQLite
{
    public delegate void VideosChanged();

    public class MMDatabase
    {
        public static event VideosChanged onVideosChanged;

        public static List<Video> selectAllVideos()
        {
            List<Video> videos = new List<Video>();

            try
            {
                //get all videos from  database
                DsVideos datasetVideos = new DsVideos();
                FillDatasetWithAllVideos(datasetVideos);

                //to parse dates
                DateTimeFormatInfo format = new DateTimeFormatInfo();
                format.FullDateTimePattern = "G";

                //Convert to List<Video>
                foreach (DsVideos.videosRow Row in datasetVideos.Tables["Videos"].Rows)
                {
                    string sRelease = (Row["release"] == DBNull.Value ? null : Convert.ToString(Row["release"]));
                    DateTime Release = (!string.IsNullOrEmpty(sRelease) ? DateTime.Parse(sRelease, format) : new DateTime());
                    Video video = new Video()
                    {
                        Id = Convert.ToInt32(Row["id"]),
                        IdImdb = (Row["id_imdb"] == DBNull.Value ? null : Convert.ToString(Row["id_imdb"])),
                        Name = Convert.ToString(Row["name"]),
                        Release = Release,
                        Rating = (Row["rating"] == DBNull.Value ? -1 : Convert.ToDouble(Row["rating"])),
                        RatingImdb = (Row["rating_imdb"] == DBNull.Value ? -1 : Convert.ToDouble(Row["rating_imdb"])),
                        //TODO 015: Maak koppeltabel voor genres 
                        Genres = new List<String>() { (Row["genre"] == DBNull.Value ? "" : Convert.ToString(Row["genre"])) },
                        Path = (Row["path"] == DBNull.Value ? null : Convert.ToString(Row["path"])),
                        LastPlayLocation = Convert.ToInt32(Row["last_play_location"])
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
            catch { }

            return videos;
        }

        public static void insertVideosHDD(List<Video> videos)
        {

            SQLiteDataAdapter adap = Database.GetAdapter("select * from videos");
            SQLiteCommandBuilder builder = new SQLiteCommandBuilder(adap);
            DsVideos datasetVideos = new DsVideos();
            foreach (Video Video in videos)
            {
                DsVideos.videosRow row = datasetVideos.videos.NewvideosRow();
                row.path = Video.Path;
                row.name = Video.Name;
                datasetVideos.videos.AddvideosRow(row);
            }

            adap.Update(datasetVideos, "videos");

            if (onVideosChanged != null)
                onVideosChanged();
        }
        public static void emptyTable(String tableName)
        {
            Database.ExecuteSQL("DELETE FROM " + tableName);
            if (onVideosChanged != null)
                onVideosChanged();
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
