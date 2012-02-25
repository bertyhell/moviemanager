using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class TimestampUtilities
    {
        public static string UlongToTimestampString(ulong timestamp)
        {
            ulong Seconds = timestamp / 1000;
            ulong Minutes = Seconds / 60;
            ulong Hours = (ulong)Math.Floor((double)(Minutes / 60));
            Minutes = Minutes - Hours * 60;
            Seconds = Seconds - Minutes * 60 - Hours * 60 * 60;
            return Hours + ":" + Minutes.ToString("D2") + ":" + Seconds.ToString("D2");
        }
    }
}
