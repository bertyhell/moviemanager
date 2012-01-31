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
                Uri request = new Uri("http://www.imdbapi.com/?r=XML&i=" + HttpUtility.UrlEncode(video.IdImdb));
                String response = SimpleWebRequest.DoRequest(request);

                if (!string.IsNullOrEmpty(response))
                {
                    Console.WriteLine(response);

                    XDocument xmlDoc = XDocument.Parse(response);

                    //get elements from xml
                    var localMovies = from mov in xmlDoc.Descendants("movie")
                                      select new
                                      {
                                          RatingImdb = mov.Attribute("rating").Value,
                                          Release = mov.Attribute("released").Value
                                      };

                    var vid = localMovies.ToList()[0];
                    video.RatingImdb = double.Parse(vid.RatingImdb);
                    video.Release = DateTimeUtilities.ParseDate(vid.Release);
                }
            }
            catch
            {
            }
        }

    }
}
