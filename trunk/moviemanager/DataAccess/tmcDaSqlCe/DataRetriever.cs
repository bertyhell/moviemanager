using System.Collections.Generic;
using Model;

namespace Tmc.DataAccess.SqlCe
{
    public class DataRetriever
    {

        private static readonly TmcContext DB = new TmcContext();

        public static List<Video> Videos
        {
            get { return new List<Video>(DB.Videos); }
        }

    }
}
