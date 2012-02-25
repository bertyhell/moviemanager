﻿using System.Collections.ObjectModel;
using SQLite;

namespace MovieManager.APP.Panels.Filter
{

    public enum Filters
    {
        Name, Path, Genre, ReleaseDate, Rating
    }

    public class FilterController
    {
        public Filters SelectedFilter
        {
            set
            {
                switch (value)
                {
                    case Filters.Name:
                        AppliedFilters.Add(new FilterText("Name"));
                        break;
                    case Filters.Path:
                        AppliedFilters.Add(new FilterText("Path"));
                        break;
                    case Filters.Genre:
                        AppliedFilters.Add(new FilterMultiOption("Genre",MMDatabase.GetMovieGenres()));
                        break;
                }
            }
        }

        private ObservableCollection<FilterControl> _appliedFilters= new ObservableCollection<FilterControl>();
        public ObservableCollection<FilterControl> AppliedFilters
        {
            get { return _appliedFilters; }
            set { _appliedFilters = value; }
        }
    }
}