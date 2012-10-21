using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Model;
using Model.Interfaces;
using MovieManager.APP.Cache;
using Image = System.Windows.Controls.Image;

namespace MovieManager.APP.Converters
{
    public class AppImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {

                if (targetType == typeof(ImageSource) && value is IPreviewInfoRetriever)
                {
                    IPreviewInfoRetriever PreviewItem = (IPreviewInfoRetriever)value;
                    if (PreviewItem.Poster != null)
                    {
                        Uri LocalImageUri = ApplicationCache.GetImage(PreviewItem.Id, PreviewItem.Poster.Uri, CacheImageType.Posters);
                        if (LocalImageUri == null || !File.Exists(LocalImageUri.AbsolutePath))
                        {
                            //show empty picture
                            CreateBitmapImage(new Uri("C:/MMproject/MovieManager.APP/Images/no_image.png"));
                        }
                        return CreateBitmapImage(LocalImageUri);
                    }
                }
                if (targetType == typeof(Uri) && value.GetType() == typeof(Uri))
                {
                    return value;
                }
            }
            return CreateBitmapImage(new Uri("C:/MMproject/MovieManager.APP/Images/no_image.png"));
        }

        private static BitmapImage CreateBitmapImage(Uri localImageUrl)
        {
            BitmapImage ImagePosterSource;
            try
            {
                ImagePosterSource = new BitmapImage();
                ImagePosterSource.BeginInit();
                ImagePosterSource.UriSource = localImageUrl;

                // To save significant application memory, set the DecodePixelWidth or  
                // DecodePixelHeight of the BitmapImage value of the image source to the desired 
                // height or width of the rendered image. If you don't do this, the application will 
                // cache the image as though it were rendered as its normal size rather then just 
                // the size that is displayed.
                // Note: In order to preserve aspect ratio, set DecodePixelWidth
                // or DecodePixelHeight but not both.
                ImagePosterSource.DecodePixelWidth = 200;
                //TODO 050 link decoding width to width of previewitem with scale factor

                ImagePosterSource.EndInit();
                //set image source
            }
            catch (FileNotFoundException Ex)
            {
                //TODO 050 check why this sometimes happens (maybe 400 or 404 errors?)
                return CreateBitmapImage(new Uri("C:/MMproject/MovieManager.APP/Images/no_image.png"));
            }
            return ImagePosterSource;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
