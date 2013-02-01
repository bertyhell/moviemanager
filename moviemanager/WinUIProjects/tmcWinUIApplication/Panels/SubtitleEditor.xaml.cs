using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using Tmc.SystemFrameworks.Model;
using Tmc.WinUI.Application.Localization;
using MessageBox = System.Windows.MessageBox;

namespace Tmc.WinUI.Application.Panels
{
    /// <summary>
    /// Interaction logic for SubtitleEditor.xaml
    /// </summary>
    public partial class SubtitleEditor
    {
        public static readonly DependencyProperty VIDEO_PROPERTY =
            DependencyProperty.Register("Video", typeof(Video), typeof(SubtitleEditor), new PropertyMetadata(null));

        public Video Video
        {
            get { return (Video)GetValue(VIDEO_PROPERTY); }
            set
            {
                SetValue(VIDEO_PROPERTY, value);
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
                        Video.Files[0].Subs.Add(new Subtitle { Path = File });
                    else
                    {
                        MessageBox.Show(Resource.SubtitleAlreadyInMovie, Resource.Error,
                                        MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        //check for duplicate
        private bool AlreadyInSubs(String path)
        {
            return Video.Files[0].Subs.Any(sub => sub.Path == path);
        }

        private void BtnDelSubtitleClick(object sender, RoutedEventArgs e)
        {
            foreach(Subtitle Sub in _grdSubs.SelectedItems)
            {
                Video.Files[0].Subs.Remove(Sub);
            }
        }
    }
}
