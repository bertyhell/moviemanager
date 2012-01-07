using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using System.IO;

namespace SQLite
{
    public class MovieFileReader
    { // TODO 060: extends SwingWorker

        static readonly String[] VIDEO_FILE_EXTENSIONS = { "ASX", "DTS", "GXF", "M2V", "M3U", "M4V", "MPEG1", "MPEG2", "MTS", "MXF", "OGM", "PLS", "BUP", "A52", "AAC", "B4S", "CUE", "DIVX", "DV", "FLV", "M1V", "M2TS", "MKV", "MOV", "MPEG4", "OMA", "SPX", "TS", "VLC", "VOB", "XSPF", "DAT", "BIN", "IFO", "PART", "3G2", "AVI", "MPEG", "MPG", "FLAC", "M4A", "MP1", "OGG", "WAV", "XM", "3GP", "SRT", "WMV", "AC3", "ASF", "MOD", "MP2", "MP3", "MP4", "WMA", "MKA", "M4P" };
        static readonly String[] DELIMITERS = { "CD-1", "CD-2", "CD1", "CD2", "DVD-1", "DVD-2", "[Divx-ITA]", "[XviD-ITA]", "AC3", "DVDRip", "Xvid", "http", "www.", ".com", "shared", "powered", "sponsored", "sharelive", "filedonkey", "saugstube", "eselfilme", "eseldownloads", "emulemovies", "spanishare", "eselpsychos.de", "saughilfe.de", "goldesel.6x.to", "freedivx.org", "elitedivx", "deviance", "-ftv", "ftv", "-flt", "flt", "1080p", "720p", "1080i", "720i", "480", "x264", "ext", "ac3", "6ch", "axxo", "pukka", "klaxxon", "edition", "limited", "dvdscr", "screener", "unrated", "BRRIP", "subs", "_NL_", "m-hd" };

        public static void GetVideos(DirectoryInfo dir, List<Video> videos)
        { // TODO 050 Add search options -> minimal size, limit extensions, ...

            foreach (FileInfo File in dir.EnumerateFiles())
            {
                GetVideos(File, videos);
            }
            foreach (DirectoryInfo Dir in dir.EnumerateDirectories())
            {
                GetVideos(Dir, videos);
            }
        }

        public static void GetVideos(FileInfo file, List<Video> videos)
        {
            if (VIDEO_FILE_EXTENSIONS.Contains(file.Extension.ToUpper().Substring(1)))
            {
                videos.Add(new Video
                {
                    Path = file.FullName,
                    Name = CleanTitle(file.Name.Substring(0,file.Name.LastIndexOf(".")))
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
            String MovieName = fileName.ToLower();
            foreach (String Delimiter in DELIMITERS)
            {
                int FirstIndex = MovieName.IndexOf(Delimiter.ToLower());
                if (FirstIndex != -1)
                {
                    MovieName = MovieName.Substring(0, FirstIndex);
                }
            }

            return MovieName.Replace(".", " ").Replace("(", " ").Replace(")", " ").Replace("_", " ").Trim();
        }
    }
}
