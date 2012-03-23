using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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

        public static void SelectAllVideos(ObservableCollection<Video> videos)
        {
            try
            {
                //get all videos from  database
                DsVideos datasetVideos = new DsVideos();
                FillDatasetWithAllVideos(datasetVideos);

                //to parse dates
                DateTimeFormatInfo format = new DateTimeFormatInfo { FullDateTimePattern = "G" };

                DsVideos.videos_genresDataTable videosGenresDataTable = datasetVideos.videos_genres;
                DsVideos.genresDataTable GenresDataTable = datasetVideos.genres;

                //Convert to ObservableCollection<Video>
                foreach (DsVideos.videosRow Row in datasetVideos.videos.Rows)
                {
                    DateTime Release = Row.release;
                    Video Video = new Video
                                      {
                                          Id = (int)Row.id,
                                          IdImdb = Row.id_imdb,
                                          Name = Row.name,
                                          Release = Release,
                                          Rating = Row.rating,
                                          RatingImdb = Row.rating_imdb,
                                          Path = Row.path,
                                          LastPlayLocation = (int)Row.last_play_location
                                      };

                    //get genre from dataset
                    foreach (DataRow DataRow in videosGenresDataTable.Select(videosGenresDataTable.video_idColumn.ColumnName + " = " + Video.Id))
                    {
                        int GenreID =  (int)(long)DataRow[videosGenresDataTable.genre_idColumn.ColumnName];
                        DataRow GenreRow = GenresDataTable.FindBygen_id(GenreID);
                        Video.Genres.Add((string)GenreRow[GenresDataTable.gen_labelColumn]);
                    }

                    DsVideos.moviesRow MoviesRow = datasetVideos.movies.Rows.Find(Video.Id) as DsVideos.moviesRow;
                    if (MoviesRow != null)
                    {
                        Video = Video as Movie;
                    }
                    else
                    {
                        DsVideos.episodesRow EpisodeRow = datasetVideos.episodes.Rows.Find(Video.Id) as DsVideos.episodesRow;
                        if (EpisodeRow != null)
                        {
                            Video = Video as Episode;
                        }
                    }
                    videos.Add(Video);
                }
            }
            catch
            { }
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
            DsVideos DatasetVideos = new DsVideos();
            FillDatasetWithAllVideos(DatasetVideos);
            foreach (Video Video in videos)
            {
                if (insertDuplicates || DatasetVideos.videos.Select(DatasetVideos.videos.pathColumn.ColumnName + " = '" + Video.Path + "'").Length == 0)
                {

                    DsVideos.videosRow Row = DatasetVideos.videos.NewvideosRow();
                    Row.path = Video.Path;
                    Row.name = Video.Name;
                    DatasetVideos.videos.AddvideosRow(Row);
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
            tables.Add(datasetVideos.videos.TableName, "SELECT * FROM " + datasetVideos.videos_genres.TableName);
            tables.Add(datasetVideos.movies.TableName, "SELECT * FROM " + datasetVideos.movies.TableName);
            tables.Add(datasetVideos.episodes.TableName, "SELECT * FROM " + datasetVideos.episodes.TableName);
            tables.Add(datasetVideos.videos_genres.TableName, "SELECT * FROM " + datasetVideos.videos_genres.TableName);
            tables.Add(datasetVideos.genres.TableName, "SELECT * FROM " + datasetVideos.genres.TableName);

            Database.FillDataset(datasetVideos, tables);
        }

        #endregion

        public static void InsertVideoHDD(Video video)
        {
            throw new NotImplementedException();
        }

        public static List<String> GetMovieGenres()
        {
            List<String> Genres = new List<string>();
            DbDataReader Reader = Database.GetReader("SELECT gen_label FROM genres");
            while(Reader.Read())
            {
                Genres.Add((string) Reader[0]);
            }
            return Genres;
        } 
    }
}
