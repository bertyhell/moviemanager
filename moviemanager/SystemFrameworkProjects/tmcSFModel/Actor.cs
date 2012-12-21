using System;
using System.Collections.Generic;
using Model.Interfaces;

namespace Model
{
    public class Actor : IPreviewInfoRetriever
    {
        public Actor()
        {
            Images = new List<ImageInfo>();
            MovieImageUrls = new List<ImageInfo>();
        }

        public uint Id { get; set; }

        public ImageInfo Thumbnail
        {
            get
            {
                if (Images.Count > 0)
                {
                    return Images[0];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (Images.Count > 0)
                {
                    Images[0] = value;
                }
                else
                {
                    Images.Add(value);
                }
            }
        }

        public ImageInfo Poster
        {
            get
            {
                if (Images.Count > 0)
                {
                    return Images[0];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (Images.Count > 0)
                {
                    Images[0] = value;
                }
                else
                {
                    Images.Add(value);
                }
            }
        }

        public DateTime Year
        {
            get { return Birthday; }
            set { Birthday = value; }
        }
        public string Name { get; set; }

        public int TmdbId { get; set; }

        public void AddImage(Uri imageUri)
        {
            Images.Add(new ImageInfo { Uri = imageUri });
        }

        public void AddImage(ImageInfo imageInfo)
        {
            Images.Add(imageInfo);
        }

        public List<ImageInfo> Images { get; set; }

        public List<ImageInfo> MovieImageUrls { get; set; }

        public String Birthplace { get; set; }

        public String Biography { get; set; }

        public DateTime Birthday { get; set; }

    }
}
