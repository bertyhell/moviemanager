﻿using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using Model;

namespace MovieManager.APP.Panels.Analyse
{
    class FolderFromPathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string FilePath = value as string;
            if (FilePath != null)
            {
                return Path.GetFileName(Path.GetDirectoryName(FilePath));
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
