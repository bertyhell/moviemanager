using System;

namespace MovieManager.APP.Search
{
    public class SearchOptions
    {
        private bool _searchForActors;
        private bool _searchForMovies;
        private bool _searchOnImdb;
        private bool _searchOnTmdb;
        private string _searchTerm;

        public bool SearchForActors { get { return _searchForActors; } set { _searchForActors = value; } }
        public bool SearchForMovies { get { return _searchForMovies; } set { _searchForMovies = value; } }
        public bool SearchOnImdb { get { return _searchOnImdb; } set { _searchOnImdb = value; } }
        public bool SearchOnTmdb { get { return _searchOnTmdb; } set { _searchOnTmdb = value; } }
        public string SearchTerm { get { return _searchTerm; } set { _searchTerm = value; } }
    }
}