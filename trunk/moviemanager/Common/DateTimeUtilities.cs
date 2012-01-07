using System;
using System.Globalization;

namespace Common
{
    public class DateTimeUtilities
    {
        public static DateTime ParseDate(String date)
        {
            DateTimeFormatInfo Format = new DateTimeFormatInfo();
            DateTime Datetime;
            try
            {
                Format.FullDateTimePattern = "d MMM yyyy";

                Datetime = DateTime.Parse(date, Format);
            }
            catch
            {
                try
                {
                    Format.FullDateTimePattern = "yyyy";
                    Datetime = DateTime.Parse(date, Format);
                }
                catch
                {
                    return DateTime.MinValue;
                }
            }
            return Datetime;
        }
    }
}
