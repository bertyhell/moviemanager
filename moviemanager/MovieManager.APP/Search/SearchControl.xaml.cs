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

    public delegate void ClickedOnSearch(ClickOnSearchEventArgs e);

    public class ClickOnSearchEventArgs : EventArgs
    {

        private bool _searchForActors;
        private bool _searchForMovies;
        private string _searchTerm;

        public bool SearchForActors { get { return _searchForActors; } set { _searchForActors = value; } }
        public bool SearchForMovies { get { return _searchForMovies; } set { _searchForMovies = value; } }
        public string SearchTerm { get { return _searchTerm; } set { _searchTerm = value; } }
    }

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
                ClickOnSearch(new ClickOnSearchEventArgs()
                {
                    SearchForActors = (bool)_ChkActor.IsChecked,
                    SearchForMovies = (bool)_ChkMovie.IsChecked,
                    SearchTerm = _txtSearchTerm.Text.Trim()
                });
            }
        }
    }
}
