using System;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using Common;
using Model;

namespace MovieManager.BL.Search
{
    public class SearchIMDB
    {
        public static Video GetVideoInfo(String query)
        {
            Video Video = null;
            try
            {
                //do request
                Uri Request = new Uri("http://www.imdbapi.com/?r=XML&t=" + HttpUtility.UrlEncode(query));
                String Response = SimpleWebRequest.DoRequest(Request);

                if (!string.IsNullOrEmpty(Response))
                {
                    Console.WriteLine(Response);

                    XDocument XMLDoc = XDocument.Parse(Response);

                    //get elements from xml
                    var LocalMovies = from Movie in XMLDoc.Descendants("movie")
                                      select new
                                      {
                                          TmdbID = Convert.ToInt32(Movie.Element("rating").Value),
                                          Name = Movie.Element("release").Value
                                      };

                    Video = new Video();
                    //convert elements to actor
                    //foreach (var Movie in LocalMovies)
                    //{
                    //    Actor NewActor = new Actor
                    //    {
                    //        Name = Movie.Name,
                    //        TmdbID = Movie.TmdbID
                    //    };

                    //Actors.Add(NewActor);

                }


            }

            catch
            {
            }

            return Video;
        }

    }
}
