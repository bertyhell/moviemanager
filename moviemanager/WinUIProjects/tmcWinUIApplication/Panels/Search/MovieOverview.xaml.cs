using Tmc.SystemFrameworks.Model;

namespace Tmc.WinUI.Application.Panels.Search
{
    /// <summary>
    /// Interaction logic for ActorOverview.xaml
    /// </summary>
    public partial class MovieOverview
    {
        public MovieOverview()
        {
            InitializeComponent();
            DataContext = Video;
        }

        private Video _video;
        public Video Video
        {
            get { return _video; }
            set
            {
                _video = value;
                DataContext = value;
            }
        }
    }
}
