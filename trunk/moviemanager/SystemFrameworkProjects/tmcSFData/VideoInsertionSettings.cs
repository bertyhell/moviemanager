using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Tmc.SystemFrameworks.Data
{
    public class VideoInsertionSettings
    {
        private List<String> _episodeFilterRegEx;
        private ulong _minimalVideoSize;
        private List<String> _videoFileExtensions;


        public VideoInsertionSettings()
        {
            Init();
        }
        /// <summary>
        /// We add this method because when an object is deserialized the constructor is not called
        /// </summary>
        [OnDeserializing]
        public void OnDeserializing(StreamingContext context)
        {
            Init();
        }

        private void Init()
        {
            _episodeFilterRegEx = new List<string>
            {
                @"[Ss](\d{1,2})[Ee](\d{1,2})",
                @"[^a-zA-Z0-9](\d{1,2})(\d{2})[^a-zA-Z0-9]"
            };
            _minimalVideoSize = 30000000; //Bytes
            _videoFileExtensions = new List<String> { "ASX", "DTS", "GXF", "M2V", "M3U", "M4V", "MPEG1", "MPEG2", "MTS", "MXF", "OGM", "BUP", "A52", "AAC", "B4S", "CUE", "DIVX", "DV", "FLV", "M1V", "M2TS", "MKV", "MOV", "MPEG4", "OMA", "SPX", "TS", "VLC", "VOB", "XSPF", "DAT", "BIN", "IFO", "PART", "3G2", "AVI", "MPEG", "MPG", "FLAC", "M4A", "MP1", "OGG", "WAV", "XM", "3GP", "WMV", "AC3", "ASF", "MOD", "MP2", "MP4", "WMA", "MKA", "M4P" };
            
        }

        public List<String> EpisodeFilterRegexs
        {
            get { return _episodeFilterRegEx; }
            set { _episodeFilterRegEx = value; }
        }

        public List<String> VideoFileExtensions
        {
            get { return _videoFileExtensions; }
            set { _videoFileExtensions = value; }
        } 

        public ulong MinimalVideoSize
        {
            get { return _minimalVideoSize; }
            set { _minimalVideoSize = value; }
        } 

    }
}
