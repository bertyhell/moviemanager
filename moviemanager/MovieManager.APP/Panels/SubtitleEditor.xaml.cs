using System;
using System.Configuration;
using System.IO;
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
            String Path = ConfigurationManager.AppSettings["defaultVideoLocation"];
            if (!new DirectoryInfo(Path).Exists)
            {
                Path = ConfigurationManager.AppSettings["defaultVideoLocation1"];
            }
            OpenFileDialog Ofd = new OpenFileDialog
            {
                InitialDirectory = Path,
                Multiselect = true
            };
            if (Ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (String File in Ofd.FileNames)
                {
                    if (!AlreadyInSubs(File))
                        Video.Subs.Add(new Subtitle { Path = File });
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
