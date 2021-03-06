﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Windows.Media;
using Tmc.SystemFrameworks.Common;
using Tmc.WinUI.Application.Properties;

namespace Tmc.WinUI.Application.Cache
{
    public class ApplicationCache
    {
        private static string _cacheFolder;

        static ApplicationCache()
        {
            Init();
        }

        public static void Init()
        {
            var CacheFolder = Settings.Default.Cache_folder;
            Directory.CreateDirectory(CacheFolder);
            _cacheFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos), @"TheVideoCollector\Cache");//TODO 060 allow to change in settings

        }

        public static void AddVideoImages(int videoId, List<Uri> images, CacheImageType imageType, ImageQuality imageQuality)
        {
            if ((int)imageType >= (int)CacheImageType.PostersAndImages)
                throw new NotSupportedException("Combined cache image type not supported for setters!");

            string VideosDir = Path.Combine(_cacheFolder, videoId.ToString(CultureInfo.InvariantCulture), imageType.ToString(), imageQuality.ToString());

            Directory.CreateDirectory(VideosDir);
            foreach (Uri ImageUri in images)
            {
                AddVideoImage(videoId, ImageUri, imageType, imageQuality);
            }
        }

        public static Uri AddVideoImage(int videoId, Uri image, CacheImageType imageType, ImageQuality imageQuality)
        {
            try
            {
                if ((int)imageType >= (int)CacheImageType.PostersAndImages)
                    throw new NotSupportedException("Combined cache image type not supported for setters!");

                string VideosDir = Path.Combine(_cacheFolder, videoId.ToString(CultureInfo.InvariantCulture), imageType.ToString(), imageQuality.ToString());

                Directory.CreateDirectory(VideosDir);
                Stream ImageStream = WebRequest.Create(image).GetResponse().GetResponseStream();
                if (ImageStream != null)
                {
                    Bitmap Bmp = (Bitmap)Image.FromStream(ImageStream);
                    string FilePath = Path.Combine(VideosDir, Math.Abs(image.GetHashCode()) + ".jpg");

                    Bmp.Save(FilePath);
                    return new Uri(FilePath);
                }
	            return null;
            }
            catch (WebException Ex)
            {
	            if (Ex.Message.Contains("404") || Ex.Message.Contains("400"))//TODO 020 replace by regex   \(4[0-9]{2}\)
                {
                    //ignore exception
                    return null;
                }
	            throw;
            }
        }

        public static List<ImageSource> GetImages(int videoId, CacheImageType imageType)
        {
            var RetVal = new List<ImageSource>();

            string VideoDir = Path.Combine(_cacheFolder, videoId.ToString(CultureInfo.InvariantCulture), imageType.ToString(), Settings.Default.ImageQuality.ToString());

            foreach (string ImagePath in Directory.GetFiles(VideoDir))
            {
                ImageSourceConverter Converter = new ImageSourceConverter();
                RetVal.Add((ImageSource)Converter.ConvertTo(ImagePath, typeof(ImageSource)));
            }
            return RetVal;
        }

        public static Uri GetImage(int videoId, Uri imageUri, CacheImageType imageType)
        {
            string FilePath = Path.Combine(
                _cacheFolder,
                videoId.ToString(CultureInfo.InvariantCulture),
                imageType.ToString(),
                Settings.Default.ImageQuality.ToString(),
                Math.Abs(imageUri.GetHashCode()) + ".jpg");

            //TODO 030 check higher quality images
            //TODO 060 download image if not in cache
            //TODO make 1 get method

            if (!File.Exists(FilePath))
            {
                AddVideoImage(videoId, imageUri, imageType, Settings.Default.ImageQuality);
            }
            return new Uri(FilePath);

        }
    }

    public enum CacheImageType
    {
        Images = 0,
        Posters = 1,
        Portraits = 2,
        PostersAndImages = 3
    }

}
