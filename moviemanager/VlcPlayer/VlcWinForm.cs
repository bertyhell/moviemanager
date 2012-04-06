using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using VlcPlayer.Common;

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
            string[] Args = new[] {
                "--ignore-config",
                @"--plugin-path=C:\Program Files (x86)\VideoLAN\VLC\plugins"
                //,"--vout-filter=deinterlace", "--deinterlace-mode=blend"
            };

            _vlcInstance = new VlcInstance(Args);
            _player = null;
            _pnlControls.VlcWinForm = this;
        }

        public bool IsFullScreen
        {
            get { return _isFullScreen; }
            set { _isFullScreen = value; }
        }

        public MediaPlayerControl ControlPanel
        {
            get { return _pnlControls; }
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
                    _player = new VlcMediaPlayer(Media, this);
                else
                    _player.Media = Media;
            }

            //_player.Drawable = _video.Handle;
            _player.Drawable = _pnlVideo.Handle;
            _player.Play();

            ActivateOverlay();
            _pnlControls.Player = _player;
            _pnlControls.AttachToEvents();
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
                Controls.Remove(_pnlControls);
                _overlayForm.Controls.Add(_pnlControls);
                _pnlControls.ToggleFullScreen();
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

                _overlayForm.Controls.Remove(_pnlControls);
                Controls.Add(_pnlControls);
                _pnlControls.ToggleFullScreen();
                FormBorderStyle = FormBorderStyle.Sizable;
                Size = _previousFormSize;
                Location = _previousFormLocation;
                _pnlVideo.Size = _previousVideoPanelSize;
                _pnlVideo.Location = _previousVideoPanelLocation;
                _menubar.Visible = true;
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
            _overlayForm.Show(this);//TODO 080 check if already visible -> now this throws an else exception
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
            int BorderWidth = (Width - ClientSize.Width) / 2;
            int BorderHeight = (Height - ClientSize.Height) - BorderWidth;
            return new Point(
                Location.X + BorderWidth + _pnlVideo.Location.X,
                Location.Y + BorderHeight + _pnlVideo.Location.Y
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
            else if (keys == Keys.M)
                _player.Mute();
        }


        private void VlcWinFormFormClosing(object sender, FormClosingEventArgs e)
        {
            _pnlControls.DetachFromEvents();
            _player.Stop();
        }



    }
}
