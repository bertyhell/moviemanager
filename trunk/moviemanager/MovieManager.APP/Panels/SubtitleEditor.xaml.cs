using System;
using System.Configuration;
using System.Windows;
using System.Windows.Forms;
using Model;
using MessageBox = System.Windows.MessageBox;

namespace MovieManager.APP.Panels
{
    /// <summary>
    /// Interaction logic for SubtitleEditor.xaml
    /// </summary>
    public partial class SubtitleEditor
    {
        public static readonly DependencyProperty VideoProperty =
            DependencyProperty.Register("Video", typeof(Video), typeof(SubtitleEditor), new PropertyMetadata(null));

        public Video Video
        {
            get { return (Video)GetValue(VideoProperty); }
            set
            {
                SetValue(VideoProperty, value);
                DataContext = value;
            }
        }

        public SubtitleEditor()
        {
            InitializeComponent();
        }

        private void BtnAddSubtitleClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                InitialDirectory = ConfigurationManager.AppSettings["defaultVideoLocation"],
                Multiselect = true
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (String file in ofd.FileNames)
                {
                    if (!AlreadyInSubs(file))
                        Video.Subs.Add(new Subtitle { Path = file });
                    else
                    {
                        MessageBox.Show(Localization.Resource.SubtitleAlreadyInMovie, Localization.Resource.Error,
                                        MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        //check for duplicate
        private bool AlreadyInSubs(String path)
        {
            bool isDuplicate = false;
            foreach (Subtitle sub in Video.Subs)
            {
                if (sub.Path == path)
                {
                    isDuplicate = true;
                    break;
                }
            }
            return isDuplicate;
        }

        private void BtnDelSubtitleClick(object sender, RoutedEventArgs e)
        {
            foreach(Subtitle sub in _grdSubs.SelectedItems)
            {
                Video.Subs.Remove(sub);
            }
        }
    }
}
