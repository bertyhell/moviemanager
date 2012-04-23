using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Model;
using System.Xml.Linq;
using System.Web;
using Newtonsoft.Json.Linq;

namespace MovieManager.BL.Search
{
    public class SearchTMDB
    {
        private const string APIKEY = "02004323eee9878ce511ca57faf0b29c";
        
        //TODO 100: search movie info via API v3
        static SearchTMDB()
        {
            //Get configuration from server
            Uri Request = new Uri("http://api.themoviedb.org/3/configuration?api_key=" + APIKEY);
            String Response = SimpleWebRequest.DoJSONRequest(Request);

            if (!string.IsNullOrEmpty(Response))
            {
                JObject JsonDoc = JObject.Parse(Response);
                JToken Results = JsonDoc["images"];
                TMDBConfiguration.BaseUrl = (string)Results["base_url"];
                foreach (JToken BackdropSizes in Results["backdrop_sizes"])
                {
                    TMDBConfiguration.BackdropSizes.Add((string)BackdropSizes);
                }
                foreach (JToken PosterSizes in Results["poster_sizes"])
                {
                    TMDBConfiguration.PosterSizes.Add((string)PosterSizes);
                }
                foreach (JToken ProfileSizes in Results["profile_sizes"])
                {
                    TMDBConfiguration.ProfileSizes.Add((string)ProfileSizes);
                }
            }
        }

        #region videos

        public static List<Movie> GetVideoInfo(String query)
        {
            var Videos = new List<Movie>();
            try
            {
                //do request
                var Request = new Uri("http://api.themoviedb.org/3/search/movie?api_key=" + APIKEY + "&query="
                                    + HttpUtility.UrlEncode(query));
                var Response = SimpleWebRequest.DoJSONRequest(Request);

                if (!string.IsNullOrEmpty(Response))
                {
                    JObject JsonDoc = JObject.Parse(Response);
                    JToken Results = JsonDoc["results"];
                    Videos.AddRange(Results.Select(jToken => new Movie {Name = (string) jToken["title"]}));
                }
            }

            catch
            {
            }

            return Videos;
        }



        private static void GetImages(IEnumerable<XNode> images, Movie movie)
        {
            bool FirstImage = true;
            foreach (XNode ImageNode in images)
            {
                XElement ImageEl = ImageNode as XElement;
                if (ImageEl != null)
                {
                    var XAttribute = ImageEl.Attribute("size");
                    if (XAttribute != null && (XAttribute.Value == "original"))
                    {
                        var Attribute = ImageEl.Attribute("url");
                        if (Attribute != null)
                        {
                            string Url = Attribute.Value;
                            if (!string.IsNullOrEmpty(Url))
                            {
                                if (FirstImage)
                                {
                                    movie.Poster = new Uri(Url);
                                    FirstImage = false;
                                }
                                else
                                {
                                    movie.Images.Add(new ImageInfo { Uri = new Uri(Url), Type = typeof(Movie) });
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void GetExtraMovieInfo(int tmdbId, Movie movie)
        {
            Uri Request = new Uri("http://api.themoviedb.org/2.1/Movie.getInfo/en/xml/02004323eee9878ce511ca57faf0b29c/" + tmdbId);
            String Response = SimpleWebRequest.DoRequest(Request);

            if (!string.IsNullOrEmpty(Response))
            {
                //Console.WriteLine(Response);

                var XMLDoc = XDocument.Parse(Response);

                //get elements from xml
                var LocalMovies = from MovieEl in XMLDoc.Descendants("movie")
                                  let XElement = MovieEl.Element("overview")
                                  where XElement != null
                                  let Element = MovieEl.Element("name")
                                  where Element != null
                                  let XElement1 = MovieEl.Element("categories")
                                  where XElement1 != null
                                  let Element1 = MovieEl.Element("images")
                                  where Element1 != null
                                  let XElement2 = MovieEl.Element("cast")
                                  where XElement2 != null
                                  select new
                                  {
                                      Plot = XElement.Value,
                                      Name = Element.Value,
                                      Genres = XElement1.Nodes().ToList(),
                                      Images = Element1.Nodes().ToList(),
                                      Cast = XElement2.Nodes().ToList()
                                  };

                var MovieVar = LocalMovies.ToList()[0];
                GetImages(MovieVar.Images, movie);
                movie.Name = MovieVar.Name;
                movie.Plot = MovieVar.Plot;

                movie.Genres.Clear();
                foreach (XNode Genre in MovieVar.Genres)
                {
                    XElement CategoryElement = Genre as XElement;
                    if (CategoryElement != null)
                    {
                        var XAttribute = CategoryElement.Attribute("name");
                        if (XAttribute != null)
                            movie.Genres.Add(XAttribute.Value);
                    }
                }
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

                String Response = SimpleWebRequest.DoJSONRequest(Request);

                if (!string.IsNullOrEmpty(Response))
                {
                    //Console.WriteLine(response);

                    JObject JsonDoc = JObject.Parse(Response);
                    //XDocument xmlDoc = XDocument.Parse(response);

                    //get elements from json

                    JArray Results = (JArray)JsonDoc["results"];

                    foreach (JToken JActor in Results)
                    {
                        Actor LocalActor = new Actor { TmdbID = (int)JActor["id"], Name = (string)JActor["name"] };
                        LocalActor.ImageUrls.Add(TMDBConfiguration.BaseUrl + TMDBConfiguration.ProfileSizes[TMDBConfiguration.ProfileSizes.Count - 1] +
                                                                (string)JActor["profile_path"]);
                        Actors.Add(LocalActor);
                    }
                }
            }
            catch (Exception)
            { }

            return Actors;
        }


        public static void GetActorInfo(Actor actor)
        {
            try
            {
                //do request
                Uri Request = new Uri("http://api.themoviedb.org/3/person/" + actor.TmdbID + "?api_key=" + APIKEY);
                String Response = SimpleWebRequest.DoJSONRequest(Request);

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
            catch { }
        }

        public static void GetMovieListFromActor(Actor actor)
        {
            try
            {
                //do request
                Uri Request = new Uri("http://api.themoviedb.org/3/person/" + actor.TmdbID + "/credits?api_key=" + APIKEY);
                String Response = SimpleWebRequest.DoJSONRequest(Request);

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
                                                     Uri = new Uri(TMDBConfiguration.BaseUrl + TMDBConfiguration.PosterSizes[TMDBConfiguration.PosterSizes.Count - 1] + (string)JMovie["poster_path"]),
                                                     Name = (string)JMovie["original_title"],
                                                     Tag = JMovie["id"].ToString(),
                                                     Type = typeof(Movie)
                                                 });
                    }

                    //TODO 010 add birthday, and more ...
                }
            }
            catch { }
        }


        #endregion

        private static DateTime ParseTmdbDate(String stringDate)
        {
            return DateTimeUtilities.ParseDate(stringDate, "yyyy-MM-dd");
        }
    }
}
