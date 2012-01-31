using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Microsoft.Win32;
using System.Windows.Interop;

namespace VlcPlayer
{

    /// <summary>
    /// Interaction logic for VlcPlayerControl.xaml
    /// </summary>
    public partial class VlcPlayerControl
    {
        private readonly VlcInstance _vlcInstance;
        private VlcMediaPlayer _player;

        public VlcPlayerControl()
        {
            InitializeComponent();

            string[] args = new[] {
                "--ignore-config",
                @"--plugin-path=C:\Program Files (x86)\VideoLAN\VLC\plugins"
                //,"--vout-filter=deinterlace", "--deinterlace-mode=blend"
            };

            _vlcInstance = new VlcInstance(args);
            _player = null;
        }

        #region Event Handlers

        private void BtnPlayClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            if (openFileDialog1.ShowDialog() != true)
                return;

            PlayVideo(openFileDialog1.FileName);

        }

        private void BtnPauseClick(object sender, RoutedEventArgs e)
        {
            Pause();
        }

        #endregion

        private void VideoBackgroundClick(object sender, EventArgs e)
        {
            Console.WriteLine("");

        }

        #region Media Player Controls

        public void PlayVideo(String fileName)
        {
            using (VlcMedia media = new VlcMedia(_vlcInstance, fileName))
            {
                if (_player == null)
                    _player = new VlcMediaPlayer(media);
                else
                    _player.Media = media;
            }

            //_player.Drawable = _video.Handle;
            var fromVisual = (HwndSource)PresentationSource.FromVisual(_video);
            if (fromVisual != null)
            {

                _player.Drawable = fromVisual.Handle;
                _player.Play();

                Button inAdorner = new Button
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

        }

        public void Pause()
        {
            if (_player.IsPaused)
                _player.Pause();
            else
                _player.Play();
        }

        #endregion



    }
}
