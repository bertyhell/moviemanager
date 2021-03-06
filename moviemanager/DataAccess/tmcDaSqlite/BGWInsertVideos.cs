﻿using System.Collections.Generic;
using System.ComponentModel;
using Tmc.DataAccess.SqlCe;
using Tmc.SystemFrameworks.Model;

namespace Tmc.DataAccess.Sqlite
{
    public class BgwInsertVideos : BackgroundWorker
    {
        private readonly IList<Video> _videos;

        public BgwInsertVideos(IList<Video> videos)
        {
            _videos = videos;
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            DataRetriever.Videos = _videos;
        }
    }
}
