using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
using Common;
using Model;
using System.Data.Common;
using SQLite.DsVideosTableAdapters;
using SQLite.dsDatabaseVersionTableAdapters;


namespace SQLite
{
    public class MMDatabase
    {
        private static string _connectionString;

        public static void Init(String connectionString)
        {
            Database.Init(connectionString);
            MMDatabaseCreation.Init(connectionString);
            _connectionString = connectionString;
        }

        public static void SelectAllVideos(IList<Video> videos)
        {
            try
            {
                var DsVideos = new DsVideos();
                FillDatasetWithAllVideos(DsVideos);

                DsVideos.Videos_genresDataTable VideosGenresDataTable = DsVideos.Videos_genres;
                DsVideos.GenresDataTable GenresDataTable = DsVideos.Genres;

                //Convert to ObservableCollection<Video>
                foreach (DsVideos.VideosRow Row in DsVideos.Videos.Rows)
                {
                    var Video = new Video
                    {
                        Id = (uint) Row.id,
                        IdImdb = Row.id_imdb,
                        Name = Row.name,
                        Release = Row.release,
                        Rating = Row.rating,
                        RatingImdb = Row.rating_imdb,
                        Path = Row.path,
                        LastPlayLocation = (ulong)Row.last_play_location
                    };

                    //get genre from dataset
                    foreach (DataRow DataRow in VideosGenresDataTable.Select(VideosGenresDataTable.video_idColumn.ColumnName + " = " + Video.Id))
                    {
                        var GenreId = (int)(long)DataRow[VideosGenresDataTable.genre_idColumn.ColumnName];
                        DataRow GenreRow = GenresDataTable.FindBygen_id(GenreId);
                        Video.Genres.Add((string)GenreRow[GenresDataTable.gen_labelColumn]);
                    }

                    var MoviesRow = DsVideos.Movies.Rows.Find(Video.Id) as DsVideos.MoviesRow;
                    if (MoviesRow != null)
                    {
                        Video = Video.ConvertVideo(VideoTypeEnum.Movie, Video);
                    }
                    else
                    {
                        var EpisodeRow = DsVideos.Episodes.Rows.Find(Video.Id) as DsVideos.EpisodesRow;
                        if (EpisodeRow != null)
                        {
                            Video = Video.ConvertVideo(VideoTypeEnum.Episode, Video);
                        }
                    }
                    videos.Add(Video);
                }
            }
            catch (Exception E) { Console.WriteLine("Exception in GetVideos in MMDatabase: " + E.Message); }
        }

        public static IList<Video> InsertVideosHDD(IList<Video> videos)
        {
            return InsertVideosHDD(videos, false);
        }
        public static void InsertVideosHDDWithDuplicates(ObservableCollection<Video> videos)
        {
            InsertVideosHDD(videos, true);
        }

        public static event OnInsertVideosProgress InsertVideosProgress;

        public delegate void OnInsertVideosProgress(object sender, ProgressEventArgs eventArgs);

