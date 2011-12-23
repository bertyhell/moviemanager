using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Windows.Interop;

namespace VlcPlayer
{

    /// <summary>
    /// Interaction logic for VlcPlayerControl.xaml
    /// </summary>
    public partial class VlcPlayerControl : UserControl
    {
        private VlcInstance vlcInstance;
        private VlcMediaPlayer player;

        public VlcPlayerControl()
        {
            InitializeComponent();

            string[] args = new string[] {
                "--ignore-config",
                @"--plugin-path=C:\Program Files (x86)\VideoLAN\VLC\plugins"
                //,"--vout-filter=deinterlace", "--deinterlace-mode=blend"
            };

            vlcInstance = new VlcInstance(args);
            player = null;
        }

        private void _btnPlay_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            if (openFileDialog1.ShowDialog() != true)
                return;

            using (VlcMedia media = new VlcMedia(vlcInstance, openFileDialog1.FileName))
            {
                if (player == null)
                    player = new VlcMediaPlayer(media);
                else
                    player.Media = media;
            }

            player.Drawable = ((HwndSource)HwndSource.FromVisual(_video)).Handle;
            player.Play();

            Button inAdorner = new Button()
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                Content = "X",
            };
            OverlayAdorner adorner = new OverlayAdorner(_video)
            {
                Child = inAdorner,
            };
            AdornerLayer.GetAdornerLayer(this).Add(adorner);
        }

        private void _VideoBackground_Click(object sender, EventArgs e)
        {
            Console.WriteLine("");

        }


    }
}
