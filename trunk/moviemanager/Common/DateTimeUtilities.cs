using System;
using System.Globalization;

namespace Common
{
    public class DateTimeUtilities
    {
        public static DateTime ParseDate(String date)
        {
            DateTimeFormatInfo format = new DateTimeFormatInfo();
            DateTime datetime;
            try
            {
                format.FullDateTimePattern = "d MMM yyyy";

                datetime = DateTime.Parse(date, format);
            }
            catch
            {
                try
                {
                    format.FullDateTimePattern = "yyyy";
                    datetime = DateTime.Parse(date, format);
                }
                catch
                {
                    return DateTime.MinValue;
                }
            }
            return datetime;
        }

        public static DateTime ParseDate(String date, String stringFormat)
        {
            DateTimeFormatInfo format = new DateTimeFormatInfo();
            DateTime datetime;
            try
            {
                format.FullDateTimePattern = stringFormat;

                datetime = DateTime.Parse(date, format);
            }
            catch
            {
                return DateTime.MinValue;
            }
            return datetime;
        }
    }
}
