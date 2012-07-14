﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Common;
using Model;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace MovieManager.WEB.Search
{
    public class SearchTMDB
    {
        private const string APIKEY = "02004323eee9878ce511ca57faf0b29c";

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

                    foreach (JToken JToken in Results)
                    {
                        Videos.Add(new Movie
                        {
                            Name = (string)JToken["title"],
                            IdTmdb = (int)JToken["id"],
                            Images = new List<ImageInfo> { new ImageInfo { Uri = new Uri(TMDBConfiguration.BaseUrl + TMDBConfiguration.SeletedPosterSize + (string)JToken["poster_path"]) } },
                            Release =ParseTmdbDate((string)JToken["release_date"])
                        });
                    }
                }
            }

            catch
            {
            }

            return Videos;
        }

        public static void GetMovieImages(Movie movie)
        {
            Uri Request = new Uri("http://api.themoviedb.org/3/movie/" + movie.IdTmdb + "/images?api_key=" + APIKEY);
            String Response = SimpleWebRequest.DoJSONRequest(Request);

            if (!string.IsNullOrEmpty(Response))
            {
                JObject JSonImages = JObject.Parse(Response);
                JSonImages["posters"].ToList().ForEach(g => movie.Images.Add(
                    new ImageInfo { Uri = new Uri(TMDBConfiguration.BaseUrl + TMDBConfiguration.SeletedPosterSize + g["file_path"]) })
                );
                JSonImages["backdrops"].ToList().ForEach(g => movie.Images.Add(
                    new ImageInfo { Uri = new Uri(TMDBConfiguration.BaseUrl + TMDBConfiguration.SeletedBackdropSize + g["file_path"]) })
                );
            }
        }

        public static void GetExtraMovieInfo(Movie movie)
        {
            Uri Request = new Uri("http://api.themoviedb.org/3/movie/" + movie.IdTmdb + "?api_key=" + APIKEY);
            String Response = SimpleWebRequest.DoJSONRequest(Request);

            if (!string.IsNullOrEmpty(Response))
            {
                JObject JsonMovie = JObject.Parse(Response);
                List<string> Genres = new List<string>();
                JsonMovie["genres"].ToList().ForEach(g => Genres.Add((string)g["name"])); // get genres
                movie.Genres = Genres;
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
                        LocalActor.ImageUrls.Add(TMDBConfiguration.BaseUrl + TMDBConfiguration.SeletedPosterSize +
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
