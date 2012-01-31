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
                List<Image> localImages = new List<Image>();
                foreach (string imageUrl in ImageUrls)
                {
                    try
                    {
                        Image image = new Image();

                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(imageUrl, UriKind.Absolute);
                        bitmap.EndInit();

                        image.Source = bitmap;
                        localImages.Add(image);
                    }
                    catch
                    {
                    }
                }
                return localImages;
            }
        }

        public List<ImageInfo> MovieImageUrls { get; set; }

        public String Birthplace { get; set; }

        public String Biography { get; set; }
    }
}
