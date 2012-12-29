using System.Collections.ObjectModel;
using System.ComponentModel;
using Tmc.DataAccess.SqlCe;

namespace Tmc.WinUI.Application.Panels.Filter
{

    public enum Filters
    {
        Name, Path, Genre, ReleaseDate, Rating
    }

    public class FilterController : INotifyPropertyChanged
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
                        AppliedFilters.Add(new FilterMultiOption("Genres", DataRetriever.Genres, "Genres"));
                        break;
                    case Filters.ReleaseDate:
                        AppliedFilters.Add(new FilterDate("Release", "Release Date"));
                        break;
                    case Filters.Rating:
                        AppliedFilters.Add(new FilterRating("Rating", "Rating"));
                        break;
                }
                PropChanged("SelectedFilterIndex");
            }
        }

        public int SelectedFilterIndex
        {
            get { return -1; }
        }

        private ObservableCollection<FilterControl> _appliedFilters= new ObservableCollection<FilterControl>();
        public ObservableCollection<FilterControl> AppliedFilters
        {
            get { return _appliedFilters; }
            set { _appliedFilters = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void PropChanged(string field)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(field));
            }
        }
    }
}
