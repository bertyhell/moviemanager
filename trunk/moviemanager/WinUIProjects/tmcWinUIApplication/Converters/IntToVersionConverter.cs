using System;
using System.Globalization;
using System.Windows.Data;

namespace Tmc.WinUI.Application.Converters
{
    class IntToVersionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(targetType == typeof(string))
            {
                return ((int) value).ToString("000");
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
