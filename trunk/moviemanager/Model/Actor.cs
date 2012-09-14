using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Model.Interfaces;
using MovieManager.LOG;

namespace Model
{
    public class Actor : IPreviewInfoRetriever
    {
        public Actor()
        {
            ImageUrls = new List<String>();
            MovieImageUrls = new List<ImageInfo>();
        }

        public ImageInfo Thumbnail
        {
            get
            {
                if (ImageUrls.Count > 0)
                    return new ImageInfo{Uri = new Uri(ImageUrls[0])};
                return null;
            }
            set { throw new NotImplementedException(); }
        }

        public Uri Poster
        {
            get { return new Uri(ImageUrls[0]); }
            set{}
        }

        public DateTime Year
        {
            get { return Birthday; }
            set { Birthday = value; }
        }
        public string Name { get; set; }

        public int TmdbID { get; set; }

        public void AddImage(String url)
        {
            ImageUrls.Add(url);
        }

        public List<string> ImageUrls { get; set; }

        public List<Image> Images
        {
            get
            {
                List<Image> LocalImages = new List<Image>();
                foreach (string ImageUrl in ImageUrls)
                {
                    try
                    {
                        Image Image = new Image();

                        BitmapImage Bitmap = new BitmapImage();
                        Bitmap.BeginInit();
                        Bitmap.UriSource = new Uri(ImageUrl, UriKind.Absolute);
                        Bitmap.EndInit();

                        Image.Source = Bitmap;
                        LocalImages.Add(Image);
                    }
                    catch (Exception Ex)
                    {
                        GlobalLogger.Instance.MovieManagerLogger.Error(GlobalLogger.FormatExceptionForLog("SettingsPanelBase", "SaveAllSettings", Ex.Message));
                    }
                }
                return LocalImages;
            }
        }

        public List<ImageInfo> MovieImageUrls { get; set; }

        public String Birthplace { get; set; }

        public String Biography { get; set; }

        public DateTime Birthday { get; set; }

    }
}