        private static IList<Video> InsertVideosHDD(IList<Video> videos, bool insertDuplicates)
        {
            IList<Video> Duplicates = new List<Video>();
            var DatasetVideos = new DsVideos();
            FillDatasetWithAllVideos(DatasetVideos);

            const int PERCENT_PREPARE_WORK = 5;
            int PrepareWork = videos.Count * PERCENT_PREPARE_WORK / 100;

            //report as first 5%
            for (int I = 0; I < videos.Count; I++)
            {
                if (insertDuplicates || DatasetVideos.Videos.Select(DatasetVideos.Videos.pathColumn.ColumnName + " = '" + videos[I].Path + "'").Length == 0)
                {

                    DsVideos.VideosRow Row = DatasetVideos.Videos.NewVideosRow();
                    Row.path = videos[I].Path;
                    Row.name = videos[I].Name;
                    DatasetVideos.Videos.AddVideosRow(Row);
                    videos[I].Id = (uint) Row.id;

                    if (videos[I] is Episode)
                    {
                        InsertEpisodeRow((Episode)videos[I], DatasetVideos);
                    }
                }
                else
                {
                    Duplicates.Add(videos[I]);
                }
                if (InsertVideosProgress != null)
                    InsertVideosProgress(null, new ProgressEventArgs { MaxNumber = videos.Count, ProgressNumber = I * PERCENT_PREPARE_WORK / 100 });//recalculate to 5%
            }

            int NumberOfVideos = DatasetVideos.Videos.Count;
            int NumberOfEpisodes = DatasetVideos.Episodes.Count;

            //report as other 95% of progress
            var VideosTableAdapter = new VideosTableAdapter();
            VideosTableAdapter.Connection = Database.GetConnection();
            for (int I = 0; I < NumberOfVideos; I++)//TODO 001 ping pong compare times with bulk insert
            {
                VideosTableAdapter.Update(DatasetVideos.Videos[I]);
                if (InsertVideosProgress != null)
                    InsertVideosProgress(null, new ProgressEventArgs { MaxNumber = NumberOfVideos, ProgressNumber = PrepareWork + ((I + 1) * NumberOfVideos / (NumberOfVideos + NumberOfEpisodes)) * (100 - PERCENT_PREPARE_WORK) / 100 });//recalculate to number of videos and then to 95%
            }
            var EpisodesTableAdapter = new EpisodesTableAdapter();
            EpisodesTableAdapter.Connection = Database.GetConnection();
            for (int I = 0; I < NumberOfEpisodes; I++)
            {
                EpisodesTableAdapter.Update(DatasetVideos.Episodes[I]);
                if (InsertVideosProgress != null)
                    InsertVideosProgress(null, new ProgressEventArgs { MaxNumber = NumberOfVideos, ProgressNumber = PrepareWork + ((NumberOfVideos + I + 1) * NumberOfVideos / (NumberOfVideos + NumberOfEpisodes)) * (100 - PERCENT_PREPARE_WORK) / 100 });//recalculate to number of series and then to 95%
            }

            //return the duplicates that are not inserted in the table

            OnVideosChanged();
            return Duplicates;
        }

        private static void InsertEpisodeRow(Episode episode, DsVideos dsVideos)
        {
            DsVideos.EpisodesRow EpisodesRow = dsVideos.Episodes.NewEpisodesRow();
            EpisodesRow.id = episode.Id;
            EpisodesRow.serie_id = episode.SerieId;
            EpisodesRow.season = episode.Season;
            EpisodesRow.episode_number = episode.EpisodeNumber;
            dsVideos.Episodes.AddEpisodesRow(EpisodesRow);
        }

        public static void EmptyVideoTables()
        {
            EmptyTable("Videos");
            EmptyTable("Episodes");
            EmptyTable("Movies");
            EmptyTable("Franchises");
            EmptyTable("Series");
            EmptyTable("Videos_genres");
            OnVideosChanged();
        }

        private static void EmptyTable(String tableName)
        {
            Database.ExecuteSQL("DELETE FROM " + tableName);
        }

        # region fill dataset

        private static void FillDatasetWithAllVideos(DsVideos datasetVideos)
        {
            VideosTableAdapter VideosTableAdapter = new VideosTableAdapter();
            VideosTableAdapter.Connection = Database.GetConnection();
            EpisodesTableAdapter EpisodesTableAdapter = new EpisodesTableAdapter();
            EpisodesTableAdapter.Connection = Database.GetConnection();
            MoviesTableAdapter MoviesTableAdapter = new MoviesTableAdapter();
            MoviesTableAdapter.Connection = Database.GetConnection();
            GenresTableAdapter GenresTableAdapter = new GenresTableAdapter();
            GenresTableAdapter.Connection = Database.GetConnection();
            Videos_genresTableAdapter Videos_genresTableAdapter = new Videos_genresTableAdapter();
            Videos_genresTableAdapter.Connection = Database.GetConnection();
            SeriesTableAdapter SeriesTableAdapter = new SeriesTableAdapter();
            SeriesTableAdapter.Connection = Database.GetConnection();

            VideosTableAdapter.Fill(datasetVideos.Videos);
            EpisodesTableAdapter.Fill(datasetVideos.Episodes);
            MoviesTableAdapter.Fill(datasetVideos.Movies);
            GenresTableAdapter.Fill(datasetVideos.Genres);
            Videos_genresTableAdapter.Fill(datasetVideos.Videos_genres);
            SeriesTableAdapter.Fill(datasetVideos.Series);
        }

