using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Documents;
using Common;
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

        public event VideoInfoFoundProgress VideoInfoProgress;

        public void OnVideoInfoProgress(ProgressEventArgs args)
        {
            VideoInfoFoundProgress Handler = VideoInfoProgress;
            if (Handler != null) Handler(this, args);
        }

        internal delegate void VideoInfoFoundProgress(object sender, ProgressEventArgs args);

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            int Counter = 0;
            foreach (var AnalyseVideo in _analyseVideos)
            {
                //Console.WriteLine(AnalyseVideo.Video.Name);
                AnalyseVideo.Candidates.Clear();
                foreach (var VideoInfo in SearchTMDB.GetVideoInfo(AnalyseVideo.Video.Name))
                {
                    AnalyseVideo.Candidates.Add(VideoInfo);
                }
                //set selected index
                if (AnalyseVideo.Candidates.Count > 1)
                {
                    AnalyseVideo.SelectedCandidateIndex = 0;
                    AnalyseVideo.MatchPercentage = 50;
                }else if(AnalyseVideo.Candidates.Count > 0)
                {
                    AnalyseVideo.SelectedCandidateIndex = 0;
                    AnalyseVideo.MatchPercentage = 100;
                }else
                {
                    AnalyseVideo.MatchPercentage = 0;
                }
                Counter++;
                OnVideoInfoProgress(new ProgressEventArgs(){MaxNumber = _analyseVideos.Count, ProgressNumber = Counter});
            }
        }
    }

}
