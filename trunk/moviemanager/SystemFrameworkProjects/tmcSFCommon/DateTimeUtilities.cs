using System;
using System.Globalization;

namespace Tmc.SystemFrameworks.Common
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
					return new DateTime(1800, 1, 1);
                }
            }
            return Datetime;
        }

        public static DateTime ParseDate(String date, String stringFormat)
        {
	        if (date == null)
	        {
		        return new DateTime(1800, 1, 1);
	        }
	        else
	        {
		        DateTimeFormatInfo Format = new DateTimeFormatInfo();
		        DateTime Datetime;
		        try
		        {
			        Format.FullDateTimePattern = stringFormat;
			        Datetime = DateTime.Parse(date, Format);
		        }
		        catch
		        {
			        return new DateTime(1800, 1, 1);
		        }
		        return Datetime;
	        }
        }
    }
}
