using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Documents;
using Common;
using Model;
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

        public AnalyseWorker(AnalyseVideo analyseVideo)
        {
            _analyseVideos=new List<AnalyseVideo>{analyseVideo};
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
                var Candidates = new ObservableCollection<Video>();
                foreach (var VideoInfo in SearchTMDB.GetVideoInfo(AnalyseVideo.SearchString))
                {
                    Candidates.Add(VideoInfo);
                }
                AnalyseVideo.Candidates = Candidates;
                //set selected index
                if (AnalyseVideo.Candidates.Count > 1)
                {
                    AnalyseVideo.SelectedCandidateIndex = 0;
                    AnalyseVideo.MatchPercentage = 33;
                }else if(AnalyseVideo.Candidates.Count > 0)
                {
                    AnalyseVideo.SelectedCandidateIndex = 0;
                    AnalyseVideo.MatchPercentage = 100;
                }else
                {
                    AnalyseVideo.MatchPercentage = 0;
                }
                AnalyseVideo.AnalyseNeeded = false;
                Counter++;
                OnVideoInfoProgress(new ProgressEventArgs(){MaxNumber = _analyseVideos.Count, ProgressNumber = Counter});
            }
        }
    }

}
