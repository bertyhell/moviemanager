using System.Windows.Controls;
using Model;

namespace MovieManager.APP.Panels.Search
{

    /// <summary>
    /// Interaction logic for ActorOverview.xaml
    /// </summary>
    public partial class ActorOverview
    {

        public event ClickedOnSearch SearchEvent;

        public ActorOverview()
        {
            InitializeComponent();
            DataContext = Actor;
            _movieScrollPanel.SearchEvent += MovieScrollPanelSearchEvent;
        }

        void MovieScrollPanelSearchEvent(SearchEventArgs e)
        {
            if (SearchEvent != null)
                SearchEvent(e);
        }

        private Actor _actor;
        public Actor Actor
        {
            get { return _actor; }
            set
            {
                _actor = value;
                DataContext = value;
            }
        }
    }
}
