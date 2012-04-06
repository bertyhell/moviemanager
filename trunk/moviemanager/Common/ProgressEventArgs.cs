using System;

namespace Common
{
    public class ProgressEventArgs : EventArgs
    {
        public int ProgressNumber { get; set; }
        public int MaxNumber { get; set; }
    }
}