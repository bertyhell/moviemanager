using System.Collections.Generic;
using System.ComponentModel;
using Model;

namespace SQLite
{
    public class BGWInsertVideos : BackgroundWorker
    {
        private readonly IList<Video> _videos;

        public BGWInsertVideos(IList<Video> videos)
        {
            _videos = videos;
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            MMDatabase.InsertVideosHDD(_videos);
        }
    }
}
