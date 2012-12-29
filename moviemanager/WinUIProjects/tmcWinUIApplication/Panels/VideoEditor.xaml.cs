using System.Collections.ObjectModel;
using System.Windows;
using Tmc.SystemFrameworks.Model;
using Tmc.WinUI.Application.Commands;

namespace Tmc.WinUI.Application.Panels
{
    /// <summary>
    /// Interaction logic for VideoEditor.xaml
    /// </summary>
    public partial class VideoEditor
    {
        public static readonly DependencyProperty VIDEO_PROPERTY =
            DependencyProperty.Register("Video", typeof(Video), typeof(VideoEditor), new PropertyMetadata(default(Video)));

        public Video Video
        {
            get { return (Video)GetValue(VIDEO_PROPERTY); }
            set
            {
                SetValue(VIDEO_PROPERTY, value);
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
            VideoTypeEnum VideoType = (VideoTypeEnum)e.AddedItems[0];
            if (Video.VideoType != VideoType)
            {
                ObservableCollection<Video> LocalVideos = MainController.Instance.Videos;
                int Index = LocalVideos.IndexOf(Video);
                Video = Video.ConvertVideo(VideoType, Video);
                LocalVideos.RemoveAt(Index);
                LocalVideos.Insert(Index,Video);
            }
        }

        /// <summary>
        /// Save button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            UpdateVideosCommand UpdateVideosCommand = new UpdateVideosCommand(Video);
            UpdateVideosCommand.Execute(new object());
        }
    }
}