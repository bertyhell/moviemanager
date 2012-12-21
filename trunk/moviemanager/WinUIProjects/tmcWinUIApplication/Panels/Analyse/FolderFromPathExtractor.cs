using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using Model;

namespace MovieManager.APP.Panels.Analyse
{
    class FolderFromPathExtractor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                return Path.GetFileName(Path.GetDirectoryName((string)value));
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
