using System;
using System.Net;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Tmc.SystemFrameworks.Common
{
    public class ImageFactory
    {
        public static Image GetImage(Uri uri)
        {
            Image FinalImage = new Image();

            BitmapImage Source = new BitmapImage();
            Source.BeginInit();
            Source.UriSource = uri;
            Source.EndInit();
            FinalImage.Source = Source;

            return FinalImage;
        }

        public static ImageSource GetImageSource(Uri uri)
        {

            BitmapImage ImageSource = new BitmapImage();

            ImageSource.BeginInit();
            ImageSource.UriSource = uri;
            ImageSource.EndInit();

            return ImageSource;
        }

        public static void SaveImageToHdd(string url, string pathOnHdd)
        {
            WebClient Client = new WebClient();
            Client.DownloadFile(url,pathOnHdd);
        }
    }
}
