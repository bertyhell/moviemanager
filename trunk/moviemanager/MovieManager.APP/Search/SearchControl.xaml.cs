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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MovieManager.APP.Search
{

    public class SearchEventArgs : EventArgs
    {
        public SearchOptions SearchOptions { get; set; }
    }

    public delegate void ClickedOnSearch(SearchEventArgs e);

    /// <summary>
    /// Interaction logic for SearchControl.xaml
    /// </summary>
    public partial class SearchControl : UserControl
    {
        public event ClickedOnSearch ClickOnSearch;

        public SearchControl()
        {
            InitializeComponent();
        }

        private void _btnSearch_Click(object sender, RoutedEventArgs e)
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
