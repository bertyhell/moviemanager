using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Common
{
    public class VideoTitleExtractor
    {
        //TODO 030 split delimiters in 2 groups --> 1 where the regexp is less strict --> doesn't have to be a non alphanumeric char before and after the delimiter, and the rest
        static readonly String[] DELIMITERS = { "CD-1", "CD-2", "CD1", "CD2", "DVD-1", "DVD-2", "[Divx-ITA]", "[XviD-ITA]", "AC3", "DVDRip", "Xvid", "http", "www.", ".com", "shared", "powered", "sponsored", "sharelive", "filedonkey", "saugstube", "eselfilme", "eseldownloads", "emulemovies", "spanishare", "eselpsychos.de", "saughilfe.de", "goldesel.6x.to", "freedivx.org", "elitedivx", "deviance", "-ftv", "ftv", "-flt", "flt", "1080p", "720p", "1080i", "720i", "480", "x264", "ext", "ac3", "6ch", "axxo", "pukka", "klaxxon", "edition", "limited", "dvdscr", "screener", "unrated", "BRRIP", "subs", "_NL_", "m-hd", "ts", "dvd rip", "hdtv" };
        static readonly String[] REMOVE_CHARS = { ".", "(", ")", "{", "}", "[", "]", "_", "-" };

        /// <summary>
        /// cleans up title, returns the cleaned up title
        /// </summary>
        /// <param name="fileName">original file name</param>
        /// <returns>cleaned up filename, should be closest to movie title with year</returns>
        public static String CleanTitle(String fileName)
        {
            string FileName = Path.GetFileNameWithoutExtension(fileName)??fileName;
            String MovieName = FileName.ToLower();
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
            var Guesses = new List<string>();

            //guesses based on filename
            string FileName = Path.GetFileNameWithoutExtension(videoPath);
            List<string> GuessesFromFileName = GetTitleGuessesFromText(FileName);
            
            //guesses based on foldername
            string FolderName = Path.GetDirectoryName(videoPath);
            List<string> GuessesFromFolderName = GetTitleGuessesFromText(FolderName);//TODO 030 only check foldername if its not a general folder (should only contain this moviefile, else it will be to general)

            //TODO 010 use all directories up untill folder where other videofiles are discovered (for videos who are 2 subfolders down from the mainfolder)
            
            //TODO 020 during analysis check all possible matches and choose the one with the best textual match to the original filename/foldername

            Guesses.Add(GuessesFromFileName[0]);
            Guesses.Add(GuessesFromFolderName[0]);
            if (GuessesFromFileName.Count > 1) Guesses.Add(GuessesFromFileName[1]);
            if (GuessesFromFolderName.Count > 1) Guesses.Add(GuessesFromFolderName[1]);

            //TODO 010 check if this improves accuracy (parts of title being searched)
            //if (GuessesFromFileName.Count > 2) Guesses.AddRange(GuessesFromFileName.GetRange(2, GuessesFromFileName.Count - 2));
            //if (GuessesFromFolderName.Count > 2) Guesses.AddRange(GuessesFromFolderName.GetRange(2, GuessesFromFolderName.Count - 2));
            
            return Guesses;
        }

        private static List<string> GetTitleGuessesFromText(string text)
        {
            var Guesses = new List<string>();

            string Guess1 = CleanTitle(text);
            //remove text after realistic release yeardate (1800-2200):
            int FirstIndex = Regex.Match(Guess1, "^.*[^0-9]((1[89]|2[012])[0-9][0-9])($|[^0-9].*$)").Groups[1].Index;
            if (FirstIndex > 0)
            {
                Guesses.Add(Guess1.Substring(0, FirstIndex));
            }
            Guesses.Add(Guess1);

            //current title guess but drop 1 word from the end untill only 1 word left --> all guesses
            int LastIndex = Guesses.Count - 1;
            string[] Words = Guess1.Split(' ');
            string BuildTitle = Words[0];
            for (int I = 1; I < Words.Length-1; I++)//length -1 cause full guess1 has already been added
            {
                BuildTitle += " " + Words[I];
                Guesses.Insert(LastIndex, BuildTitle);
            }
            return Guesses;
        } 

    }
}
