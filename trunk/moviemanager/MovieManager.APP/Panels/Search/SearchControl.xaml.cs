using System;
using System.Windows;
using System.Windows.Controls;

namespace MovieManager.APP.Panels.Search
{

    public class SearchEventArgs : EventArgs
    {
        public SearchOptions SearchOptions { get; set; }
    }

    public delegate void ClickedOnSearch(SearchEventArgs e);

    /// <summary>
    /// Interaction logic for SearchControl.xaml
    /// </summary>
    public partial class SearchControl
    {
        public event ClickedOnSearch ClickOnSearch;

        public SearchControl()
        {
            InitializeComponent();
        }

        private void BtnSearchClick(object sender, RoutedEventArgs e)
        {
            if (ClickOnSearch != null)
            {
                ClickOnSearch(new SearchEventArgs
                {
                    SearchOptions = new SearchOptions
                    {
                        SearchForActors = (_ChkActor.IsChecked == true),
                        SearchForMovies = (_ChkMovie.IsChecked == true),
                        SearchOnImdb = (_radImdb.IsChecked == true),
                        SearchOnTmdb = (_radTmdb.IsChecked == true),
                        SearchTerm = _txtSearchTerm.Text.Trim()
                    }
                });
            }
        }
    }
}
