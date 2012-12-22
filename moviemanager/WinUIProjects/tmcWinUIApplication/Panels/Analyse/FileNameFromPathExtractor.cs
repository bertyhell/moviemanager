using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace Tmc.WinUI.Application.Panels.Analyse
{
    class FileNameFromPathExtractor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string FilePath = value as string;
            if(FilePath != null)
            {
                return Path.GetFileName(FilePath);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
