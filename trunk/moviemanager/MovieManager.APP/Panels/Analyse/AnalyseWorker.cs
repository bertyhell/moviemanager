using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
            _analyseVideos = new List<AnalyseVideo> { analyseVideo };
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
            foreach (AnalyseVideo AnalyseVideo in _analyseVideos)
            {
                string FileNameGuess = AnalyseVideo.TitleGuesses[0];
                string FolderNameGuess = AnalyseVideo.TitleGuesses[1];

                //Console.WriteLine(AnalyseVideo.Video.Name);
                var Candidates = new SortedSet<Video>(new SimilarityComparer());//sort candidates by their match score with the original filename and foldername
                foreach (string TitleGuess in AnalyseVideo.TitleGuesses)//all title guesses
                {
                    foreach (var VideoInfo in SearchTMDB.GetVideoInfo(TitleGuess))//get multiple results for each guess
                    {
                        //add pairs of similarity and videoInfo to the list
                        //similarity == max of similarity between to the original guesses for filename and foldername and the videoinfo name from the webservice
                        VideoInfo.TitleMatchRatio = Math.Max(StringSimilarity.GetSimilarity(VideoInfo.Name, FileNameGuess), StringSimilarity.GetSimilarity(VideoInfo.Name, FolderNameGuess));
                        Candidates.Add(VideoInfo);//TODO 080 avoid duplicates (tried with equatable<> interface and sortedset --> doesn't work yet)
                    }
                }
                var UniqueSortedCandidates = new List<Video>();
                //TODO 070 add option so user can disable info being changed for this video --> none of the videoInfo's from webservice are correct (maybe video isn't famous enough) --> shouldn't change all movieinfo --> abort analyse for this video
                foreach (Video Candidate in Candidates)
                {
                    UniqueSortedCandidates.Add(Candidate);
                }
                AnalyseVideo.Candidates = UniqueSortedCandidates;
                //set selected index
                if (AnalyseVideo.Candidates.Count > 1)
                {
                    AnalyseVideo.SelectedCandidateIndex = 0;
                    AnalyseVideo.MatchPercentage = 33;
                }
                else if (AnalyseVideo.Candidates.Count > 0)
                {
                    AnalyseVideo.SelectedCandidateIndex = 0;
                    AnalyseVideo.MatchPercentage = 100;
                }
                else
                {
                    AnalyseVideo.MatchPercentage = 0;
                }
                AnalyseVideo.AnalyseNeeded = false;
                Counter++;
                OnVideoInfoProgress(new ProgressEventArgs() { MaxNumber = _analyseVideos.Count, ProgressNumber = Counter });
            }
        }
    }

    class SimilarityComparer : IComparer<Video>
    {
        public int Compare(Video x, Video y)
        {
            return x.TitleMatchRatio < y.TitleMatchRatio ? 1 : -1;//doesn't matter what happens to x.key == y.key
        }
    }

}
