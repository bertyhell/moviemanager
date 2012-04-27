using System.Collections.Generic;

namespace MovieManager.BL.Search
{
    public static class TMDBConfiguration
    {
        static TMDBConfiguration()
        {
            BackdropSizes = new List<string>();
            PosterSizes = new List<string>();
            ProfileSizes = new List<string>();
        }

        public static List<string> BackdropSizes { get; set; }

        public static string BaseUrl { get; set; }

        public static List<string> PosterSizes { get; set; }

        public static List<string> ProfileSizes { get; set; }
    }
}
