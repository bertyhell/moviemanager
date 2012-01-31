﻿using System.Windows.Controls;
using Model;

namespace MovieManager.APP.Panels.Search
{
    /// <summary>
    /// Interaction logic for ActorOverview.xaml
    /// </summary>
    public partial class MovieOverview
    {
        public MovieOverview()
        {
            InitializeComponent();
            DataContext = Movie;
        }

        private Movie _movie;
        public Movie Movie
        {
            get { return _movie; }
            set
            {
                _movie = value;
                DataContext = value;
            }
        }
    }
}
