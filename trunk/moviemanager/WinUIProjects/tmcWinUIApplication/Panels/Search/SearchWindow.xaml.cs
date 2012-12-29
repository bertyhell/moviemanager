﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Tmc.BusinessRules.Web.Search;
using Tmc.SystemFrameworks.Model;
using Tmc.SystemFrameworks.Model.Interfaces;
using Tmc.WinUI.Application.Localization;
using Tmc.WinUI.Application.Panels.Common;

namespace Tmc.WinUI.Application.Panels.Search
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
                List<Actor> SearchedActors = SearchTmdb.SearchActor(options.SearchTerm);
                if (SearchedActors.Count > 1)
                {
                    ThumbnailDescriptionListWindow Window = new ThumbnailDescriptionListWindow { ThumbnailDescriptionItems = SearchedActors.ToList<IPreviewInfoRetriever>() };
                    Window.ShowDialog();

                    SearchForActor((Actor)Window.SelectedPreviewDescription);
                }
                else if (SearchedActors.Count == 1)
                {
                    SearchForActor(SearchedActors[0]);
                }
                else
                {
                    MessageBox.Show(Resource.NoResultsFound, Resource.NoActorsWereFound,
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }

            if (options.SearchForMovies)
            {
                Movie Movie = null;
                try
                {

                    Movie = new Movie {IdTmdb = Convert.ToInt32(options.SearchTerm)};
                }
                catch
                {
                    List<Movie> Movies = SearchTmdb.GetVideoInfo(options.SearchTerm);
                    if(Movies.Count > 1)
                    {
                        ThumbnailDescriptionListWindow Window = new ThumbnailDescriptionListWindow { ThumbnailDescriptionItems = Movies.ToList<IPreviewInfoRetriever>() };
                        Window.ShowDialog();

                        Movie = (Movie)Window.SelectedPreviewDescription;
                    }
                    else if (Movies.Count == 1)
                    {
                        Movie = Movies.Count > 0 ? Movies[0] : null;
                    }
                }
                if (Movie == null) return;
                SearchTmdb.GetExtraMovieInfo(Movie);
                UpdateGuiForMovie(Movie);
            }
        }

        private void SearchForActor(Actor actor)
        {
            ResetGrid();
            SearchTmdb.GetActorInfo(actor);

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