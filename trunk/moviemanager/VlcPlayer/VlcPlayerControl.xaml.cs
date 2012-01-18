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
        private readonly VlcInstance _vlcInstance;
        private VlcMediaPlayer _player;

        public VlcPlayerControl()
        {
            InitializeComponent();

            string[] Args = new string[] {
                "--ignore-config",
                @"--plugin-path=C:\Program Files (x86)\VideoLAN\VLC\plugins"
                //,"--vout-filter=deinterlace", "--deinterlace-mode=blend"
            };

            _vlcInstance = new VlcInstance(Args);
            _player = null;
        }

#region Event Handlers

        private void _btnPlay_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog OpenFileDialog1 = new OpenFileDialog();

            if (OpenFileDialog1.ShowDialog() != true)
                return;

            PlayVideo(OpenFileDialog1.FileName);

        }

        private void _btnPause_Click(object sender, RoutedEventArgs e)
        {
            Pause();
        }

#endregion

        private void _VideoBackground_Click(object sender, EventArgs e)
        {
            Console.WriteLine("");

        }

        #region Media Player Controls

        public void PlayVideo(String fileName)
        {
            using (VlcMedia Media = new VlcMedia(_vlcInstance, fileName))
            {
                if (_player == null)
                    _player = new VlcMediaPlayer(Media);
                else
                    _player.Media = Media;
            }

            //_player.Drawable = _video.Handle;
            _player.Drawable = ((HwndSource)HwndSource.FromVisual(_video)).Handle;
            _player.Play();

            Button InAdorner = new Button()
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                Content = "X",
            };
            OverlayAdorner Adorner = new OverlayAdorner(_video)
            {
                Child = InAdorner,
            };
            AdornerLayer.GetAdornerLayer(this).Add(Adorner);

        }

        public void Pause()
        {
            if(_player.IsPaused)
                _player.Pause();
            else
                _player.Play();
        }

        #endregion



    }
}
