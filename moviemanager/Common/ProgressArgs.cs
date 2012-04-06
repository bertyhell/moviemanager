using System;

namespace Common
{
    public class ProgressArgs : EventArgs
    {
        public int ProgressNumber { get; set; }
        public int MaxNumber { get; set; }
    }
}