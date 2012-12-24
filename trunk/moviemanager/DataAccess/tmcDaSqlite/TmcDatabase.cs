using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlServerCe;
using Model;
using System.Data.Common;
using Tmc.DataAccess.Sqlite.DsVideosTableAdapters;
using Tmc.SystemFrameworks.Common;

namespace Tmc.DataAccess.Sqlite
{
    public class TmcDatabase
    {
        public static void Init(String connectionString)
        {
            Database.Init(connectionString);
            TmcDatabaseCreation.Init(connectionString);
        }

        public static IList<Video> SelectAllVideos()
        {
            var Videos = new List<Video>();

            var DsVideos = new DsVideos();
            FillDatasetWithAllVideos(DsVideos);

            DsVideos.Videos_genresDataTable VideosGenresDataTable = DsVideos.Videos_genres;
            DsVideos.GenresDataTable GenresDataTable = DsVideos.Genres;

            //Convert to ObservableCollection<Video>
            foreach (DsVideos.VideosRow Row in DsVideos.Videos.Rows)
            {
                var Video = new Video
                {
                    Id = (uint)Row.id,
                    IdImdb = Row.id_imdb,
                    Name = Row.name,
                    Release = Row.release,
                    Rating = Row.rating,
                    RatingImdb = Row.rating_imdb,
                    Path = Row.path,
                    LastPlayLocation = (ulong)Row.last_play_location
                };
                if (!String.IsNullOrEmpty(Row.poster))
                    Video.Poster = new ImageInfo { Uri = new Uri(Row.poster) };

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
                Videos.Add(Video);
            }
            return Videos;
        }

        public List<Serie> GetAllSeries()
        {
            List<Serie> RetVal = new List<Serie>();
            DsVideos VideosDs = new DsVideos();
            FillDatasetWithAllSeries(VideosDs);

            DsVideos.SeriesDataTable SeriesTable = VideosDs.Series;

            foreach (DsVideos.SeriesRow SeriesRow in SeriesTable)
            {
                Serie Serie = new Serie
                    {
                        Id = SeriesRow.id,
                        Name = SeriesRow.name
                    };
                RetVal.Add(Serie);
            }

            return RetVal;
        }

        public static IList<Video> InsertVideosHdd(IList<Video> videos)
        {
            return InsertVideosHdd(videos, false);
        }
        public static void InsertVideosHddWithDuplicates(ObservableCollection<Video> videos)
        {
            InsertVideosHdd(videos, true);
        }

        public static event OnInsertVideosProgress InsertVideosProgress;

        public delegate void OnInsertVideosProgress(object sender, ProgressEventArgs eventArgs);

        private static IList<Video> InsertVideosHdd(IList<Video> videos, bool insertDuplicates)
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
                    videos[I].Id = (uint)Row.id;

                    Episode Episode = videos[I] as Episode;
                    if (Episode != null)
                    {
                        InsertEpisodeRow(Episode, DatasetVideos);
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
            var VideosTableAdapter = new VideosTableAdapter { Connection = Database.GetConnection() };
            for (int I = 0; I < NumberOfVideos; I++)//TODO 001 ping pong compare times with bulk insert
            {
                VideosTableAdapter.Update(DatasetVideos.Videos[I]);
                if (InsertVideosProgress != null)
                    InsertVideosProgress(null, new ProgressEventArgs { MaxNumber = NumberOfVideos, ProgressNumber = PrepareWork + ((I + 1) * NumberOfVideos / (NumberOfVideos + NumberOfEpisodes)) * (100 - PERCENT_PREPARE_WORK) / 100 });//recalculate to number of videos and then to 95%
            }
            var EpisodesTableAdapter = new EpisodesTableAdapter { Connection = Database.GetConnection() };
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
            EpisodesRow.id = (int)episode.Id;
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
            Database.ExecuteSql("DELETE FROM " + tableName);
        }

        # region fill dataset

