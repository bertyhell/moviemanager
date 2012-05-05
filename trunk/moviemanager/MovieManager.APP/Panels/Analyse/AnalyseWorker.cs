using System.Collections.Generic;
using System.ComponentModel;
using MovieManager.WEB.Search;

namespace MovieManager.APP.Panels.Analyse
{
    class AnalyseWorker : BackgroundWorker
    {
        private readonly IList<AnalyseVideo> _analyseVideos;

        public AnalyseWorker(IList<AnalyseVideo> analyseVideos)
        {
            _analyseVideos = analyseVideos;
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            foreach (var AnalyseVideo in _analyseVideos)
            {
                foreach (var VideoInfo in SearchTMDB.GetVideoInfo(AnalyseVideo.Video.Name))
                {
                    AnalyseVideo.Candidates.Add(VideoInfo);
                }
            }
        }
    }
}
