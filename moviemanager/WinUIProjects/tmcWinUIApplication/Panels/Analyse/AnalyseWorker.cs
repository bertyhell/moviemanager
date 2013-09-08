using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Bing;
using Tmc.BusinessRules.Web.Search;
using Tmc.SystemFrameworks.Common;
using Tmc.SystemFrameworks.Model;

namespace Tmc.WinUI.Application.Panels.Analyse
{
    class AnalyseWorker : BackgroundWorker
    {
        private readonly IList<AnalyseVideo> _analyseVideos;
        private readonly bool _fullAnalyse;

        public AnalyseWorker(IList<AnalyseVideo> analyseVideos, bool fullAnalyse)
        {
            _analyseVideos = analyseVideos;
            _fullAnalyse = fullAnalyse;
        }

        public AnalyseWorker(AnalyseVideo analyseVideo)
        {
            _analyseVideos = new List<AnalyseVideo> { analyseVideo };
        }

        public event VideoInfoFoundProgress TotalProgress;

        public void OnTotalProgress(ProgressEventArgs args)
        {
            VideoInfoFoundProgress Handler = TotalProgress;
            if (Handler != null) Handler(this, args);
        }

        public event VideoInfoFoundProgress PassProgress;

        public void OnPassProgress(ProgressEventArgs args)
        {
            VideoInfoFoundProgress Handler = PassProgress;
            if (Handler != null) Handler(this, args);
        }

        internal delegate void VideoInfoFoundProgress(object sender, ProgressEventArgs args);

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            //TODO 005 try to remove some duplicate code from this method
            //TODO 040 give a bonus similarity score to the videoInfo that matches the release year in the filename

            int TotalProgressCounter = 0;
            int PassProgressCounter = 0;
            int Passes = 1;
            if (_fullAnalyse) Passes = 3;
            
            //pass1 (filename / foldername)
            foreach (AnalyseVideo AnalyseVideo in _analyseVideos)
            {
                //TODO 070 split up in different analysing passes --> only reanalyse videos where no good match was found (or selected by user)

                string FileNameGuess = AnalyseVideo.TitleGuesses[0];
                string FolderNameGuess = AnalyseVideo.TitleGuesses.Count > 1 ? AnalyseVideo.TitleGuesses[1] : null;

                AnalyseVideo.Candidates = new UniqueCollection<Video>();
                FillCandidates(AnalyseVideo, FileNameGuess, FolderNameGuess);

                TotalProgressCounter++;
                PassProgressCounter++;
                OnTotalProgress(new ProgressEventArgs { MaxNumber = _analyseVideos.Count * Passes, ProgressNumber = TotalProgressCounter, Message = "Pass 1/" + Passes });
                OnPassProgress(new ProgressEventArgs { MaxNumber = _analyseVideos.Count, ProgressNumber = PassProgressCounter });
            }

            PassProgressCounter = 0;
            OnPassProgress(new ProgressEventArgs { MaxNumber = _analyseVideos.Count, ProgressNumber = PassProgressCounter });