        private static void FillDatasetWithAllVideos(DsVideos datasetVideos)
        {
            VideosTableAdapter VideosTableAdapter = new VideosTableAdapter { Connection = Database.GetConnection() };
            EpisodesTableAdapter EpisodesTableAdapter = new EpisodesTableAdapter { Connection = Database.GetConnection() };
            MoviesTableAdapter MoviesTableAdapter = new MoviesTableAdapter { Connection = Database.GetConnection() };
            GenresTableAdapter GenresTableAdapter = new GenresTableAdapter { Connection = Database.GetConnection() };
            Videos_genresTableAdapter VideosGenresTableAdapter = new Videos_genresTableAdapter { Connection = Database.GetConnection() };
            SeriesTableAdapter SeriesTableAdapter = new SeriesTableAdapter { Connection = Database.GetConnection() };

            VideosTableAdapter.Fill(datasetVideos.Videos);
            EpisodesTableAdapter.Fill(datasetVideos.Episodes);
            MoviesTableAdapter.Fill(datasetVideos.Movies);
            GenresTableAdapter.Fill(datasetVideos.Genres);
            VideosGenresTableAdapter.Fill(datasetVideos.Videos_genres);
            SeriesTableAdapter.Fill(datasetVideos.Series);
        }

        private static void FillDatasetWithAllSeries(DsVideos datasetVideos)
        {
            SeriesTableAdapter SeriesTableAdapter = new SeriesTableAdapter { Connection = Database.GetConnection() };
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
                    foreach (string Genre in Video.Genres)
                    {
                        InsertVideoGenre(Video.Id, Genre);
                    }
                }
            }
            catch
            {
                RetVal = false;
            }

