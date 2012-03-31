﻿using System;
using System.Collections.Generic;
using System.Globalization;
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
            List<Movie> videos = new List<Movie>();
            try
            {
                //do request
                Uri request = new Uri("http://api.themoviedb.org/3/search/movie?api_key=" + APIKEY + "&query="
                                    + HttpUtility.UrlEncode(query));
                String Response = SimpleWebRequest.DoJSONRequest(request);

                if (!string.IsNullOrEmpty(Response))
                {
                    JObject JsonDoc = JObject.Parse(Response);
                    JToken Results = JsonDoc["results"];
                    foreach (JToken JToken in Results)
                    {
                        Movie Mov = new Movie() {Name = (string)JToken["title"]};
                        videos.Add(Mov);
                    }
                }
            }

            catch
            {
            }

            return videos;
        }



        private static void GetImages(IEnumerable<XNode> images, Movie movie)
        {
            bool firstImage = true;
            foreach (XNode imageNode in images)
            {
                XElement imageEl = imageNode as XElement;
                if (imageEl != null && imageEl.Attribute("size").Value == "original")
                {
                    string url = imageEl.Attribute("url").Value;
                    if (!string.IsNullOrEmpty(url))
                    {
                        if (firstImage)
                        {
                            movie.Poster = new Uri(url);
                            firstImage = false;
                        }
                        else
                        {
                            movie.Images.Add(new ImageInfo { Uri = new Uri(url), Type = typeof(Movie) });
                        }
                    }
                }
            }
        }

        public static void GetExtraMovieInfo(int tmdbId, Movie movie)
        {
            Uri request = new Uri("http://api.themoviedb.org/2.1/Movie.getInfo/en/xml/02004323eee9878ce511ca57faf0b29c/" + tmdbId);
            String response = SimpleWebRequest.DoRequest(request);

            if (!string.IsNullOrEmpty(response))
            {
                //Console.WriteLine(Response);

                XDocument xmlDoc = XDocument.Parse(response);

                //get elements from xml
                var localMovies = from movieEl in xmlDoc.Descendants("movie")
                                  select new
                                  {
                                      Plot = movieEl.Element("overview").Value,
                                      Name = movieEl.Element("name").Value,
                                      Genres = movieEl.Element("categories").Nodes().ToList(),
                                      Images = movieEl.Element("images").Nodes().ToList(),
                                      Cast = movieEl.Element("cast").Nodes().ToList()
                                  };

                var movieVar = localMovies.ToList()[0];
                GetImages(movieVar.Images, movie);
                movie.Name = movieVar.Name;
                movie.Plot = movieVar.Plot;

                movie.Genres.Clear();
                foreach (XNode genre in movieVar.Genres)
                {
                    XElement categoryElement = genre as XElement;
                    movie.Genres.Add(categoryElement.Attribute("name").Value);
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
            catch { }

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
