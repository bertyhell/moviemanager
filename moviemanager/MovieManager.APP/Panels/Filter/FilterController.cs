﻿using System.Collections.Generic;
using System.Collections.ObjectModel;

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
                        AppliedFilters.Add(new FilterText());
                        break;
                    case Filters.Genre:
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