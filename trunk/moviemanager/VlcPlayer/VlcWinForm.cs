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

        private Point _previousFormLocation;
        private Size _previousFormSize;
        private Point _previousVideoPanelLocation;
        private Size _previousVideoPanelSize;
        private bool _isFullScreen;
        Overlay overlayForm;

        public VlcWinForm()
        {
            InitializeComponent();
            this.KeyPreview = true;
            string[] Args = new string[] {
                "--ignore-config",
                @"--plugin-path=C:\Program Files (x86)\VideoLAN\VLC\plugins"
                //,"--vout-filter=deinterlace", "--deinterlace-mode=blend"
            };

            _vlcInstance = new VlcInstance(Args);
            _player = null;


        }
        
        #region Media Player Controls

        public void PlayVideo()
        {
            OpenFileDialog OpenFileDialog1 = new OpenFileDialog();


            if (OpenFileDialog1.ShowDialog() == DialogResult.OK)
                return;

            PlayVideo(OpenFileDialog1.FileName);
        }

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

            ActivateOverlay();
        }

        public void Pause()
        {
            if (!_player.IsPaused)
                _player.Pause();
            else
                _player.Play();
        }

        public void Stop()
        {
            _player.Stop();
        }

        public void ToggleFullScreen()
        {
            if (!_isFullScreen)
            {
                //save data
                _previousFormLocation = Location;
                _previousFormSize = Size;
                _previousVideoPanelLocation = _pnlVideo.Location;
                _previousVideoPanelSize = _pnlVideo.Size;
                FormBorderStyle = FormBorderStyle.None;

                //change visual
                _pnlControls.Visible = false;
                _menubar.Visible = false;
                Location = new Point(0, 0);
                Size = new Size((int)System.Windows.SystemParameters.PrimaryScreenWidth,
                               (int)System.Windows.SystemParameters.PrimaryScreenHeight);
                _pnlVideo.Location = new Point(0, 0);
                _pnlVideo.Size = new Size((int)System.Windows.SystemParameters.PrimaryScreenWidth,
                                          (int)System.Windows.SystemParameters.PrimaryScreenHeight);
                _isFullScreen = true;

            }
            else
            {
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.Size = _previousFormSize;
                this.Location = _previousFormLocation;
                _pnlVideo.Size = _previousVideoPanelSize;
                _pnlVideo.Location = _previousVideoPanelLocation;
                _menubar.Visible = true;
                _pnlControls.Visible = true;
                overlayForm.Location = CalculateOverlayLocation();
                overlayForm.Size = _pnlVideo.Size;
                _isFullScreen = false;
                //overlayForm.Close();
            }

        }

        private void ActivateOverlay()
        {
            //activate overlay
            if (overlayForm == null)
                overlayForm = new Overlay(this);
            overlayForm.Show(this);
            overlayForm.Focus();
            overlayForm.Size = _pnlVideo.Size;
            overlayForm.Location = CalculateOverlayLocation();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Stop();
            overlayForm.Close();
            base.OnClosing(e);
        }

        private void _btnPause_Click(object sender, EventArgs e)
        {
            Pause();
        }

        private void _btnPlay_Click(object sender, EventArgs e)
        {
            PlayVideo();
        }

        private void _btnStop_Click(object sender, EventArgs e)
        {
            Stop();
        }

        private void _btnMute_Click(object sender, EventArgs e)
        {
            _player.Mute();
        }

        private void _btnFullScreen_Click(object sender, EventArgs e)
        {
            ToggleFullScreen();
        }



        #endregion

        private void VlcWinForm_Move(object sender, EventArgs e)
        {
            if (overlayForm != null)
            {
                overlayForm.Location = CalculateOverlayLocation();
            }
        }

        private Point CalculateOverlayLocation()
        {
            int BorderWidth = (this.Width - this.ClientSize.Width) / 2;
            int BorderHeight = (this.Height - this.ClientSize.Height) - BorderWidth;
            return new Point(
                this.Location.X + BorderWidth + _pnlVideo.Location.X,
                this.Location.Y + BorderHeight + _pnlVideo.Location.Y
                );
        }

        private void _pnlVideo_Resize(object sender, EventArgs e)
        {
            overlayForm.Size = _pnlVideo.Size;
        }

        private void VlcWinForm_KeyUp(object sender, KeyEventArgs e)
        {
            HandleKeys(e.KeyCode);
        }

        public void HandleKeys(Keys keys)
        {
            //video
            if (keys == Keys.F)
                ToggleFullScreen();
            else if (keys == Keys.Space)
                Pause();

            //audio
            else if(keys == Keys.M)
                _player.Mute();
        }
        
    }
}