        #endregion

        /// <summary>
        /// Update a List of Videos, Episodes and Movies
        /// </summary>
        /// <param name="videos"></param>
        /// <returns></returns>
        public static bool UpdateVideos(List<Video> videos)
        {
            bool RetVal = true;

            try
            {
                var DatasetVideos = new DsVideos();
                FillDatasetWithAllVideos(DatasetVideos);

                foreach (Video Video in videos)
                {
                    RetVal &= UpdateVideo(Video, DatasetVideos);
                }
            }
            catch (Exception ex)
            {
                RetVal = false;
            }

            OnVideosChanged();
            return RetVal;
        }

        /// <summary>
        /// Updates one Video, Episode, Movie
        /// </summary>
        /// <param name="video"></param>
        /// <returns></returns>
        public static bool UpdateVideo(Video video)
        {
            bool RetVal = true;

            try
            {
                var DatasetVideos = new DsVideos();
                FillDatasetWithAllVideos(DatasetVideos);
                RetVal &= UpdateVideo(video, DatasetVideos);
            }
            catch (Exception ex)
            {
                RetVal = false;
            }

            return RetVal;
        }

        /// <summary>
        /// Updates one Video, Episode, Movie
        /// </summary>
        /// <param name="video"></param>
        /// <param name="datasetVideos"></param>
        /// <returns></returns>
        public static bool UpdateVideo(Video video, DsVideos datasetVideos)
        {
            if (video == null || datasetVideos == null)
                return false;

            bool RetVal = true;

            try
            {
                //check if row exists
                DsVideos.VideosRow VideosRow = null;
                if (video.Id > 0)
                {
                    VideosRow = datasetVideos.Videos.FindByid(video.Id);
                }

                bool Add = false;
                if (VideosRow == null)
                {
                    //if not exist --> new row
                    VideosRow = datasetVideos.Videos.NewVideosRow();
                    Add = true;
                }

                //set values
                VideosRow.id = video.Id;
                VideosRow.id_imdb = video.IdImdb;
                VideosRow.name = video.Name;
                VideosRow.release = video.Release;
                VideosRow.rating = video.Rating;
                VideosRow.rating_imdb = video.RatingImdb;
                VideosRow.path = video.Path;
                VideosRow.poster = video.Poster;
                VideosRow.last_play_location = (long)video.LastPlayLocation;

                if (Add)
                    datasetVideos.Videos.AddVideosRow(VideosRow);

                VideosTableAdapter VideosTableAdapter = new VideosTableAdapter();
                VideosTableAdapter.Connection = Database.GetConnection();
                RetVal &= (VideosTableAdapter.Update(datasetVideos) > 0);

                //update movie or episode
                if (video.VideoType == VideoTypeEnum.Movie)
                    RetVal &= UpdateMovie((Movie)video, datasetVideos);
                else if (video.VideoType == VideoTypeEnum.Episode)
                    RetVal &= UpdateEpisode((Episode)video, datasetVideos);

            }
            catch (Exception ex)
            {
                RetVal = false;
            }

            return RetVal;
        }

