﻿using System.Collections.Generic;
using System.ComponentModel;
using Model;

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
            TmcDatabase.InsertVideosHdd(_videos);
        }
    }
}