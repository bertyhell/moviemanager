using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using Model;
using System.Data.SQLite;
using System.Globalization;
using System.Data.Common;
using SQLite.DsVideosTableAdapters;


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
                DsVideos DatasetVideos = new DsVideos();
                FillDatasetWithAllVideos(DatasetVideos);

                DsVideos.videos_genresDataTable VideosGenresDataTable = DatasetVideos.videos_genres;
                DsVideos.genresDataTable GenresDataTable = DatasetVideos.genres;

                //Convert to ObservableCollection<Video>
                foreach (DsVideos.videosRow Row in DatasetVideos.videos.Rows)
                {
                    Video Video = new Video
                                      {
                                          Id = (int)Row.id,
                                          IdImdb = Row.id_imdb,
                                          Name = Row.name,
                                          Release = Row.release,
                                          Rating = Row.rating,
                                          RatingImdb = Row.rating_imdb,
                                          Path = Row.path,
                                          LastPlayLocation = (int)Row.last_play_location
                                      };

                    //get genre from dataset
                    foreach (DataRow DataRow in VideosGenresDataTable.Select(VideosGenresDataTable.video_idColumn.ColumnName + " = " + Video.Id))
                    {
                        int GenreID =  (int)(long)DataRow[VideosGenresDataTable.genre_idColumn.ColumnName];
                        DataRow GenreRow = GenresDataTable.FindBygen_id(GenreID);
                        Video.Genres.Add((string)GenreRow[GenresDataTable.gen_labelColumn]);
                    }

                    DsVideos.moviesRow MoviesRow = DatasetVideos.movies.Rows.Find(Video.Id) as DsVideos.moviesRow;
                    if (MoviesRow != null)
                    {
                        Video = Video as Movie;
                    }
                    else
                    {
                        DsVideos.episodesRow EpisodeRow = DatasetVideos.episodes.Rows.Find(Video.Id) as DsVideos.episodesRow;
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

            (new videosTableAdapter()).Update(DatasetVideos);
            //SQLiteCommand SqLiteCommand = new SqlCommand("insert into ", connection);
            //DatasetVideos..Update(DatasetVideos, "videos");

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
            (new videosTableAdapter()).Fill(datasetVideos.videos);
            (new genresTableAdapter()).Fill(datasetVideos.genres);
            (new videos_genresTableAdapter()).Fill(datasetVideos.videos_genres);

            //Dictionary<String, String> tables = new Dictionary<String, String>();
            //tables.Add(datasetVideos.videos.TableName, "SELECT * FROM " + datasetVideos.videos_genres.TableName);
            //tables.Add(datasetVideos.movies.TableName, "SELECT * FROM " + datasetVideos.movies.TableName);
            //tables.Add(datasetVideos.episodes.TableName, "SELECT * FROM " + datasetVideos.episodes.TableName);
            //tables.Add(datasetVideos.videos_genres.TableName, "SELECT * FROM " + datasetVideos.videos_genres.TableName);
            //tables.Add(datasetVideos.genres.TableName, "SELECT * FROM " + datasetVideos.genres.TableName);

            //Database.FillDataset(datasetVideos, tables);
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