        private static bool UpdateEpisode(Episode episode, DsVideos datasetVideos)
        {
            bool RetVal = true;

            try
            {
                //remove movie record if there is one
                DsVideos.MoviesRow MoviesRow = datasetVideos.Movies.FindByid(episode.Id);
                if (MoviesRow != null)
                {
                    datasetVideos.Movies.RemoveMoviesRow(MoviesRow);
                    MoviesTableAdapter MoviesTableAdapter = new MoviesTableAdapter();
                    MoviesTableAdapter.Connection = Database.GetConnection();
                    RetVal &= (MoviesTableAdapter.Update(datasetVideos) > 0);
                }

                //add episode information
                DsVideos.EpisodesRow EpisodesRow = null;

                if (episode.Id > 0)
                    EpisodesRow = datasetVideos.Episodes.FindByid(episode.Id);

                bool Add = false;
                if (EpisodesRow == null)
                {
                    EpisodesRow = datasetVideos.Episodes.NewEpisodesRow();
                    Add = true;
                }

                EpisodesRow.id = episode.Id;
                EpisodesRow.serie_id = episode.SerieId;
                EpisodesRow.season = episode.Season;
                EpisodesRow.episode_number = episode.EpisodeNumber;

                if (Add)
                    datasetVideos.Episodes.AddEpisodesRow(EpisodesRow);

                //update
                EpisodesTableAdapter EpisodesTableAdapter = new EpisodesTableAdapter();
                EpisodesTableAdapter.Connection = Database.GetConnection();
                RetVal &= (EpisodesTableAdapter.Update(datasetVideos) > 0);
            }
            catch (Exception ex)
            {
                RetVal = false;
            }

            return RetVal;
        }

        private static bool UpdateMovie(Movie movie, DsVideos datasetVideos)
        {
            bool RetVal = true;

            try
            {
                //remove movie record if there is one
                DsVideos.EpisodesRow EpisodeRow = datasetVideos.Episodes.FindByid(movie.Id);
                if (EpisodeRow != null)
                {
                    datasetVideos.Episodes.RemoveEpisodesRow(EpisodeRow);
                    EpisodesTableAdapter EpisodesTableAdapter = new EpisodesTableAdapter();
                    EpisodesTableAdapter.Connection = Database.GetConnection();
                    RetVal &= (EpisodesTableAdapter.Update(datasetVideos) > 0);
                }

                //add episode information
                DsVideos.MoviesRow MoviesRow = null;

                if (movie.Id > 0)
                    MoviesRow = datasetVideos.Movies.FindByid(movie.Id);

                bool Add = false;
                if (MoviesRow == null)
                {
                    MoviesRow = datasetVideos.Movies.NewMoviesRow();
                    Add = true;
                }

                MoviesRow.id = movie.Id;
                MoviesRow.franchise_id = movie.FranchiseID;
                MoviesRow.id_tmdb = movie.IdTmdb;

                if (Add)
                    datasetVideos.Movies.AddMoviesRow(MoviesRow);

                //update
                MoviesTableAdapter MoviesTableAdapter = new MoviesTableAdapter();
                MoviesTableAdapter.Connection = Database.GetConnection();
                RetVal &= (MoviesTableAdapter.Update(datasetVideos) > 0);
            }
            catch (Exception ex)
            {
                RetVal = false;
            }

            return RetVal;
        }

        public static List<String> GetMovieGenres()
        {
            List<String> Genres = new List<string>();
            DbDataReader Reader = Database.GetReader("SELECT gen_label FROM genres");
            while (Reader.Read())
            {
                Genres.Add((string)Reader[0]);
            }
            return Genres;
        }

        #region add series

        public static void AddSerie(Serie serie)
        {
            DsVideos DsVideos = new DsVideos();
            SeriesTableAdapter SeriesTableAdapter = new SeriesTableAdapter();
            SeriesTableAdapter.Connection = Database.GetConnection();
            SeriesTableAdapter.Fill(DsVideos.Series);

            DsVideos.SeriesRow SerieRow = DsVideos.Series.NewSeriesRow();
            SerieRow.name = serie.Name;
            DsVideos.Series.AddSeriesRow(SerieRow);
            serie.Id = (int)SerieRow.id;

            SeriesTableAdapter.Update(DsVideos.Series);
        }

        #endregion


        public delegate void VideosChangedDel();

        public static event VideosChangedDel VideosChanged;

        private static void OnVideosChanged()
        {
            if (VideosChanged != null)
                VideosChanged();
        }
    }

}