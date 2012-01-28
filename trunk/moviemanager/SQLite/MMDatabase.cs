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
                DateTimeFormatInfo Format = new DateTimeFormatInfo { FullDateTimePattern = "G" };

                //Convert to ObservableCollection<Video>
                foreach (DsVideos.videosRow row in datasetVideos.Tables["Videos"].Rows)
                {
                    string sRelease = (row["release"] == DBNull.Value ? null : Convert.ToString(row["release"]));
                    DateTime release = (!string.IsNullOrEmpty(sRelease) ? DateTime.Parse(sRelease, Format) : new DateTime());
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
                    DsVideos.moviesRow MoviesRow = datasetVideos.Tables["Movies"].Rows.Find(video.Id) as DsVideos.moviesRow;
                    if (MoviesRow != null)
                    {
                        video = video as Movie;
                    }
                    else
                    {
                        DsVideos.episodesRow EpisodeRow = datasetVideos.Tables["Episodes"].Rows.Find(video.Id) as DsVideos.episodesRow;
                        if (EpisodeRow != null)
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
            ObservableCollection<Video> Duplicates = new ObservableCollection<Video>();
            SQLiteDataAdapter Adap = Database.GetAdapter("select * from videos"); 
            SQLiteCommandBuilder Builder = new SQLiteCommandBuilder(Adap);
            DsVideos DatasetVideos = new DsVideos();
            FillDatasetWithAllVideos(DatasetVideos);
            foreach (Video Video in videos)
            {
                if (insertDuplicates || DatasetVideos.videos.Select(DatasetVideos.videos.pathColumn.ColumnName + " = '" + Video.Path + "'").Length == 0)
                {

                    DsVideos.videosRow row = DatasetVideos.videos.NewvideosRow();
                    row.path = Video.Path;
                    row.name = Video.Name;
                    DatasetVideos.videos.AddvideosRow(row);
                }
                else
                {
                    Duplicates.Add(Video);
                }
            }

            Adap.Update(DatasetVideos, "videos");

            if (OnVideosChanged != null)
                OnVideosChanged();

            //return the duplicates that are not inserted in the table
            return Duplicates;
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
    }
}
