using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace MovieManager.APP.Converters
{
    /// <summary>
    /// Converts UTC to localTime
    /// </summary>
    class TimestampConverter :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if( targetType == typeof(string))
            {
                return ((DateTime) value).ToLocalTime().ToString();
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
