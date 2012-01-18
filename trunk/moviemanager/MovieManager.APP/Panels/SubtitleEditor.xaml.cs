using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using Model;
using SQLite;
using MessageBox = System.Windows.MessageBox;
using UserControl = System.Windows.Controls.UserControl;

namespace MovieManager.APP.CommonControls
{
    /// <summary>
    /// Interaction logic for SubtitleEditor.xaml
    /// </summary>
    public partial class SubtitleEditor : UserControl
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

        private void _btnAddSubtitle_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog Ofd = new OpenFileDialog
            {
                InitialDirectory = ConfigurationManager.AppSettings["defaultVideoLocation"],
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
            bool IsDuplicate = false;
            foreach (Subtitle Sub in Video.Subs)
            {
                if (Sub.Path == path)
                {
                    IsDuplicate = true;
                    break;
                }
            }
            return IsDuplicate;
        }

        private void _btnDelSubtitle_Click(object sender, RoutedEventArgs e)
        {
            foreach(Subtitle Sub in _grdSubs.SelectedItems)
            {
                Video.Subs.Remove(Sub);
            }
        }
    }
}
