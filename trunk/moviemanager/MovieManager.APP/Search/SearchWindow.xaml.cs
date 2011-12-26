using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MovieManager.BL.Search;
using Model;

namespace MovieManager.APP.Search
{
    /// <summary>
    /// Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        private ActorOverview _overview;

        public SearchWindow()
        {
            InitializeComponent();
        }

        private void _searchControl_ClickOnSearch(SearchEventArgs e)
        {
            Search(e.SearchOptions);
        }

        private void Search(SearchOptions options)
        {
            if (options.SearchForActors)
            {
                List<Actor> SearchedActors = SearchTMDB.SearchActor(options.SearchTerm);
                if (SearchedActors.Count > 1)
                {
                    //TODO 090: Make selection window
                    SearchForActor(SearchedActors[0]);
                }
                else if (SearchedActors.Count == 1)
                {
                    SearchForActor(SearchedActors[0]);
                }
                else
                {
                    MessageBox.Show(Localization.Resource.NoResultsFound, Localization.Resource.NoActorsWereFound,
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }

            if (options.SearchForMovies)
            {
                Movie Movie;
                try
                {
                    int QueryIsId = Convert.ToInt32(options.SearchTerm);
                    Movie = new Movie();
                    SearchTMDB.GetExtraMovieInfo(QueryIsId, Movie);
                }
                catch
                {
                    List<Movie> Movies = SearchTMDB.GetVideoInfo(options.SearchTerm);
                    //TODO 090: Make selection window
                    if (Movies.Count > 0)
                    {
                        Movie = Movies[0];
                    }
                    else
                    {
                        Movie = null;
                    }
                }
                SearchForMovie(Movie);
            }
        }

        private void SearchForActor(Actor actor)
        {
            ResetGrid();
            SearchTMDB.GetActorInfo(actor);

            if (_overview == null)
            {
                _overview = new ActorOverview();
                _overview.SearchEvent += _searchControl_ClickOnSearch;
                Grid.SetRow(_overview, 1);
            }
            _overview.Actor = actor;
            _layoutRoot.Children.Add(_overview);
        }

        private void SearchForMovie(Movie movie)
        {
            ResetGrid();
            MovieOverview Overview = new MovieOverview();
            Overview.Movie = movie;
            Grid.SetRow(Overview, 1);
            _layoutRoot.Children.Add(Overview);
        }

        private void ResetGrid()
        {
            _layoutRoot.Children.Clear();
            _layoutRoot.Children.Add(_searchControl);
        }
    }


}
