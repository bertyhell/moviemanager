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

        public static void GetVideoInfo(Video video)
        {
            if (!string.IsNullOrEmpty(video.IdImdb))
            {
                GetVideoInfoFromId(video);
            }
        }


        private static void GetVideoInfoFromId(Video video)
        {
            try
            {
                //do request
                Uri Request = new Uri("http://www.imdbapi.com/?r=XML&i=" + HttpUtility.UrlEncode(video.IdImdb));
                String Response = SimpleWebRequest.DoRequest(Request);

                if (!string.IsNullOrEmpty(Response))
                {
                    Console.WriteLine(Response);

                    XDocument XMLDoc = XDocument.Parse(Response);

                    //get elements from xml
                    var LocalMovies = from Mov in XMLDoc.Descendants("movie")
                                      select new
                                      {
                                          RatingImdb = Mov.Attribute("rating").Value,
                                          Release = Mov.Attribute("released").Value
                                      };

                    var Vid = LocalMovies.ToList()[0];
                    video.RatingImdb = double.Parse(Vid.RatingImdb);
                    video.Release = DateTimeUtilities.ParseDate(Vid.Release);
                }
            }
            catch
            {
            }
        }

    }
}
