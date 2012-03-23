using System.Collections.ObjectModel;
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
                        AppliedFilters.Add(new FilterText("Name", "Name"));
                        break;
                    case Filters.Path:
                        AppliedFilters.Add(new FilterText("Path", "Path"));
                        break;
                    case Filters.Genre:
                        AppliedFilters.Add(new FilterMultiOption("Genres",MMDatabase.GetMovieGenres(), "Genres"));
                        break;
                    case Filters.ReleaseDate:
                        AppliedFilters.Add(new FilterDate("Release", "Release Date"));
                        break;
                    case Filters.Rating:
                        AppliedFilters.Add(new FilterRating("Rating", "Rating"));
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
