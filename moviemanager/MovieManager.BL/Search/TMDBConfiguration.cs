using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieManager.BL.Search
{
    public class TMDBConfiguration
    {
        public TMDBConfiguration()
        {
            BackdropSizes = new List<string>();
            PosterSizes = new List<string>();
            ProfileSizes = new List<string>();
        }

        public List<string> BackdropSizes { get; set; }

        public string BaseUrl { get; set; }

        public List<string> PosterSizes { get; set; }

        public List<string> ProfileSizes { get; set; }
    }
}