            OnVideosChanged();
            return RetVal;
        }

        public static void InsertVideoGenre(uint videoId, string genreLabel)
        {
            SqlCeDataReader Reader = null;
            try
            {
                Reader = Database.GetReader("SELECT gen_id from Genres where gen_label = ?", new SqlCeParameter("gen_label", genreLabel));
                if (!Reader.Read()) //TODO 001 should be only 0 or 1 results --> maybe add an assure test?
                {
                    Reader.Close();
                    Database.ExecuteSql("INSERT INTO Genres (gen_label) VALUES (?)", new SqlCeParameter("gen_label", genreLabel));
                }
                //genre already in database --> add link between video and genre
                int GenreId = Reader.GetInt32(0);
                Reader = Database.GetReader("SELECT 1 from Videos_genres where gen_id = ? && video_id = ?", new SqlCeParameter("gen_id", GenreId), new SqlCeParameter("video_id", videoId));
                if (!Reader.Read()) //link between video and genre not yet in database
                {
                    Reader.Close();
                    Database.ExecuteSql("INSERT INTO Videos_genres (video_id, gen_id) VALUES (?,?)", new SqlCeParameter("video_id", videoId), new SqlCeParameter("gen_id", GenreId));
                }
            }
            finally
            {
                if (Reader != null && !Reader.IsClosed) Reader.Close();
            }
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
            catch
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
                    VideosRow = datasetVideos.Videos.FindByid((int)video.Id);
                }

                bool Add = false;
                if (VideosRow == null)
                {
                    //if not exist --> new row
                    VideosRow = datasetVideos.Videos.NewVideosRow();
                    Add = true;
                }

                //set values
                //VideosRow.id = (int)video.Id;
                VideosRow.id_imdb = video.IdImdb;
                VideosRow.name = video.Name;
                VideosRow.release = video.Release;
                VideosRow.rating = (float)video.Rating;
                VideosRow.rating_imdb = (float)video.RatingImdb;
                VideosRow.path = video.Path;
                VideosRow.poster = video.Poster.Uri.AbsoluteUri;
                VideosRow.last_play_location = (int)video.LastPlayLocation;

                if (Add)
                    datasetVideos.Videos.AddVideosRow(VideosRow);

                VideosTableAdapter VideosTableAdapter = new VideosTableAdapter { Connection = Database.GetConnection() };
                RetVal &= (VideosTableAdapter.Update(datasetVideos) > 0);

                //update movie or episode
                if (video.VideoType == VideoTypeEnum.Movie)
                    RetVal &= UpdateMovie((Movie)video, datasetVideos);
                else if (video.VideoType == VideoTypeEnum.Episode)
                    RetVal &= UpdateEpisode((Episode)video, datasetVideos);

            }
            catch
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
                DsVideos.MoviesRow MoviesRow = datasetVideos.Movies.FindByid((int)episode.Id);
                if (MoviesRow != null)
                {
                    datasetVideos.Movies.RemoveMoviesRow(MoviesRow);
                    MoviesTableAdapter MoviesTableAdapter = new MoviesTableAdapter { Connection = Database.GetConnection() };
                    RetVal &= (MoviesTableAdapter.Update(datasetVideos) > 0);
                }

                //add episode information
                DsVideos.EpisodesRow EpisodesRow = null;

                if (episode.Id > 0)
                    EpisodesRow = datasetVideos.Episodes.FindByid((int)episode.Id);

                bool Add = false;
                if (EpisodesRow == null)
                {
                    EpisodesRow = datasetVideos.Episodes.NewEpisodesRow();
                    Add = true;
                }

                EpisodesRow.id = (int)episode.Id;
                EpisodesRow.serie_id = episode.SerieId;
                EpisodesRow.season = episode.Season;
                EpisodesRow.episode_number = episode.EpisodeNumber;

                if (Add)
                    datasetVideos.Episodes.AddEpisodesRow(EpisodesRow);

                //update
                EpisodesTableAdapter EpisodesTableAdapter = new EpisodesTableAdapter { Connection = Database.GetConnection() };
                RetVal &= (EpisodesTableAdapter.Update(datasetVideos) > 0);
            }
            catch
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
                DsVideos.EpisodesRow EpisodeRow = datasetVideos.Episodes.FindByid((int)movie.Id);
                if (EpisodeRow != null)
                {
                    datasetVideos.Episodes.RemoveEpisodesRow(EpisodeRow);
                    EpisodesTableAdapter EpisodesTableAdapter = new EpisodesTableAdapter { Connection = Database.GetConnection() };
                    RetVal &= (EpisodesTableAdapter.Update(datasetVideos) > 0);
                }

                //add episode information
                DsVideos.MoviesRow MoviesRow = null;

                if (movie.Id > 0)
                    MoviesRow = datasetVideos.Movies.FindByid((int)movie.Id);

                bool Add = false;
                if (MoviesRow == null)
                {
                    MoviesRow = datasetVideos.Movies.NewMoviesRow();
                    Add = true;
                }

                MoviesRow.id = (int)movie.Id;
                MoviesRow.franchise_id = movie.FranchiseId;
                MoviesRow.id_tmdb = movie.IdTmdb;

                if (Add)
                    datasetVideos.Movies.AddMoviesRow(MoviesRow);

                //update
                MoviesTableAdapter MoviesTableAdapter = new MoviesTableAdapter { Connection = Database.GetConnection() };
                RetVal &= (MoviesTableAdapter.Update(datasetVideos) > 0);
            }
            catch
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
                Genres.Add(Reader.GetString(0));
            }
            return Genres;
        }

        #region add series

        public static void AddSerie(Serie serie)
        {
            DsVideos DsVideos = new DsVideos();
            SeriesTableAdapter SeriesTableAdapter = new SeriesTableAdapter { Connection = Database.GetConnection() };
            SeriesTableAdapter.Fill(DsVideos.Series);

            DsVideos.SeriesRow SerieRow = DsVideos.Series.NewSeriesRow();
            SerieRow.name = serie.Name;
            DsVideos.Series.AddSeriesRow(SerieRow);
            serie.Id = SerieRow.id;

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