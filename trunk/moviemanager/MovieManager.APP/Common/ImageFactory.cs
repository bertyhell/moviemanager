using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MovieManager.APP.Common
{
    public class ImageFactory
    {
        public static Image GetImage(Uri uri)
        {
            Image FinalImage = new Image();
            BitmapImage Logo = new BitmapImage();
            Logo.BeginInit();
            Logo.UriSource = uri;
            Logo.EndInit();
            FinalImage.Source = Logo;
            return FinalImage;
        }
    }
}
