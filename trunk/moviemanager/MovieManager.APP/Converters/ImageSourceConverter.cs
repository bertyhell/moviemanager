using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MovieManager.APP.Converters
{
    public class ImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return new Uri("/MovieManager;component/Images/no_image.png", UriKind.Relative);
            if (targetType == typeof(Uri) && value.GetType() == typeof(Uri))
            {

                return value;
                // Create source.
                //BitmapImage Bi = new BitmapImage();
                //// BitmapImage.UriSource must be in a BeginInit/EndInit block.
                //Bi.BeginInit();
                //Bi.UriSource = (Uri)value;
                //Bi.EndInit();
                //// Set the image source.
                //return Bi;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
