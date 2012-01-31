using System.Collections.ObjectModel;
using System.Windows;
using Model;

namespace MovieManager.APP.Panels
{
    /// <summary>
    /// Interaction logic for VideoEditor.xaml
    /// </summary>
    public partial class VideoEditor
    {
        public static readonly DependencyProperty VideoProperty =
            DependencyProperty.Register("Video", typeof(Video), typeof(VideoEditor), new PropertyMetadata(default(Video)));

        public Video Video
        {
            get { return (Video)GetValue(VideoProperty); }
            set
            {
                SetValue(VideoProperty, value);
                DataContext = value;
            }
        }

        //private Video _video;

        public VideoEditor()
        {
            InitializeComponent();
        }

        //public Video Video
        //{
        //    get { return _video; }
        //    set
        //    {
        //        _video = value;
        //        DataContext = _video;
        //    }
        //}

        private void ComboBoxSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            VideoTypeEnum videoType = (VideoTypeEnum)e.AddedItems[0];
            if (Video.VideoType != videoType)
            {
                ObservableCollection<Video> localVideos = MainController.Instance.Videos;
                int index = localVideos.IndexOf(Video);
                Video = Video.ConvertVideo(videoType, Video);
                localVideos.RemoveAt(index);
                localVideos.Insert(index,Video);
            }
        }
    }
}