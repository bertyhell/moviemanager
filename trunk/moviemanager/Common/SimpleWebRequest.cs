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

            WebClient client = new WebClient();
            Stream data = client.OpenRead(url);
            string responseXML = null;
            if (data != null)
            {
                StreamReader reader = new StreamReader(data);
                responseXML = reader.ReadToEnd();
                data.Close();
                reader.Close();
            }

            return responseXML;

        }
    }
}
