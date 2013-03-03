using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tmc.BusinessRules.Web.Search
{
    internal static class Constants
    {
        private static readonly String MOVIE = "movie";
        private static readonly String IMAGES = "images";
        public static readonly String SEARCH = "search";
        public static readonly String QUERY = "query";
        private static readonly String API_KEY_PARAM = "api_key";

        public static readonly String TMDB_API_URL = "http://api.themoviedb.org/";
        public static readonly String TMDB_API_VERSION = "3";

        public static readonly string TMDB_API_URL_SEARCH = TMDB_API_URL + TMDB_API_VERSION + "/" + SEARCH + "/";

        public static readonly string TMDB_API_URL_GENRES =TMDB_API_URL + "/" + TMDB_API_VERSION + "/genre/list?" + API_KEY_PARAM + "={0}";

        /// <summary>
        /// TMDB Movie url --> params: Movie id, Api key
        /// </summary>
        public static readonly string TMDB_API_URL_MOVIE = TMDB_API_URL + TMDB_API_VERSION + "/" + MOVIE + "/{0}?" + API_KEY_PARAM + "={1}";

        /// <summary>
        /// TMDB Movie images url --> params: Movie id, Api key
        /// </summary>
        public static readonly string TMDB_API_URL_MOVIE_IMAGES = TMDB_API_URL + TMDB_API_VERSION + "/" + MOVIE + "/{0}/" + IMAGES + "?" + API_KEY_PARAM + "={1}";

        /// <summary>
        /// TMDB Movie images url --> params: search string, Api key
        /// </summary>
        public static readonly String TMDB_API_URL_MOVIE_SEARCH = TMDB_API_URL_SEARCH + MOVIE + "?" + API_KEY_PARAM + "={1}&" + QUERY + "={0}";
    }
}
