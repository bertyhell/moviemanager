using System;
using System.Collections.Generic;

namespace MovieManager.Common
{
    public class StringSimilarity
    {
        public static double GetSimilarity(string firstString, string secondString)
        {
            if (firstString == null)
            {
                throw new ArgumentNullException("firstString");
            }
            if (secondString == null)
            {
                throw new ArgumentNullException("secondString");
            }

            if (firstString == secondString)
            {
                return 1;
            }

            int LongestLenght = Math.Max(firstString.Length, secondString.Length);
            int Distance = GetLevensteinDistance(firstString, secondString);
            double Percent = Distance / Convert.ToDouble(LongestLenght);
            return 1 - Percent;
        }

        public static string GetBestMatch(string s1, List<string> list)
        {
            string BestMatch = list[0];
            double BestSimilarity = 0;
            foreach (string S2 in list)
            {
                double Similarity = GetSimilarity(s1, S2);
                if (Similarity > BestSimilarity)
                {
                    BestMatch = S2;
                    BestSimilarity = Similarity;
                }
            }
            return BestMatch;
        }

        private static int GetLevensteinDistance(string firstString, string secondString)
        {
            if (firstString == null)
            {
                throw new ArgumentNullException("firstString");
            }
            if (secondString == null)
            {
                throw new ArgumentNullException("secondString");
            }

            if (firstString == secondString)
            {
                return 0;
            }

            int[,] Matrix = new int[firstString.Length + 1, secondString.Length + 1];

            for (int I = 0; I <= firstString.Length; I++)
            {
                Matrix[I, 0] = I;
            }
            // deletion
            for (int J = 0; J <= secondString.Length; J++)
            {
                Matrix[0, J] = J;
            }
            // insertion
            for (int I = 0; I <= firstString.Length - 1; I++)
            {
                for (int j = 0; j <= secondString.Length - 1; j++)
                {
                    if (firstString[I] == secondString[j])
                    {
                        Matrix[I + 1, j + 1] = Matrix[I, j];
                    }
                    else
                    {
                        Matrix[I + 1, j + 1] = Math.Min(Matrix[I, j + 1] + 1, Matrix[I + 1, j] + 1);
                        //deletion or insertion
                        //substitution
                        Matrix[I + 1, j + 1] = Math.Min(Matrix[I + 1, j + 1], Matrix[I, j] + 1);
                    }
                }
            }
            return Matrix[firstString.Length, secondString.Length];
        }
    }
}
