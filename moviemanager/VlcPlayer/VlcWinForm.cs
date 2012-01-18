using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VlcPlayer
{
    public partial class VlcWinForm : Form
    {
        private readonly VlcInstance _vlcInstance;
        private VlcMediaPlayer _player;

        public VlcWinForm()
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

        //private void _btnPlay_Click(object sender, RoutedEventArgs e)
        //{
        //    OpenFileDialog OpenFileDialog1 = new OpenFileDialog();

        //    if (OpenFileDialog1.ShowDialog() != true)
        //        return;

        //    PlayVideo(OpenFileDialog1.FileName);

        //}

        //private void _btnPause_Click(object sender, RoutedEventArgs e)
        //{
        //    Pause();
        //}

#endregion

        //        private void _VideoBackground_Click(object sender, EventArgs e)
//        {
//            Console.WriteLine("");

//        }

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
            _player.Drawable = _pnlVideo.Handle;
            _player.Play();
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
