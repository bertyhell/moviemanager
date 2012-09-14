using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Media;
using Common;
using MovieManager.WEB.Search;

namespace MovieManager.APP.Cache
{
    public class ApplicationCache
    {
        private static string _videosFolder;
        private static string _actorsFolder;

        static ApplicationCache()
        {
            Init();
        }

        public static void Init()
        {
            var CacheFolder = Properties.Settings.Default.Cache_folder;
            Directory.CreateDirectory(CacheFolder);
            _videosFolder = Path.Combine(CacheFolder, "Videos");
            Directory.CreateDirectory(_videosFolder);
            _actorsFolder = Path.Combine(CacheFolder, "Actors");
            Directory.CreateDirectory(_actorsFolder);
        }

        public static void AddVideoImages(int videoId, Dictionary<string,Image> images, CacheImageType imageType)
        {
            if((int)imageType >= (int)CacheImageType.PostersAndBackdrops)
                throw new NotSupportedException("Combined cache image type not supported for setters!");
            ImageQuality BackdropSize = Properties.Settings.Default.ImageQuality;
            string VideoDir = Path.Combine(_videosFolder, videoId.ToString(CultureInfo.InvariantCulture), imageType.ToString(), BackdropSize.ToString());
            Directory.CreateDirectory(VideoDir);
            foreach (KeyValuePair<string, Image> Pair in images)
            {
                Pair.Value.Save(Path.Combine(VideoDir, Pair.Key));
            }
        }

        public static List<Image> GetImages(int videoId, CacheImageType imageType)
        {
            List<Image> RetVal = new List<Image>();

            return RetVal;
        }

        public static Image GetImage(string uri, CacheImageType imageType)
        {
            string hashCode = uri.GetHashCode();
            ImageQuality minImageQuality = Properties.Settings.Default.ImageQuality;
            string VideoDir = Path.Combine(_videosFolder, videoId.ToString(CultureInfo.InvariantCulture), imageType.ToString(), minImageQuality.ToString());

        }



    }

    public enum CacheImageType
    {
        Backdrops = 0,
        Posters = 1,
        Profiles = 2,
        PostersAndBackdrops =3
    }

}
