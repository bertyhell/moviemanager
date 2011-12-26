using System;
using System.IO;
using System.Net;

namespace Common
{
    public class SimpleWebRequest
    {        /**
         * 
         * @param url
         * @return
         */
        public static String DoRequest(Uri url)
        {

            WebClient Client = new WebClient();
            Stream Data = Client.OpenRead(url);
            string ResponseXML = null;
            if (Data != null)
            {
                StreamReader Reader = new StreamReader(Data);
                ResponseXML = Reader.ReadToEnd();
                Data.Close();
                Reader.Close();
            }

            return ResponseXML;

        }
    }
}
