namespace MovieManager.APP.Panels.Search
{
    public class SearchOptions
    {
        public bool SearchForActors { get; set; }
        public bool SearchForMovies { get; set; }
        public bool SearchOnImdb { get; set; }
        public bool SearchOnTmdb { get; set; }
        public string SearchTerm { get; set; }
    }
}