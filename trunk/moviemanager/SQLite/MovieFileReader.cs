using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Model;
using System.IO;
using System.Text.RegularExpressions;
using SQLite.RegexSettings;

namespace SQLite
{
    public class MovieFileReader : BackgroundWorker
    { // TODO 060: extends SwingWorker

        static readonly String[] VideoFileExtensions = { "ASX", "DTS", "GXF", "M2V", "M3U", "M4V", "MPEG1", "MPEG2", "MTS", "MXF", "OGM", "PLS", "BUP", "A52", "AAC", "B4S", "CUE", "DIVX", "DV", "FLV", "M1V", "M2TS", "MKV", "MOV", "MPEG4", "OMA", "SPX", "TS", "VLC", "VOB", "XSPF", "DAT", "BIN", "IFO", "PART", "3G2", "AVI", "MPEG", "MPG", "FLAC", "M4A", "MP1", "OGG", "WAV", "XM", "3GP", "WMV", "AC3", "ASF", "MOD", "MP2", "MP3", "MP4", "WMA", "MKA", "M4P" };
        static readonly String[] DELIMITERS = { "CD-1", "CD-2", "CD1", "CD2", "DVD-1", "DVD-2", "[Divx-ITA]", "[XviD-ITA]", "AC3", "DVDRip", "Xvid", "http", "www.", ".com", "shared", "powered", "sponsored", "sharelive", "filedonkey", "saugstube", "eselfilme", "eseldownloads", "emulemovies", "spanishare", "eselpsychos.de", "saughilfe.de", "goldesel.6x.to", "freedivx.org", "elitedivx", "deviance", "-ftv", "ftv", "-flt", "flt", "1080p", "720p", "1080i", "720i", "480", "x264", "ext", "ac3", "6ch", "axxo", "pukka", "klaxxon", "edition", "limited", "dvdscr", "screener", "unrated", "BRRIP", "subs", "_NL_", "m-hd" };

        private DirectoryInfo _dir;
        private FileInfo _file;

        public MovieFileReader(DirectoryInfo dir)
        {
            _dir = dir;
            _videos = new ObservableCollection<Video>();
        }
        public MovieFileReader(FileInfo file)
        {
            _file = file;
            _videos = new ObservableCollection<Video>();
        }

        private ObservableCollection<Video> _videos;
        public ObservableCollection<Video> Videos
        {
            get { return _videos; }
            set { _videos = value; }
        }

        private static void GetVideos(DirectoryInfo dir, ObservableCollection<Video> videos)
        { // TODO 050 Add search options -> minimal size, limit extensions, ...
            try
            {
                foreach (FileInfo file in dir.GetFiles())
                {

                    if (file.Name.Length > file.Extension.Length)
                        GetVideos(file, videos);

                }
            }
            catch (Exception e){}
            try{
            foreach (DirectoryInfo directory in dir.GetDirectories())
            {
                GetVideos(directory, videos);
            }
            }
            catch (Exception e){}
        }

        private static void GetVideos(FileInfo file, ObservableCollection<Video> videos)
        {
            if (!string.IsNullOrEmpty(file.Extension) && VideoFileExtensions.Contains(file.Extension.ToUpper().Substring(1)))
            {
                videos.Add(new Video
                {
                    Path = file.FullName.Replace("\'", "''"),
                    Name = CleanTitle(file.Name.Substring(0, file.Name.LastIndexOf(".")))
                });
                Console.WriteLine(file.FullName);
            }
        }
        /**
         * cleans up title, returns the cleaned up title
         * @param fileName original file name
         * @return cleaned up filename, should be closest to movie title with year
         */
        public static String CleanTitle(String fileName)
        {
            String movieName = fileName.ToLower();
            foreach (String delimiter in DELIMITERS)
            {
                int firstIndex = movieName.IndexOf(delimiter.ToLower());
                if (firstIndex != -1)
                {
                    movieName = movieName.Substring(0, firstIndex);
                }
            }

            return movieName.Replace(".", " ").Replace("(", " ").Replace(")", " ").Replace("_", " ").Trim();
        }


        #region series

        public static void GetSerie(DirectoryInfo dir, string seasonDir, string episodeString, ObservableCollection<Video> videos)
        {


            //get all files
            ObservableCollection<Video> LocalVideos = new ObservableCollection<Video>();
            GetVideos(dir, LocalVideos);

            //create Serie in database
            Serie Serie = new Serie();
            Serie.Name = dir.FullName.Substring(dir.FullName.LastIndexOf("\\") + 1);
            MMDatabase.AddSerie(Serie);

            //convert video to episode
            foreach (Video Video in LocalVideos)
            {
                FileInfo FileInfo = new FileInfo(Video.Path);
                //int LastIndexOf = Video.Path.LastIndexOf(dir.FullName);
                //string Path = Video.Path.Remove(0, LastIndexOf);

                //find episodenumber in Filename
                Episode Episode = null;
                bool RegexMatched = false;
                int Index = 0;
                ObservableCollection<String> RegularExpressions = RegexSettingsStorage.EpisodeRegularExpressions;

                while (!RegexMatched && Index < RegularExpressions.Count)
                {
                    String RegEx = RegularExpressions[Index];
                    Match Match = Regex.Match(FileInfo.Name, RegEx);
                    if (Match.Success)
                    {
                        int SeasonNumber = int.Parse(Match.Groups[1].Value);
                        int EpisodeNumber = int.Parse(Match.Groups[2].Value);

                        Episode = (Episode)Video.ConvertVideo(VideoTypeEnum.Episode, Video);
                        Episode.EpisodeNumber = EpisodeNumber;
                        Episode.Season = SeasonNumber;
                        Episode.SerieId = Serie.Id;
                        videos.Add(Episode);

                        RegexMatched = true;
                    }
                    Index++;
                }
            }
        }

        #endregion

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            GetVideos(_dir, _videos);
        }


        public event EventHandler<GetVideoCompletedEventArgs> OnGetVideoCompleted;
        protected override void OnRunWorkerCompleted(RunWorkerCompletedEventArgs e)
        {
            if (OnGetVideoCompleted != null)
                OnGetVideoCompleted(this, new GetVideoCompletedEventArgs { Videos = _videos });
        }

        protected override void OnProgressChanged(ProgressChangedEventArgs e)
        {

        }
    }

    public class GetVideoCompletedEventArgs : EventArgs
    {
        public ObservableCollection<Video> Videos { get; set; }
    }
}
