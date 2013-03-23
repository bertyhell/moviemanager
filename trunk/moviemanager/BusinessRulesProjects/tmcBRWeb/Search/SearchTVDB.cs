using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using Newtonsoft.Json.Linq;
using Tmc.SystemFrameworks.Common;
using Tmc.SystemFrameworks.Log;
using Tmc.SystemFrameworks.Model;

namespace Tmc.BusinessRules.Web.Search
{
    public class SearchTvdb
    {
        private const string APIKEY = "E5E33524E2E1E724";
        private static readonly List<string> MIRRORS = new List<string>();

        static SearchTvdb()
        {
            //Get configuration from server

            Uri Request = new Uri("http://www.thetvdb.com/api/" + APIKEY + "/mirrors.xml");
            String Response = SimpleWebRequest.DoRequest(Request);

            if (!string.IsNullOrEmpty(Response))
            {
                XmlDocument Doc = new XmlDocument();
                Doc.LoadXml(Response);
                Doc.GetElementsByTagName("mirror");
                foreach (XmlElement MirrorNode in Doc.GetElementsByTagName("mirror"))
                {
                    MIRRORS.Add(MirrorNode.GetElementsByTagName("mirrorpath")[0].Value);
                }
            }
        }

        #region videos

        public static List<Video> GetVideoInfo(String query)
        {
            var Videos = new List<Video>();
            try
            {
                //do request
                var Request = new Uri("http://api.themoviedb.org/3/search/MovieInfo?api_key=" + APIKEY + "&query="
                                    + HttpUtility.UrlEncode(query));
                var Response = SimpleWebRequest.DoJsonRequest(Request);

                if (!string.IsNullOrEmpty(Response))
                {
                    JObject JsonDoc = JObject.Parse(Response);
                    JToken Results = JsonDoc["results"];

                    foreach (JToken JToken in Results)
                    {
                        Videos.Add(new Video
                        {
                            Name = (string)JToken["title"],
                            MovieInfo = new MovieInfo
                                {
                                    IdTmdb = (int)JToken["id"]
                                },
                            Images = new List<ImageInfo> { new ImageInfo { Uri = new Uri(TmdbConfiguration.BaseUrl + TmdbConfiguration.SeletedPosterSize + (string)JToken["poster_path"]) } },
                            Release = ParseTmdbDate((string)JToken["release_date"])
                        });
                    }
                }
            }

            catch (Exception Ex)
            {
                GlobalLogger.Instance.MovieManagerLogger.Error(GlobalLogger.FormatExceptionForLog("SearchTvdb", "GetVideoInfo", Ex));
            }

            return Videos;
        }

        public static void GetMovieImages(Video movie)
        {
            Uri Request = new Uri("http://api.themoviedb.org/3/MovieInfo/" + movie.MovieInfo.IdTmdb + "/images?api_key=" + APIKEY);
            String Response = SimpleWebRequest.DoJsonRequest(Request);

            if (!string.IsNullOrEmpty(Response))
            {
                JObject JSonImages = JObject.Parse(Response);
                JSonImages["posters"].ToList().ForEach(g => movie.Images.Add(
                    new ImageInfo { Uri = new Uri(TmdbConfiguration.BaseUrl + TmdbConfiguration.SeletedPosterSize + g["file_path"]) })
                );
                JSonImages["backdrops"].ToList().ForEach(g => movie.Images.Add(
                    new ImageInfo { Uri = new Uri(TmdbConfiguration.BaseUrl + TmdbConfiguration.SeletedBackdropSize + g["file_path"]) })
                );
            }
        }

        public static void GetExtraMovieInfo(Video movie)
        {
            Uri Request = new Uri("http://api.themoviedb.org/3/MovieInfo/" + movie.MovieInfo.IdTmdb + "?api_key=" + APIKEY);
            String Response = SimpleWebRequest.DoJsonRequest(Request);

            if (!string.IsNullOrEmpty(Response))
            {
                JObject JsonMovie = JObject.Parse(Response);
                List<string> Genres = new List<string>();
                JsonMovie["genres"].ToList().ForEach(g => Genres.Add((string)g["name"])); // get genres
                //TODO 090: add genres to movie
                //movie.Genres = Genres;
                movie.IdImdb = (string)JsonMovie["imdb_id"];
                movie.Plot = (string)JsonMovie["overview"];
                movie.Runtime = (int)JsonMovie["runtime"];
                GetMovieImages(movie);
            }
        }

