

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace MovieManager.APP.Common
{
    class Int2ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(Brush) && value is int)
            {
                int Percent = (int)value;
                if (Percent < 0 || Percent > 100)
                {
                    return null;
                }
                else if (Percent < 33)
                {
                    return Brushes.LightCoral;
                }else if(Percent < 66)
                {
                    return Brushes.LightSalmon;
                }else
                {
                    return Brushes.LightGreen;
                }
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
