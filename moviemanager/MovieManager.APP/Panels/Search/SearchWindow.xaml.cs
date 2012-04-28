﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Model;
using MovieManager.WEB.Search;

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
                Movie Movie = null;
                int QueryIsId = 0;
                try
                {
                    QueryIsId = Convert.ToInt32(options.SearchTerm);
                    Movie = new Movie();
                }
                catch
                {
                    List<Movie> Movies = SearchTMDB.GetVideoInfo(options.SearchTerm);
                    //TODO 090: Make selection window
                    Movie = Movies.Count > 0 ? Movies[0] : null;
                    if (Movie != null) QueryIsId = Movie.IdTmdb;
                    else return;
                }
                
                SearchTMDB.GetExtraMovieInfo(QueryIsId, Movie);
                UpdateGuiForMovie(Movie);
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

        private void UpdateGuiForMovie(Movie movie)
        {
            ResetGrid();
            MovieOverview Overview = new MovieOverview {Movie = movie};
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
