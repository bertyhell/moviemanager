using System.Windows;
using Model;
using System;

namespace MovieManager.APP.Panels
{
    /// <summary>
    /// Interaction logic for VideoEditor.xaml
    /// </summary>
    public partial class VideoEditor : Window
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

        private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            VideoTypeEnum VideoType = (VideoTypeEnum)e.AddedItems[0];
            if (Video.VideoType != VideoType)
                Video = Video.ConvertVideo(VideoType, Video);
        }
    }
}