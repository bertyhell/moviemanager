using System;
using System.ComponentModel;
using System.Drawing;
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
        Overlay _overlayForm;

        public VlcWinForm()
        {
            InitializeComponent();
            KeyPreview = true;
            string[] args = new[] {
                "--ignore-config",
                @"--plugin-path=C:\Program Files (x86)\VideoLAN\VLC\plugins"
                //,"--vout-filter=deinterlace", "--deinterlace-mode=blend"
            };

            _vlcInstance = new VlcInstance(args);
            _player = null;


        }
        
        #region Media Player Controls

        public void PlayVideo()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();


            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                return;

            PlayVideo(openFileDialog1.FileName);
        }

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
                FormBorderStyle = FormBorderStyle.Sizable;
                Size = _previousFormSize;
                Location = _previousFormLocation;
                _pnlVideo.Size = _previousVideoPanelSize;
                _pnlVideo.Location = _previousVideoPanelLocation;
                _menubar.Visible = true;
                _pnlControls.Visible = true;
                _overlayForm.Location = CalculateOverlayLocation();
                _overlayForm.Size = _pnlVideo.Size;
                _isFullScreen = false;
                //overlayForm.Close();
            }

        }

        private void ActivateOverlay()
        {
            //activate overlay
            if (_overlayForm == null)
                _overlayForm = new Overlay(this);
            _overlayForm.Show(this);
            _overlayForm.Focus();
            _overlayForm.Size = _pnlVideo.Size;
            _overlayForm.Location = CalculateOverlayLocation();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Stop();
            _overlayForm.Close();
            base.OnClosing(e);
        }

        private void BtnPauseClick(object sender, EventArgs e)
        {
            Pause();
        }

        private void BtnPlayClick(object sender, EventArgs e)
        {
            PlayVideo();
        }

        private void BtnStopClick(object sender, EventArgs e)
        {
            Stop();
        }

        private void BtnMuteClick(object sender, EventArgs e)
        {
            _player.Mute();
        }

        private void BtnFullScreenClick(object sender, EventArgs e)
        {
            ToggleFullScreen();
        }



        #endregion

        private void VlcWinFormMove(object sender, EventArgs e)
        {
            if (_overlayForm != null)
            {
                _overlayForm.Location = CalculateOverlayLocation();
            }
        }

        private Point CalculateOverlayLocation()
        {
            int borderWidth = (Width - ClientSize.Width) / 2;
            int borderHeight = (Height - ClientSize.Height) - borderWidth;
            return new Point(
                Location.X + borderWidth + _pnlVideo.Location.X,
                Location.Y + borderHeight + _pnlVideo.Location.Y
                );
        }

        private void PnlVideoResize(object sender, EventArgs e)
        {
            _overlayForm.Size = _pnlVideo.Size;
        }

        private void VlcWinFormKeyUp(object sender, KeyEventArgs e)
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
