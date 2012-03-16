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
                DsVideos DsVideos = new DsVideos();
                FillDatasetWithAllVideos(DsVideos);

                DsVideos.videos_genresDataTable VideosGenresDataTable = DsVideos.videos_genres;
                DsVideos.genresDataTable GenresDataTable = DsVideos.genres;

                //Convert to ObservableCollection<Video>
                foreach (DsVideos.videosRow Row in DsVideos.videos.Rows)
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
                        int GenreID = (int)(long)DataRow[VideosGenresDataTable.genre_idColumn.ColumnName];
                        DataRow GenreRow = GenresDataTable.FindBygen_id(GenreID);
                        Video.Genres.Add((string)GenreRow[GenresDataTable.gen_labelColumn]);
                    }

                    DsVideos.moviesRow MoviesRow = DsVideos.movies.Rows.Find(Video.Id) as DsVideos.moviesRow;
                    if (MoviesRow != null)
                    {
                        Video = Video as Movie;
                    }
                    else
                    {
                        DsVideos.episodesRow EpisodeRow = DsVideos.episodes.Rows.Find(Video.Id) as DsVideos.episodesRow;
                        if (EpisodeRow != null)
                        {
                            Video = Video.ConvertVideo(VideoTypeEnum.Episode, Video);
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
            return InsertVideosHDD(videos, false);
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
                    Video.Id = (int) Row.id;

                    if( Video is Episode)
                    {
                        InsertEpisodeRow((Episode)Video, DatasetVideos);
                    }
                }
                else
                {
                    Duplicates.Add(Video);
                }
            }

            (new videosTableAdapter()).Update(DatasetVideos.videos);
            (new episodesTableAdapter()).Update(DatasetVideos.episodes);

            if (OnVideosChanged != null)
                OnVideosChanged();

            //return the duplicates that are not inserted in the table
            return Duplicates;
        }

        private static void InsertEpisodeRow(Episode episode, DsVideos dsVideos)
        {
            DsVideos.episodesRow EpisodesRow = dsVideos.episodes.NewepisodesRow();
            EpisodesRow.id = episode.Id;
            EpisodesRow.serie_id = episode.SerieId;
            EpisodesRow.season = episode.Season;
            EpisodesRow.episode_number = episode.EpisodeNumber;
            dsVideos.episodes.AddepisodesRow(EpisodesRow);
        }

        public static void EmptyVideoTables()
        {
            EmptyTable("videos");
            EmptyTable("episodes");
            EmptyTable("movies");
            EmptyTable("franchises");
            EmptyTable("serie");
            EmptyTable("videos_genres");

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
            (new episodesTableAdapter()).Fill(datasetVideos.episodes);
            (new moviesTableAdapter()).Fill(datasetVideos.movies);
            (new genresTableAdapter()).Fill(datasetVideos.genres);
            (new videos_genresTableAdapter()).Fill(datasetVideos.videos_genres);
            (new serieTableAdapter()).Fill(datasetVideos.serie);

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
            while (reader.Read())
            {
                genres.Add((string)reader[0]);
            }
            return genres;
        }

        #region add series

        public static void AddSerie(Serie serie)
        {
            DsVideos _dsVideos = new DsVideos();
            (new serieTableAdapter()).Fill(_dsVideos.serie);

            DsVideos.serieRow SerieRow = _dsVideos.serie.NewserieRow();
            SerieRow.name = serie.Name;
            _dsVideos.serie.AddserieRow(SerieRow);
            serie.Id = (int)SerieRow.id;

            (new serieTableAdapter()).Update(_dsVideos.serie);
        }

        #endregion
    }
}
