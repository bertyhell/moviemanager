using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Model
{
    public class Actor
    {
        public Actor()
        {
            ImageUrls = new List<String>();
            MovieImageUrls= new List<ImageInfo>();
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
                    catch (Exception E)
                    {
                        //TODO 080 uncomment and fix missing dll errors
                        //GlobalLogger.Instance.MovieManagerLogger.Error(GlobalLogger.FormatExceptionForLog("SettingsPanelBase", "SaveAllSettings", Ex.Message));
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
