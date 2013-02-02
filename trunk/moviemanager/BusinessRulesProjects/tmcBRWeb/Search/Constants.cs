using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tmc.BusinessRules.Web.Search
{
    internal static class Constants
    {
        public static readonly String TMDB_API_URL = "http://api.themoviedb.org/";
        public static readonly String TMDB_API_VERSION = "3";
        public static readonly String TMDB_API_SEARCH = "search";

        public static readonly string TMDB_API_URL_SEARCH = TMDB_API_URL + TMDB_API_VERSION + "/" + TMDB_API_SEARCH + "/";

        public static readonly String TMDB_API_URL_SEARCH_MOVIE = TMDB_API_URL_SEARCH + "movie";
    }
}
