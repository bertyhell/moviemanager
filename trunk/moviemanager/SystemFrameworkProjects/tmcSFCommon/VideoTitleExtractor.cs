using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Tmc.SystemFrameworks.Common
{
    public enum SearchOptions
    {
        DePreSubFix//, InternetSuggestion
    }

    public class VideoTitleExtractor
    {
        //TODO 030 split delimiters in 2 groups --> 1 where the regexp is less strict --> doesn't have to be a non alphanumeric char before and after the delimiter, and the rest
        static readonly String[] DELIMITERS = { "CD-1", "CD-2", "CD1", "CD2", "DVD-1", "DVD-2", "Divx-ITA", "XviD-ITA", "AC3", "DVDRip", "Xvid", "http", "www.", ".com", "shared", 
                                                  "powered", "sponsored", "sharelive", "filedonkey", "saugstube", "eselfilme", "eseldownloads", "emulemovies", "spanishare", "eselpsychos.de", 
                                                  "saughilfe.de", "goldesel.6x.to", "freedivx.org", "elitedivx", "deviance", "ftv", "flt", "1080p", "720p", "1080i", "720i", 
                                                  "480p", "x264", "h264", "ext", "6ch", "axxo", "pukka", "klaxxon", "edition", "limited", "dvdscr", "screener", "unrated", "BRRIP", "subs", "NL", 
                                                  "m-hd", "ts", "dvd rip", "hdtv", "widescreen", "bluray", "mp4", "wbz", "hd", "ue", "aac" };
        static readonly String[] REMOVE_CHARS = { ".", "(", ")", "{", "}", "[", "]", "_", "-" };

        /// <summary>
        /// cleans up title, returns the cleaned up title
        /// </summary>
        /// <param name="fileNameWithoutExtension">original file name</param>
        /// <returns>cleaned up filename, should be closest to movie title with year</returns>
        public static String CleanTitle(String fileNameWithoutExtension)
        {
            String MovieName = fileNameWithoutExtension.ToLower();
            //remove all chars after specific delimiters (common text after title, eg: dvdrip, cd1, ...)
            foreach (String Delimiter in DELIMITERS)
            {
                int FirstIndex = Regex.Match(MovieName, "^.*[^0-9a-z](" + Regex.Escape(Delimiter.ToLower()) + ")($|[^0-9a-z].*$)").Groups[1].Index;
                if (FirstIndex > 0)
                {
                    MovieName = MovieName.Substring(0, FirstIndex);
                }
            }
            //remove points and brackets, ...
            foreach (var RemoveChar in REMOVE_CHARS)
            {
                MovieName = MovieName.Replace(RemoveChar, " ");
            }
            return MovieName.Trim();
        }

        public static List<string> GetTitleGuessesFromPath(string videoPath)
        {
            var Guesses = new UniqueCollection<string>();
            videoPath = videoPath.ToLower();

            //guesses based on filename
            string FileName = Path.GetFileNameWithoutExtension(videoPath.ToLower());
           Guesses.AddRange(GetTitleGuessesFromString(FileName, false));

            //guesses based on foldername
            var DirectoryName = Path.GetDirectoryName(videoPath.ToLower());
            if (DirectoryName != null)
            {
                string FolderName = DirectoryName.Split(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar).Last();
                Guesses.AddRange(GetTitleGuessesFromString(FolderName, false));
                //TODO 030 only check foldername if its not a general folder (should only contain this moviefile, else it will be to general)
                //TODO 010 use all directories up untill folder where other videofiles are discovered (for videos who are 2 subfolders down from the mainfolder)
            }

            return Guesses;
        }

        public static List<string> GetTitleGuessesFromString(string name, bool dePreSubFix)
        {
            var Guesses = new UniqueCollection<string>();

            string Guess1 = CleanTitle(name);
            //remove text after realistic release yeardate (1800-2200):
            int FirstIndex = Regex.Match(Guess1, "^.*[^0-9]((1[89]|2[012])[0-9][0-9])($|[^0-9].*$)").Groups[3].Index;

            if (!dePreSubFix)
            {
                if (FirstIndex > 0)
                {
                    Guesses.Add(Guess1.Substring(0, FirstIndex));
                }
                Guesses.Add(Guess1);
            }
            else
            {
                //current title guess but drop 1 word from the end untill only 1 word left --> all guesses
                //remove suffixes
                int LastIndex = Guesses.Count;
                string[] Words = Guess1.Split(' ');
                string BuildTitle = Words[0];
                for (int I = 1; I < Words.Length - 1; I++) //length -1 cause full guess1 has already been added
                {
                    Guesses.Insert(LastIndex, BuildTitle);
                    BuildTitle += " " + Words[I];
                }
                Guesses.Insert(LastIndex, BuildTitle);

                //remove prefixes
                LastIndex = Guesses.Count;
                BuildTitle = Words[Words.Length-1];
                for (int I = Words.Length - 2; I >= 0; I--)
                {
                    Guesses.Insert(LastIndex, BuildTitle);
                    BuildTitle = Words[I] + " ";
                }
                Guesses.Insert(LastIndex, BuildTitle);
            }
            return Guesses;
        }
    }
}
