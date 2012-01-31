using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using MovieManager.BL.Search;
using Model;

namespace MovieManager.APP.Panels.Search
{
    /// <summary>
    /// Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow
    {
        private ActorOverview _overview;

        public SearchWindow()
        {
            InitializeComponent();
        }

        private void SearchControlClickOnSearch(SearchEventArgs e)
        {
            Search(e.SearchOptions);
        }

        private void Search(SearchOptions options)
        {
            if (options.SearchForActors)
            {
                List<Actor> searchedActors = SearchTMDB.SearchActor(options.SearchTerm);
                if (searchedActors.Count > 1)
                {
                    //TODO 090: Make selection window
                    SearchForActor(searchedActors[0]);
                }
                else if (searchedActors.Count == 1)
                {
                    SearchForActor(searchedActors[0]);
                }
                else
                {
                    MessageBox.Show(Localization.Resource.NoResultsFound, Localization.Resource.NoActorsWereFound,
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }

            if (options.SearchForMovies)
            {
                Movie movie;
                try
                {
                    int queryIsId = Convert.ToInt32(options.SearchTerm);
                    movie = new Movie();
                    SearchTMDB.GetExtraMovieInfo(queryIsId, movie);
                }
                catch
                {
                    List<Movie> movies = SearchTMDB.GetVideoInfo(options.SearchTerm);
                    //TODO 090: Make selection window
                    movie = movies.Count > 0 ? movies[0] : null;
                }
                SearchForMovie(movie);
            }
        }

        private void SearchForActor(Actor actor)
        {
            ResetGrid();
            SearchTMDB.GetActorInfo(actor);

            if (_overview == null)
            {
                _overview = new ActorOverview();
                _overview.SearchEvent += SearchControlClickOnSearch;
                Grid.SetRow(_overview, 1);
            }
            _overview.Actor = actor;
            _layoutRoot.Children.Add(_overview);
        }

        private void SearchForMovie(Movie movie)
        {
            ResetGrid();
            MovieOverview overview = new MovieOverview();
            overview.Movie = movie;
            Grid.SetRow(overview, 1);
            _layoutRoot.Children.Add(overview);
        }

        private void ResetGrid()
        {
            _layoutRoot.Children.Clear();
            _layoutRoot.Children.Add(_searchControl);
        }
    }


}
