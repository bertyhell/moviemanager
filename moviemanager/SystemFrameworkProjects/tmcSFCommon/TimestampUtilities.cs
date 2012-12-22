namespace Tmc.SystemFrameworks.Common
{
    public class TimestampUtilities
    {
        public static string LongToTimestampString(long timestamp)
        {
            long Seconds = timestamp / 1000;
            long Minutes = Seconds / 60;
            long Hours = Minutes / 60;
            Minutes = Minutes - Hours * 60;
            Seconds = Seconds - Minutes * 60 - Hours * 60 * 60;
            return Hours + ":" + Minutes.ToString("D2") + ":" + Seconds.ToString("D2");
        }
    }
}
