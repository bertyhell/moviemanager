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
        public ActorOverview()
        {
            InitializeComponent();
            DataContext = Actor;
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
