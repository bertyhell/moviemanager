using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Model;
using System.Xml.Linq;
using System.Web;

namespace MovieManager.BL.Search
{
    public class SearchTMDB
    {
        public static List<Movie> GetVideoInfo(String query)
        {
            List<Movie> videos = new List<Movie>();
            try
            {
                //do request
                Uri request =
                    new Uri("http://api.themoviedb.org/2.1/Movie.search/en/xml/02004323eee9878ce511ca57faf0b29c/" +
                            HttpUtility.UrlEncode(query));
                String response = SimpleWebRequest.DoRequest(request);

                if (!string.IsNullOrEmpty(response))
                {

                    XDocument xmlDoc = XDocument.Parse(response);

                    //get elements from xml
                    var localMovies = from movie in xmlDoc.Descendants("movie")
                                      select new
                                                 {
                                                     TmdbId = movie.Element("id").Value,
                                                     ImdbId = movie.Element("imdb_id").Value,
                                                     Images = movie.Element("images").Nodes().ToList()
                                                 };


                    //convert elements to Movie
                    foreach (var movie in localMovies)
                    {
                        Movie newMovie = new Movie
                                             {
                                                 IdImdb = movie.ImdbId,
                                                 IdTmdb = int.Parse(movie.TmdbId)
                                             };
                        GetPosterFromMovie(movie.Images, newMovie);
                        videos.Add(newMovie);

                    }
                }
            }

            catch
            {
            }

            return videos;
        }

        private static void GetPosterFromMovie(IEnumerable<XNode> images, Movie movie)
        {
            int maxWidth = 0;
            foreach (XNode imageNode in images)
            {
                XElement imageEl = imageNode as XElement;
                if (imageEl != null && imageEl.Attribute("type").Value == "poster"
                    && Convert.ToInt32(imageEl.Attribute("width").Value) > maxWidth)
                {
                    string url = imageEl.Attribute("url").Value;
                    if (!string.IsNullOrEmpty(url))
                    {
                        movie.Poster = new Uri(url);
                        maxWidth = Convert.ToInt32(imageEl.Attribute("width").Value);
                    }
                }
            }
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
                            movie.Images.Add(new ImageInfo {Uri = new Uri(url), Type = typeof (Movie)});
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
                GetImages(movieVar.Images,movie);
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

        public static List<Actor> SearchActor(String query)
        {
            List<Actor> actors = new List<Actor>();
            try
            {

                //do request
                Uri request = new Uri("http://api.themoviedb.org/2.1/Person.search/en/xml/02004323eee9878ce511ca57faf0b29c/" + HttpUtility.UrlEncode(query));
                String response = SimpleWebRequest.DoRequest(request);

                if (!string.IsNullOrEmpty(response))
                {
                    //Console.WriteLine(response);

                    XDocument xmlDoc = XDocument.Parse(response);

                    //get elements from xml
                    var localActors = from person in xmlDoc.Descendants("person")
                                      select new
                                      {
                                          TmdbID = Convert.ToInt32(person.Element("id").Value),
                                          Name = person.Element("name").Value,
                                          Images = person.Element("images").Nodes().ToList()
                                      };

                    //convert elements to actor
                    foreach (var actor in localActors)
                    {
                        Actor newActor = new Actor
                                             {
                                                 Name = actor.Name,
                                                 TmdbID = actor.TmdbID
                                             };

                        foreach (XElement image in actor.Images)
                        {
                            newActor.ImageUrls.Add(image.Attribute("url").Value);
                        }

                        actors.Add(newActor);
                    }
                }
            }
            catch { }

            return actors;
        }


        public static void GetActorInfo(Actor Actor)
        {
            try
            {

                //do request
                Uri request = new Uri("http://api.themoviedb.org/2.1/Person.getInfo/en/xml/02004323eee9878ce511ca57faf0b29c/" + Actor.TmdbID);
                String response = SimpleWebRequest.DoRequest(request);

                //Console.WriteLine(Response);

                if (!string.IsNullOrEmpty(response))
                {
                    XDocument xDoc = XDocument.Parse(response);

                    var actors = from actorEl in xDoc.Descendants("person")
                                 select new
                                 {
                                     ImagesMovies = actorEl.Element("filmography").Nodes().ToList(),
                                     BirthPlace = actorEl.Element("birthplace").Value,
                                     Biography = actorEl.Element("biography").Value
                                 };

                    //convert to a Actor object
                    var actor = actors.ToList()[0];
                    Actor.Birthplace = actor.BirthPlace;
                    Actor.Biography = actor.Biography;
                    foreach (XNode movieNode in actor.ImagesMovies)
                    {
                        XElement movieEl = movieNode as XElement;
                        if (movieEl != null)
                        {
                            string url = movieEl.Attribute("poster").Value;
                            string name = movieEl.Attribute("name").Value;
                            string id = movieEl.Attribute("id").Value;
                            if (!string.IsNullOrEmpty(url))
                                Actor.MovieImageUrls.Add(new ImageInfo { Uri = new Uri(url), Tag = id, Name = name, Type = typeof(Movie) });
                        }
                    }
                }
            }
            catch { }
        }




    }
}
