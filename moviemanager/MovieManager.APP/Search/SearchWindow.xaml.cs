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
        public SearchWindow()
        {
            InitializeComponent();
        }

        private void _searchControl_ClickOnSearch(ClickOnSearchEventArgs e)
        {
            List<Actor> SearchedActors = SearchTMDB.SearchActor(e.SearchTerm);
            if (SearchedActors.Count > 0)
            {
                SearchTMDB.GetActorInfo(SearchedActors[0]);
                _actorOverview.Actor = SearchedActors[0];
            }
        }
    }
}
