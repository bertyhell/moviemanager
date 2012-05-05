using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using Common;
using Model;
using System.Data.Common;
using SQLite.DsVideosTableAdapters;


namespace SQLite
{
    public delegate void VideosChanged();

    public class MMDatabase
    {
        //public static event VideosChanged OnVideosChanged;
        //don't use videos changed -> all database operations could be run in different thread -> different trhead has no access to observable collection
        //update lists in maincontroller in commandobjects

        public static void SelectAllVideos(ObservableCollection<Video> videos)
        {
            try
            {
                var DsVideos = new DsVideos();
                FillDatasetWithAllVideos(DsVideos);

                DsVideos.videos_genresDataTable VideosGenresDataTable = DsVideos.videos_genres;
                DsVideos.genresDataTable GenresDataTable = DsVideos.genres;

                //Convert to ObservableCollection<Video>
                foreach (DsVideos.videosRow Row in DsVideos.videos.Rows)
                {
                    var Video = new Video
                    {
                        Id = (int)Row.id,
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
                        var GenreID = (int)(long)DataRow[VideosGenresDataTable.genre_idColumn.ColumnName];
                        DataRow GenreRow = GenresDataTable.FindBygen_id(GenreID);
                        Video.Genres.Add((string)GenreRow[GenresDataTable.gen_labelColumn]);
                    }

                    var MoviesRow = DsVideos.movies.Rows.Find(Video.Id) as DsVideos.moviesRow;
                    if (MoviesRow != null)
                    {
                        Video = Video as Movie;
                    }
                    else
                    {
                        var EpisodeRow = DsVideos.episodes.Rows.Find(Video.Id) as DsVideos.episodesRow;
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

        public static DbDataReader GetVideosDataReader()
        {
            return Database.GetReader("select * from videos");
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
                if (insertDuplicates || DatasetVideos.videos.Select(DatasetVideos.videos.pathColumn.ColumnName + " = '" + videos[I].Path + "'").Length == 0)
                {

                    DsVideos.videosRow Row = DatasetVideos.videos.NewvideosRow();
                    Row.path = videos[I].Path;
                    Row.name = videos[I].Name;
                    DatasetVideos.videos.AddvideosRow(Row);
                    videos[I].Id = (int)Row.id;

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

            int NumberOfVideos = DatasetVideos.videos.Count;
            int NumberOfEpisodes = DatasetVideos.episodes.Count;

            //report as other 95% of progress
            var VideosTableAdapter = new videosTableAdapter();
            for (int I = 0; I < NumberOfVideos; I++)//TODO 001 ping pong compare times with bulk insert
            {
                VideosTableAdapter.Update(DatasetVideos.videos[I]);
                if (InsertVideosProgress != null)
                    InsertVideosProgress(null, new ProgressEventArgs { MaxNumber = NumberOfVideos, ProgressNumber = PrepareWork + ((I + 1) * NumberOfVideos / (NumberOfVideos + NumberOfEpisodes)) * (100 - PERCENT_PREPARE_WORK) / 100 });//recalculate to number of videos and then to 95%
            }
            var EpisodesTableAdapter = new episodesTableAdapter();
            for (int I = 0; I < NumberOfEpisodes; I++)
            {
                EpisodesTableAdapter.Update(DatasetVideos.episodes[I]);
                if (InsertVideosProgress != null)
                    InsertVideosProgress(null, new ProgressEventArgs { MaxNumber = NumberOfVideos, ProgressNumber = PrepareWork + ((NumberOfVideos + I + 1) * NumberOfVideos / (NumberOfVideos + NumberOfEpisodes)) * (100 - PERCENT_PREPARE_WORK) / 100 });//recalculate to number of series and then to 95%
            }

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
            (new serieTableAdapter()).Fill(DsVideos.serie);

            DsVideos.serieRow SerieRow = DsVideos.serie.NewserieRow();
            SerieRow.name = serie.Name;
            DsVideos.serie.AddserieRow(SerieRow);
            serie.Id = (int)SerieRow.id;

            (new serieTableAdapter()).Update(DsVideos.serie);
        }

        #endregion
    }

}