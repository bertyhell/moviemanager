﻿using System;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using Common;
using Model;

namespace MovieManager.WEB.Search
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
            Uri Request = new Uri("http://www.imdbapi.com/?r=XML&i=" + HttpUtility.UrlEncode(video.IdImdb));
            try
            {
                //do request
                String Response = SimpleWebRequest.DoRequest(Request);

                if (!string.IsNullOrEmpty(Response))
                {
                    Console.WriteLine(Response);

                    XDocument XMLDoc = XDocument.Parse(Response);

                    //get elements from xml
                    var LocalMovies = from Mov in XMLDoc.Descendants("movie")
                                      let XAttribute = Mov.Attribute("rating")
                                      where XAttribute != null
                                      let Attribute = Mov.Attribute("released")
                                      where Attribute != null
                                      select new
                                      {
                                          RatingImdb = XAttribute.Value,
                                          Release = Attribute.Value
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
