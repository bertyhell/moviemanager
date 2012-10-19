using System;

namespace MovieManager.Common
{
    public class ProgressEventArgs : EventArgs
    {
        public int ProgressNumber { get; set; }
        public int MaxNumber { get; set; }
        public String Message { get; set; }
    }
}