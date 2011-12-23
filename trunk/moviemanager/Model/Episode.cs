using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class Episode : Video
    {
        private int _serieId;
        private int _season;
        private int _episodeNumber;
        private long _runtime;

        public Episode() { }

        public override VideoTypeEnum VideoType
        {
            get { return VideoTypeEnum.Episode; }
        }

        public int EpisodeNumber
        {
            get { return _episodeNumber; }
            set { _episodeNumber = value; }
        }

        public long Runtime
        {
            get { return _runtime; }
            set { _runtime = value; }
        }

        public int Season
        {
            get { return _season; }
            set { _season = value; }
        }

        public int SerieId
        {
            get { return _serieId; }
            set { _serieId = value; }
        }

    }
}
