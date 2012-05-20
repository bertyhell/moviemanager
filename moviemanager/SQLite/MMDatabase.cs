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
    public delegate void VideosChanged();

    public class MMDatabase
    {
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

                    var MoviesRow = DsVideos.Movies.Rows.Find(Video.Id) as DsVideos.MoviesRow;
                    if (MoviesRow != null)
                    {
                        Video = Video as Movie;
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
                if (insertDuplicates || DatasetVideos.Videos.Select(DatasetVideos.Videos.pathColumn.ColumnName + " = '" + videos[I].Path + "'").Length == 0)
                {

                    DsVideos.VideosRow Row = DatasetVideos.Videos.NewVideosRow();
                    Row.path = videos[I].Path;
                    Row.name = videos[I].Name;
                    DatasetVideos.Videos.AddVideosRow(Row);
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

            int NumberOfVideos = DatasetVideos.Videos.Count;
            int NumberOfEpisodes = DatasetVideos.Episodes.Count;

            //report as other 95% of progress
            var VideosTableAdapter = new VideosTableAdapter();
            for (int I = 0; I < NumberOfVideos; I++)//TODO 001 ping pong compare times with bulk insert
            {
                VideosTableAdapter.Update(DatasetVideos.Videos[I]);
                if (InsertVideosProgress != null)
                    InsertVideosProgress(null, new ProgressEventArgs { MaxNumber = NumberOfVideos, ProgressNumber = PrepareWork + ((I + 1) * NumberOfVideos / (NumberOfVideos + NumberOfEpisodes)) * (100 - PERCENT_PREPARE_WORK) / 100 });//recalculate to number of videos and then to 95%
            }
            var EpisodesTableAdapter = new EpisodesTableAdapter();
            for (int I = 0; I < NumberOfEpisodes; I++)
            {
                EpisodesTableAdapter.Update(DatasetVideos.Episodes[I]);
                if (InsertVideosProgress != null)
                    InsertVideosProgress(null, new ProgressEventArgs { MaxNumber = NumberOfVideos, ProgressNumber = PrepareWork + ((NumberOfVideos + I + 1) * NumberOfVideos / (NumberOfVideos + NumberOfEpisodes)) * (100 - PERCENT_PREPARE_WORK) / 100 });//recalculate to number of series and then to 95%
            }

            //return the duplicates that are not inserted in the table
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
        }

        public static void EmptyTable(String tableName)
        {
            Database.ExecuteSQL("DELETE FROM " + tableName);
        }

        # region fill dataset

        private static void FillDatasetWithAllVideos(DsVideos datasetVideos)
        {
            (new VideosTableAdapter()).Fill(datasetVideos.Videos);
            (new EpisodesTableAdapter()).Fill(datasetVideos.Episodes);
            (new MoviesTableAdapter()).Fill(datasetVideos.Movies);
            (new GenresTableAdapter()).Fill(datasetVideos.Genres);
            (new Videos_genresTableAdapter()).Fill(datasetVideos.Videos_genres);
            (new SeriesTableAdapter()).Fill(datasetVideos.Series);

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
            (new SeriesTableAdapter()).Fill(DsVideos.Series);

            DsVideos.SeriesRow SerieRow = DsVideos.Series.NewSeriesRow();
            SerieRow.name = serie.Name;
            DsVideos.Series.AddSeriesRow(SerieRow);
            serie.Id = (int)SerieRow.id;

            (new SeriesTableAdapter()).Update(DsVideos.Series);
        }

        #endregion
    }

}