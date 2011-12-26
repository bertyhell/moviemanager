using System.ComponentModel;
using System.Windows.Controls;
using Model;

namespace MovieManager.APP.Search
{

    /// <summary>
    /// Interaction logic for ActorOverview.xaml
    /// </summary>
    public partial class ActorOverview : UserControl
    {

        public event ClickedOnSearch SearchEvent;

        public ActorOverview()
        {
            InitializeComponent();
            DataContext = Actor;
            _movieScrollPanel.SearchEvent += _movieScrollPanel_SearchEvent;
        }

        void _movieScrollPanel_SearchEvent(SearchEventArgs e)
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
