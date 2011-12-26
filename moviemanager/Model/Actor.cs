﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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
                    catch
                    {
                    }
                }
                return LocalImages;
            }
        }

        public List<ImageInfo> MovieImageUrls { get; set; }

        public String Birthplace { get; set; }

        public String Biography { get; set; }
    }
}
