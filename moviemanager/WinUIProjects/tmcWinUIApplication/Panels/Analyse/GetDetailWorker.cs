﻿using System.Collections.Generic;
using System.ComponentModel;
using Tmc.BusinessRules.Web.Search;
using Tmc.SystemFrameworks.Common;
using Tmc.SystemFrameworks.Model;

namespace Tmc.WinUI.Application.Panels.Analyse
{
    class GetDetailWorker : BackgroundWorker
    {
        private readonly IList<Video> _videos;

        public GetDetailWorker(IList<Video> videos)
        {
            _videos = videos;
        }

        public GetDetailWorker(Video video)
        {
            _videos = new List<Video> { video };
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
            for (int I = 0; I < _videos.Count; I++)
            {
                Video Video = _videos[I];
                if (Video.VideoType ==VideoTypeEnum.Movie)
                {
                    SearchTmdb.GetExtraMovieInfo(Video);
                    SearchTmdb.GetMovieImages(Video);
                    Video.AnalyseCompleted = true;
                }
                OnVideoInfoProgress(new ProgressEventArgs { MaxNumber = _videos.Count, ProgressNumber = I });
            }
        }
    }

}
