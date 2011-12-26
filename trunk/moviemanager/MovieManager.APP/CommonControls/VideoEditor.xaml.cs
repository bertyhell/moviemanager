using System.Windows;
using Model;

namespace MovieManager.APP.CommonControls
{
    /// <summary>
    /// Interaction logic for VideoEditor.xaml
    /// </summary>
    public partial class VideoEditor : Window
    {
        private Video _video;

        public VideoEditor()
        {
            InitializeComponent();
        }

        public Video Video
        {
            get { return _video; }
            set
            {
                _video = value;
                DataContext = _video;
            }
        }
    }
}
