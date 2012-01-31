using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Common
{
    public class ImageFactory
    {
        public static Image GetImage(Uri uri)
        {
            Image finalImage = new Image();

            BitmapImage source = new BitmapImage();
            source.BeginInit();
            source.UriSource = uri;
            source.EndInit();
            finalImage.Source = source;

            return finalImage;
        }

        public static ImageSource GetImageSource(Uri uri)
        {

            BitmapImage imageSource = new BitmapImage();

            imageSource.BeginInit();
            imageSource.UriSource = uri;
            imageSource.EndInit();

            return imageSource;
        }
    }
}
