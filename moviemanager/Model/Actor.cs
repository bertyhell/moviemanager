using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Model
{
    public class Actor
    {

        private int _tmdbID;
        private String _name;
        private List<String> _imageURLs;
        private List<String> _movieURLs;

        public Actor()
        {
            _imageURLs = new List<String>();
            _movieURLs = new List<String>();
        }

        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int TmdbID
        {
            get { return _tmdbID; }
            set { _tmdbID = value; }
        }

        public void addImage(String URL)
        {
            _imageURLs.Add(URL);
        }

        public List<String> ImageUrls
        {
            get { return _imageURLs; }
            set { _imageURLs = value; }
        }

        public List<Image> Images
        {
            get
            {
                List<Image> images = new List<Image>();
                for (int i = 0; i < _imageURLs.Count; i++)
                {
                    try
                    {
                        Image image = new Image();

                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(_imageURLs[i], UriKind.Absolute);
                        bitmap.EndInit();

                        image.Source = bitmap;
                        images.Add(image);
                    }
                    catch
                    {
                    }
                }
                return images;
            }
        }

        public List<String> MovieImageUrls
        {
            get { return _movieURLs; }
            set { _movieURLs = value; }
        }
    }
}
