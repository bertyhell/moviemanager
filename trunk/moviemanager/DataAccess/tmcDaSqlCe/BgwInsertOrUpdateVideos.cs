using System.Collections.Generic;
using System.ComponentModel;
using Tmc.SystemFrameworks.Model;

namespace Tmc.DataAccess.SqlCe
{
    public class BgwInsertOrUpdateVideos : BackgroundWorker
    {
        private readonly IList<Video> _videos;

        public BgwInsertOrUpdateVideos(IList<Video> videos)
        {
            _videos = videos;
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            DataRetriever.Videos = _videos;
        }
    }
}