            if (_fullAnalyse)
            {
                //pass2 (remove prefix / suffixes)
                foreach (AnalyseVideo AnalyseVideo in _analyseVideos)
                {
                    if (AnalyseVideo.Candidates.Count == 0 || AnalyseVideo.MatchPercentage < 0.5)
                    {
                        string FileNameGuess = AnalyseVideo.TitleGuesses[0];
                        string FolderNameGuess = AnalyseVideo.TitleGuesses.Count > 1 ? AnalyseVideo.TitleGuesses[1] : null;

                        string FileName = Path.GetFileNameWithoutExtension(AnalyseVideo.Video.Files[0].Path);
                        if (FileName != null)
                        {
                            List<string> FileNameGuesses = VideoTitleExtractor.GetTitleGuessesFromString(FileName.ToLower(), true);
                            AnalyseVideo.TitleGuesses.AddRange(FileNameGuesses);
                        }
                        var DirectoryName = Path.GetDirectoryName(AnalyseVideo.Video.Files[0].Path);
                        if (DirectoryName != null)
                        {
                            string FolderName = DirectoryName.Split(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar).Last().ToLower();
                            List<string> FolderNameGuesses = VideoTitleExtractor.GetTitleGuessesFromString(FolderName, true);
                            AnalyseVideo.TitleGuesses.AddRange(FolderNameGuesses);
                        }

                        FillCandidates(AnalyseVideo, FileNameGuess, FolderNameGuess);
                    }
                    TotalProgressCounter++;
                    PassProgressCounter++;
                    OnTotalProgress(new ProgressEventArgs { MaxNumber = _analyseVideos.Count * Passes, ProgressNumber = TotalProgressCounter, Message = "Pass 2/" + Passes });
                    OnPassProgress(new ProgressEventArgs {MaxNumber = _analyseVideos.Count, ProgressNumber = PassProgressCounter});
                }

                PassProgressCounter = 0;
                OnPassProgress(new ProgressEventArgs {MaxNumber = _analyseVideos.Count, ProgressNumber = PassProgressCounter});

                //pass3 (websearch)
                foreach (AnalyseVideo AnalyseVideo in _analyseVideos)
                {
                    if (AnalyseVideo.Candidates.Count == 0 || AnalyseVideo.MatchPercentage < 0.5)
                    {

                        AnalyseVideo.TitleGuesses = VideoTitleExtractor.GetTitleGuessesFromPath(AnalyseVideo.Video.Files[0].Path); //TODO 004 optimize this --> also gets done in pass1 --> remember somehow
                        string FileNameGuess = AnalyseVideo.TitleGuesses[0];
                        string FolderNameGuess = AnalyseVideo.TitleGuesses.Count > 1 ? AnalyseVideo.TitleGuesses[1] : null;

                        AnalyseVideo.TitleGuesses.Clear();
                        foreach (string SearchResult in BingSearch.Search(FileNameGuess))
                        {
                            AnalyseVideo.TitleGuesses.Add(VideoTitleExtractor.CleanTitle(SearchResult));
                        }

                        if (FolderNameGuess != null)
                        {
                            foreach (string SearchResult in BingSearch.Search(FolderNameGuess))
                            {
                                AnalyseVideo.TitleGuesses.Add(VideoTitleExtractor.CleanTitle(SearchResult));
                            }
                        }

                        FillCandidates(AnalyseVideo, FileNameGuess, FolderNameGuess);
                    }
                    TotalProgressCounter++;
                    PassProgressCounter++;
                    OnTotalProgress(new ProgressEventArgs { MaxNumber = _analyseVideos.Count * Passes, ProgressNumber = TotalProgressCounter, Message = "Pass 3/" + Passes });
                    OnPassProgress(new ProgressEventArgs {MaxNumber = _analyseVideos.Count, ProgressNumber = PassProgressCounter});
                }
            }
        }

        private void FillCandidates(AnalyseVideo analyseVideo, string fileNameGuess, string folderNameGuess)
        {
            var Candidates = new SortedSet<Video>(new SimilarityComparer()); //sort candidates by their match score with the original filename and foldername
            foreach (Video VideoInfo in GetVideoInfos(analyseVideo, fileNameGuess))
            {
                Candidates.Add(VideoInfo);
            }
            if (folderNameGuess != null)
            {
                foreach (Video VideoInfo in GetVideoInfos(analyseVideo, folderNameGuess))
                {
                    Candidates.Add(VideoInfo);
                }
            }
            foreach (Video OldCandidate in analyseVideo.Candidates)
            {
                Candidates.Add(OldCandidate);
            }

            //TODO 070 add option so user can disable info being changed for this video --> none of the videoInfo's from webservice are correct (maybe video isn't famous enough) --> shouldn't change all movieinfo --> abort analyse for this video

            analyseVideo.Candidates = Candidates.ToList();
            analyseVideo.AnalyseNeeded = false;
        }

        private IEnumerable<Video> GetVideoInfos(AnalyseVideo analyseVideo, string referenceName)
        {
            var Candidates = new SortedSet<Video>(new SimilarityComparer()); //sort candidates by their match score with the original filename and foldername
            foreach (string TitleGuess in analyseVideo.TitleGuesses) //all title guesses
            {
                foreach (var VideoInfo in SearchTmdb.GetVideoInfo(TitleGuess)) //get multiple results for each guess
                {
                    //add pairs of similarity and videoInfo to the list
                    //similarity == max of similarity between to the original guesses for filename and foldername and the videoinfo name from the webservice
                    List<double> Similarities = new List<double>
                                {
                                    StringSimilarity.GetSimilarity(VideoInfo.Name + " " + VideoInfo.Release.Year, referenceName), 
                                    StringSimilarity.GetSimilarity(VideoInfo.Name, referenceName)
                                    //TODO 005 give bonus to videoinfo where eg: "men in black" --> original file name contains: mib (first letters of every word)
                                };
                    VideoInfo.TitleMatchRatio = Similarities.Max();
					if (!Candidates.Contains(VideoInfo))
					{
						//VideoInfo.Files = analyseVideo.Video.Files;
						Candidates.Add(VideoInfo);
					}
                }
            }
            return Candidates.ToList();
        }
    }

    class SimilarityComparer : IComparer<Video>
    {
        public int Compare(Video x, Video y)
        {
            return (x.IdImdb == y.IdImdb || x.Name == y.Name && x.Release == y.Release) ? 0 : x.TitleMatchRatio < y.TitleMatchRatio ? 1 : -1;
        }
    }
}
