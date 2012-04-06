using System;
using System.Net;

namespace Common
{
    public class SimpleWebRequest
    {        /**
         * 
         * @param url
         * @return
         */
        public static String DoJSONRequest(Uri url)
        {

            WebClient Client = new WebClient();
            Client.Headers["Accept"] = "application/json";
            return Client.DownloadString(url);

        }

        public static String DoRequest(Uri url)
        {

            WebClient Client = new WebClient();
            return Client.DownloadString(url);
            //string responseXML = null;
            //if (data != null)
            //{
            //    StreamReader reader = new StreamReader(data);
            //    responseXML = reader.ReadToEnd();
            //    data.Close();
            //    reader.Close();
            //}

            //return responseXML;

        }
    }
}
