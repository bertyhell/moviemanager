using System.Collections.Generic;

namespace MovieManager.WEB.Search
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

        public static int SelectedBackdropSizeIndex { get; set; }

        public static int SelectedPosterSizeIndex { get; set; }

        public static int SelectedProfileSizeIndex { get; set; }

        public static string SeletedPosterSize { get { return PosterSizes[SelectedPosterSizeIndex]; } }

        public static string SeletedBackdropSize { get { return BackdropSizes[SelectedBackdropSizeIndex]; } }

        public static string SeletedProfileSize { get { return ProfileSizes[SelectedProfileSizeIndex]; } }
    }
}
