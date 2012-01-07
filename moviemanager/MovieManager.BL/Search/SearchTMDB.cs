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
            List<Movie> Videos = new List<Movie>();
            try
            {
                //do request
                Uri Request =
                    new Uri("http://api.themoviedb.org/2.1/Movie.search/en/xml/02004323eee9878ce511ca57faf0b29c/" +
                            HttpUtility.UrlEncode(query));
                String Response = SimpleWebRequest.DoRequest(Request);

                if (!string.IsNullOrEmpty(Response))
                {

                    XDocument XMLDoc = XDocument.Parse(Response);

                    //get elements from xml
                    var LocalMovies = from Movie in XMLDoc.Descendants("movie")
                                      select new
                                                 {
                                                     Id = Movie.Element("id").Value,
                                                     ImdbId = Movie.Element("imdb_id").Value,
                                                     Images = Movie.Element("images").Nodes().ToList()
                                                 };


                    //convert elements to Movie
                    foreach (var Movie in LocalMovies)
                    {
                        Movie NewMovie = new Movie
                                             {
                                                 IdImdb = Movie.ImdbId
                                             };

                        GetPosterFromMovie(Movie.Images, NewMovie);
                        GetExtraMovieInfo(Convert.ToInt32(Movie.Id), NewMovie);
                        Videos.Add(NewMovie);

                    }
                }
            }

            catch
            {
            }

            return Videos;
        }

        private static void GetPosterFromMovie(IEnumerable<XNode> images, Movie movie)
        {
            int MaxWidth = 0;
            foreach (XNode ImageNode in images)
            {
                XElement ImageEl = ImageNode as XElement;
                if (ImageEl != null && ImageEl.Attribute("type").Value == "poster"
                    && Convert.ToInt32(ImageEl.Attribute("width").Value) > MaxWidth)
                {
                    string Url = ImageEl.Attribute("url").Value;
                    if (!string.IsNullOrEmpty(Url))
                    {
                        movie.Poster = new Uri(Url);
                        MaxWidth = Convert.ToInt32(ImageEl.Attribute("width").Value);
                    }
                }
            }
        }

        private static void GetImages(IEnumerable<XNode> images, Movie movie)
        {
            bool FirstImage = true;
            foreach (XNode ImageNode in images)
            {
                XElement ImageEl = ImageNode as XElement;
                if (ImageEl != null && ImageEl.Attribute("size").Value == "original")
                {
                    string Url = ImageEl.Attribute("url").Value;
                    if (!string.IsNullOrEmpty(Url))
                    {
                        if (FirstImage)
                        {
                            movie.Poster = new Uri(Url);
                            FirstImage = false;
                        }
                        else
                        {
                            movie.Images.Add(new ImageInfo {Uri = new Uri(Url), Type = typeof (Movie)});
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

                XDocument XMLDoc = XDocument.Parse(Response);

                //get elements from xml
                var LocalMovies = from MovieEl in XMLDoc.Descendants("movie")
                                  select new
                                  {
                                      Plot = MovieEl.Element("overview").Value,
                                      Name = MovieEl.Element("name").Value,
                                      Images = MovieEl.Element("images").Nodes().ToList(),
                                      Cast = MovieEl.Element("cast").Nodes().ToList()
                                  };

                var MovieVar = LocalMovies.ToList()[0];
                GetImages(MovieVar.Images,movie);
                movie.Name = MovieVar.Name;
                movie.Plot = MovieVar.Plot;

            }
        }

        public static List<Actor> SearchActor(String query)
        {
            List<Actor> Actors = new List<Actor>();
            try
            {

                //do request
                Uri Request = new Uri("http://api.themoviedb.org/2.1/Person.search/en/xml/02004323eee9878ce511ca57faf0b29c/" + HttpUtility.UrlEncode(query));
                String Response = SimpleWebRequest.DoRequest(Request);

                if (!string.IsNullOrEmpty(Response))
                {
                    //Console.WriteLine(response);

                    XDocument XMLDoc = XDocument.Parse(Response);

                    //get elements from xml
                    var LocalActors = from Person in XMLDoc.Descendants("person")
                                      select new
                                      {
                                          TmdbID = Convert.ToInt32(Person.Element("id").Value),
                                          Name = Person.Element("name").Value,
                                          Images = Person.Element("images").Nodes().ToList()
                                      };

                    //convert elements to actor
                    foreach (var Actor in LocalActors)
                    {
                        Actor NewActor = new Actor
                                             {
                                                 Name = Actor.Name,
                                                 TmdbID = Actor.TmdbID
                                             };

                        foreach (XElement Image in Actor.Images)
                        {
                            NewActor.ImageUrls.Add(Image.Attribute("url").Value);
                        }

                        Actors.Add(NewActor);

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
                Uri Request = new Uri("http://api.themoviedb.org/2.1/Person.getInfo/en/xml/02004323eee9878ce511ca57faf0b29c/" + actor.TmdbID);
                String Response = SimpleWebRequest.DoRequest(Request);

                //Console.WriteLine(Response);

                if (!string.IsNullOrEmpty(Response))
                {
                    XDocument XDoc = XDocument.Parse(Response);

                    var Actors = from ActorEl in XDoc.Descendants("person")
                                 select new
                                 {
                                     ImagesMovies = ActorEl.Element("filmography").Nodes().ToList(),
                                     BirthPlace = ActorEl.Element("birthplace").Value,
                                     Biography = ActorEl.Element("biography").Value
                                 };

                    //convert to a Actor object
                    var Actor = Actors.ToList()[0];
                    actor.Birthplace = Actor.BirthPlace;
                    actor.Biography = Actor.Biography;
                    foreach (XNode MovieNode in Actor.ImagesMovies)
                    {
                        XElement MovieEl = MovieNode as XElement;
                        if (MovieEl != null)
                        {
                            string Url = MovieEl.Attribute("poster").Value;
                            string Name = MovieEl.Attribute("name").Value;
                            string Id = MovieEl.Attribute("id").Value;
                            if (!string.IsNullOrEmpty(Url))
                                actor.MovieImageUrls.Add(new ImageInfo { Uri = new Uri(Url), Tag = Id, Name = Name, Type = typeof(Movie) });
                        }
                    }
                }
            }
            catch { }
        }




    }
}
