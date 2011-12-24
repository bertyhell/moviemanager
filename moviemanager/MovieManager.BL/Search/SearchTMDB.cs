using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using System.Globalization;
using System.Net;
using System.IO;
using System.Xml.Linq;
using System.Web;

namespace MovieManager.BL.Search
{
    public class SearchTMDB
    {
        public static void GetVideoInfoFromImdb(String query, Video video)
        {

            try
            {
                //do request
                Uri Request = new Uri("http://www.imdbapi.com/?r=XML&t=" + HttpUtility.UrlEncode(query));
                String Response = DoRequest(Request);

                if (!string.IsNullOrEmpty(Response))
                {
                    Console.WriteLine(Response);


                }
            }
            catch { }

            //    //parse request
            //    DocumentBuilderFactory dbf = DocumentBuilderFactory.newInstance();
            //    DocumentBuilder db = dbf.newDocumentBuilder();
            //    if (!response.contains("Parse Error")) {
            //    Document doc = db.parse(new InputSource(new java.io.StringReader(response)));
            //    doc.getDocumentElement();

            //    //get all movietags
            //    NodeList movieList = doc.getElementsByTagName("movie");

            //    //get informatie from movie nodes
            //    for (int i = 0; i < movieList.getLength(); i++) {
            //        Node movie = movieList.item(i);
            //        if (movie.getNodeType() == Node.ELEMENT_NODE) {
            //        Element movieElement = (Element) movie;
            //        video.setRelease(parseDate(movieElement.getAttribute("release")));
            //        try {
            //            double rating = Double.parseDouble(movieElement.getAttribute("rating"));
            //            video.setRatingImdb(rating);
            //        } catch (NumberFormatException e) {
            //            System.out.println(e.getMessage());
            //        }
            //        //TODO 100 get all the information in the node
            //        //TODO 100 Date doesn't get saved in database
            //        }
            //    }
            //    }
            //} catch (Exception e) {
            //    e.printStackTrace();
            //}
        }

        public static List<Actor> SearchActor(String query)
        {
            List<Actor> Actors = new List<Actor>();
            try
            {

                //do request
                Uri Request = new Uri("http://api.themoviedb.org/2.1/Person.search/en/xml/02004323eee9878ce511ca57faf0b29c/" + HttpUtility.UrlEncode(query));
                String Response = DoRequest(Request);

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
                String Response = DoRequest(Request);

                Console.WriteLine(Response);

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
                    foreach (XElement Image in Actor.ImagesMovies)
                    {
                        actor.MovieImageUrls.Add(new Uri(Image.Attribute("poster").Value));
                    }
                }
            }
            catch { }
        }

        private static DateTime ParseDate(String date)
        {
            DateTimeFormatInfo Format = new DateTimeFormatInfo();
            DateTime Datetime;
            try
            {
                Format.FullDateTimePattern = "d MMM yyyy";

                Datetime = DateTime.Parse(date, Format);
            }
            catch
            {
                try
                {
                    Format.FullDateTimePattern = "yyyy";
                    Datetime = DateTime.Parse(date, Format);
                }
                catch
                {
                    return DateTime.MinValue;
                }
            }
            return Datetime;
        }

        /**
         * 
         * @param url
         * @return
         */
        private static String DoRequest(Uri url)
        {

            WebClient Client = new WebClient();
            Stream Data = Client.OpenRead(url);
            string ResponseXML = null;
            if (Data != null)
            {
                StreamReader Reader = new StreamReader(Data);
                ResponseXML = Reader.ReadToEnd();
                Console.WriteLine(ResponseXML);
                Data.Close();
                Reader.Close();
            }

            return ResponseXML;

        }
    }
}
