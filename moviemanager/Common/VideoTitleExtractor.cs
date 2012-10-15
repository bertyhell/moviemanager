using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Common
{
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

        public static List<string> GetTitleGuesses(string videoPath)
        {
            string VideoPath = videoPath.ToLower();
            var Guesses = new UniqueCollection<string>();

            //guesses based on filename
            string FileName = Path.GetFileNameWithoutExtension(VideoPath);
            List<string> GuessesFromFileName = GetTitleGuessesFromText(FileName);

            //guesses based on foldername
            List<string> GuessesFromFolderName = new List<string>();
            var DirectoryName = Path.GetDirectoryName(VideoPath);
            if (DirectoryName != null)
            {
                string FolderName = DirectoryName.Split(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar).Last();
                GuessesFromFolderName = GetTitleGuessesFromText(FolderName);//TODO 030 only check foldername if its not a general folder (should only contain this moviefile, else it will be to general)
            }

            //TODO 010 use all directories up untill folder where other videofiles are discovered (for videos who are 2 subfolders down from the mainfolder)

            //TODO 020 during analysis check all possible matches and choose the one with the best textual match to the original filename/foldername

            Guesses.Add(GuessesFromFileName[0]);
            Guesses.Add(GuessesFromFolderName[0]);
            if (GuessesFromFileName.Count > 1) Guesses.Add(GuessesFromFileName[1]);
            if (GuessesFromFolderName.Count > 1) Guesses.Add(GuessesFromFolderName[1]);



            //TODO 010 check if this improves accuracy (parts of title being searched) --> better do this in second pass or third
            //TODO 005 also do reverse --> remove words from the front --> in second or third pass
            if (GuessesFromFileName.Count > 2) Guesses.AddRange(GuessesFromFileName.GetRange(2, GuessesFromFileName.Count - 2));
            if (GuessesFromFolderName.Count > 2) Guesses.AddRange(GuessesFromFolderName.GetRange(2, GuessesFromFolderName.Count - 2));

            return Guesses;
        }

        private static List<string> GetTitleGuessesFromText(string text)
        {
            var Guesses = new UniqueCollection<string>();

            string Guess1 = CleanTitle(text);
            //remove text after realistic release yeardate (1800-2200):
            int FirstIndex = Regex.Match(Guess1, "^.*[^0-9]((1[89]|2[012])[0-9][0-9])($|[^0-9].*$)").Groups[3].Index;
            if (FirstIndex > 0)
            {
                Guesses.Add(Guess1.Substring(0, FirstIndex));
            }
            Guesses.Add(Guess1);

            //current title guess but drop 1 word from the end untill only 1 word left --> all guesses
            int LastIndex = Guesses.Count;
            string[] Words = Guess1.Split(' ');
            string BuildTitle = Words[0];
            for (int I = 1; I < Words.Length - 1; I++)//length -1 cause full guess1 has already been added
            {
                Guesses.Insert(LastIndex, BuildTitle);
                BuildTitle += " " + Words[I];
            }
            Guesses.Insert(LastIndex, BuildTitle);
            return Guesses;
        }

    }
}
