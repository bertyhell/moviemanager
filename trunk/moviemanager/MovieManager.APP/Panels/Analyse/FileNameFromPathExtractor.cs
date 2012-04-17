using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using Model;

namespace MovieManager.APP.Panels.Analyse
{
    class FileNameFromPathExtractor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is string)
            {
                return Path.GetFileName((string) value);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
