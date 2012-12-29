using System;
using System.Globalization;
using System.Windows.Data;

namespace Tmc.WinUI.Application.Converters
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
                return ((DateTime) value).ToLocalTime().ToString(CultureInfo.InvariantCulture);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