        #endregion

        #region actor

        public static List<Actor> SearchActor(String query)
        {
            List<Actor> Actors = new List<Actor>();
            try
            {

                //do request
                Uri Request = new Uri("http://api.themoviedb.org/3/search/person?api_key=" + APIKEY + "&query="
                                    + HttpUtility.UrlEncode(query));

                String Response = SimpleWebRequest.DoJsonRequest(Request);

                if (!string.IsNullOrEmpty(Response))
                {
                    //Console.WriteLine(response);

                    JObject JsonDoc = JObject.Parse(Response);
                    //XDocument xmlDoc = XDocument.Parse(response);

                    //get elements from json

                    JArray Results = (JArray)JsonDoc["results"];

                    foreach (JToken JActor in Results)
                    {
                        Actor LocalActor = new Actor { TmdbId = (int)JActor["id"], Name = (string)JActor["name"] };
                        LocalActor.Images.Add(new ImageInfo
                        {
                            Uri = new Uri(TmdbConfiguration.BaseUrl + TmdbConfiguration.SeletedPosterSize +
                                (string)JActor["profile_path"])
                        });
                        Actors.Add(LocalActor);
                    }
                }
            }
            catch (Exception Ex)
            {
                GlobalLogger.Instance.MovieManagerLogger.Error(GlobalLogger.FormatExceptionForLog("SearchTvdb", "SearchActor", Ex));
            }

            return Actors;
        }


        public static void GetActorInfo(Actor actor)
        {
            try
            {
                //do request
                Uri Request = new Uri("http://api.themoviedb.org/3/person/" + actor.TmdbId + "?api_key=" + APIKEY);
                String Response = SimpleWebRequest.DoJsonRequest(Request);

                if (!string.IsNullOrEmpty(Response))
                {
                    //Console.WriteLine(response);

                    JObject JsonDoc = JObject.Parse(Response);
                    //get elements from json
                    actor.Biography = (string)JsonDoc["biography"];
                    actor.Birthplace = (string)JsonDoc["place_of_birth"];
                    actor.Birthday = ParseTmdbDate((string)JsonDoc["birthday"]);
                    GetMovieListFromActor(actor);
                    //TODO 010 add birthday, and more ...
                }
            }
            catch (Exception Ex)
            {
                GlobalLogger.Instance.MovieManagerLogger.Error(GlobalLogger.FormatExceptionForLog("SearchTvdb", "GetActorInfo", Ex));
            }
        }

        public static void GetMovieListFromActor(Actor actor)
        {
            try
            {
                //do request
                Uri Request = new Uri("http://api.themoviedb.org/3/person/" + actor.TmdbId + "/credits?api_key=" + APIKEY);
                String Response = SimpleWebRequest.DoJsonRequest(Request);

                if (!string.IsNullOrEmpty(Response))
                {
                    //Console.WriteLine(response);

                    JObject JsonDoc = JObject.Parse(Response);
                    //get elements from json
                    JArray JMovies = (JArray)JsonDoc["cast"];
                    foreach (JToken JMovie in JMovies)
                    {
                        actor.MovieImageUrls.Add(new ImageInfo
                                                 {
                                                     Uri = new Uri(TmdbConfiguration.BaseUrl + TmdbConfiguration.PosterSizes[TmdbConfiguration.PosterSizes.Count - 1] + (string)JMovie["poster_path"]),
                                                     Name = (string)JMovie["original_title"],
                                                     Tag = JMovie["id"].ToString(),
                                                     Type = typeof(MovieInfo)
                                                 });
                    }

                    //TODO 010 add birthday, and more ...
                }
            }
            catch (Exception Ex)
            {
                GlobalLogger.Instance.MovieManagerLogger.Error(GlobalLogger.FormatExceptionForLog("SearchTvdb", "GetMovieListFromActor", Ex));
            }
        }


        #endregion

        private static DateTime ParseTmdbDate(String stringDate)
        {
            return DateTimeUtilities.ParseDate(stringDate, "yyyy-MM-dd");
        }
    }
}
