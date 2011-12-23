using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public static void getVideoInfoFromImdb(String query, Video video) {

            try
            {
                //do request
                Uri request = new Uri("http://www.imdbapi.com/?r=XML&t=" + HttpUtility.UrlEncode(query));
                String response = doRequest(request);

                if (!string.IsNullOrEmpty(response))
                {
                    Console.WriteLine(response);


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
                Uri request = new Uri("http://api.themoviedb.org/2.1/Person.search/en/xml/02004323eee9878ce511ca57faf0b29c/" + HttpUtility.UrlEncode(query));
                String response = doRequest(request);

                if (!string.IsNullOrEmpty(response))
                {
                    //Console.WriteLine(response);

                    XDocument XMLDoc = XDocument.Parse(response);

                    //get elements from xml
                    var actors = from Person in XMLDoc.Descendants("person")
                                 select new
                                 {
                                     TmdbID = Convert.ToInt32(Person.Element("id").Value),
                                     Name = Person.Element("name").Value,
                                     Images = Person.Element("images").Nodes().ToList()
                                 };

                    //convert elements to actor
                    foreach (var actor in actors)
                    {
                        Actor newActor = new Actor()
                        {
                            Name = actor.Name,
                            TmdbID = actor.TmdbID
                        };

                        foreach (XElement image in actor.Images)
                        {
                            newActor.ImageUrls.Add(image.Attribute("url").Value);
                        }

                        Actors.Add(newActor);

                    }
                }
            }
            catch { }

            return Actors;
        }


        public static void getActorInfo(Actor actor)
        {
            try
            {

                //do request
                Uri request = new Uri("http://api.themoviedb.org/2.1/Person.getInfo/en/xml/02004323eee9878ce511ca57faf0b29c/" + actor.TmdbID);
                String response = doRequest(request);

                Console.WriteLine(response);

                if (!string.IsNullOrEmpty(response))
                {
                    XDocument XDoc = XDocument.Parse(response);

                    var actors = from actorEl in XDoc.Descendants("person")
                                 select new
                                 {
                                     ImagesMovies = actorEl.Element("filmography").Nodes().ToList()
                                 };

                    foreach (XElement image in actors.ToList()[0].ImagesMovies)
                    {
                        actor.MovieImageUrls.Add(image.Attribute("poster").Value);
                    }
                }
            }
            catch { }
        }

        private static DateTime parseDate(String date)
        {
            DateTimeFormatInfo format = new DateTimeFormatInfo();
            DateTime datetime;
            try
            {
                format.FullDateTimePattern = "d MMM yyyy";

                datetime = DateTime.Parse(date, format);
            }
            catch
            {
                try
                {
                    format.FullDateTimePattern = "yyyy";
                    datetime = DateTime.Parse(date, format);
                }
                catch
                {
                    return DateTime.MinValue;
                }
            }
            return datetime;

        }

        /**
         * 
         * @param url
         * @return
         */
        private static String doRequest(Uri url)
        {

            WebClient client = new WebClient();
            Stream data = client.OpenRead(url);
            StreamReader reader = new StreamReader(data);
            string ResponseXML = reader.ReadToEnd();
            Console.WriteLine(ResponseXML);
            data.Close();
            reader.Close();

            return ResponseXML;

        }
    }
}
