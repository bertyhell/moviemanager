using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
                //TODO 070 split up in different analysing passes --> only reanalyse videos where no good match was found (or selected by user)

                string FileNameGuess = AnalyseVideo.TitleGuesses[0];
                string FolderNameGuess = AnalyseVideo.TitleGuesses.Count>1?AnalyseVideo.TitleGuesses[1]:null;

                //Console.WriteLine(AnalyseVideo.Video.Name);
                var Candidates = new SortedSet<Video>(new SimilarityComparer());//sort candidates by their match score with the original filename and foldername
                foreach (string TitleGuess in AnalyseVideo.TitleGuesses)//all title guesses
                {
                    foreach (var VideoInfo in SearchTMDB.GetVideoInfo(TitleGuess))//get multiple results for each guess
                    {
                        //add pairs of similarity and videoInfo to the list
                        //similarity == max of similarity between to the original guesses for filename and foldername and the videoinfo name from the webservice
                        List<double> Similarities = new List<double>();
                        Similarities.Add(StringSimilarity.GetSimilarity(VideoInfo.Name + " " + VideoInfo.Release.Year, FileNameGuess));
                        Similarities.Add(StringSimilarity.GetSimilarity(VideoInfo.Name, FileNameGuess));
                        if(FolderNameGuess != null)
                        {
                            Similarities.Add(StringSimilarity.GetSimilarity(VideoInfo.Name + " " + VideoInfo.Release.Year, FolderNameGuess));
                            Similarities.Add(StringSimilarity.GetSimilarity(VideoInfo.Name, FolderNameGuess));
                        }
                        VideoInfo.TitleMatchRatio = Similarities.Max();
                        if(!Candidates.Contains(VideoInfo)) Candidates.Add(VideoInfo);
                    }
                }
                //TODO 070 add option so user can disable info being changed for this video --> none of the videoInfo's from webservice are correct (maybe video isn't famous enough) --> shouldn't change all movieinfo --> abort analyse for this video
                AnalyseVideo.Candidates = Candidates.ToList();
                AnalyseVideo.AnalyseNeeded = false;
                Counter++;
                OnVideoInfoProgress(new ProgressEventArgs { MaxNumber = _analyseVideos.Count, ProgressNumber = Counter });
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
