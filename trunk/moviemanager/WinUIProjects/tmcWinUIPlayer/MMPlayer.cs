using System;
using System.Drawing;
using System.Windows.Forms;
using Tmc.SystemFrameworks.Model;
using Tmc.WinUI.Player.Common;

namespace Tmc.WinUI.Player
{
    public partial class MMPlayer : Form
    {
        private VlcMediaPlayer _player;
        private readonly VlcInstance _vlcInstance;
        private Video _video;

        private Point _previousFormLocation;
        private Size _previousFormSize;
        private Point _previousVideoPanelLocation;
        private Size _previousVideoPanelSize;
        private FormWindowState _previousIsMaximized;
        private bool _isFullScreen;
        Overlay _overlayForm;

        public MMPlayer()
        {
            InitializeComponent();
            string[] Args = new[] {
                "--ignore-config"
                //@"--plugin-path=" + Path.Combine(PluginPath , "plugins")                
                //@"--plugin-path=C:\Program Files (x86)\VideoLAN\VLC\plugins"
                //,"--vout-filter=deinterlace", "--deinterlace-mode=blend"
            };

            _vlcInstance = new VlcInstance(Args);
            _mediaPlayerControl.Initialize(this);
        }

        #region Properties

        public VlcMediaPlayer Player
        {
            get { return _player; }
        }

        public MediaPlayerControl ControlBar
        {
            get { return _mediaPlayerControl; }
        }

        #endregion


        public void PlayVideo(Video video)
        {
            Text = video.Name;
            _video = video;
            if (video.Path != null)
            {
                PlayVideo(video.Path);
                _player.CurrentTimestamp = (long)video.LastPlayLocation;
            }
        }

        private void PlayVideo(String fileName)
        {
            using (VlcMedia Media = new VlcMedia(_vlcInstance, fileName))
            {
                if (_player == null)
                    _player = new VlcMediaPlayer(Media, _pnlVideo);
                else
                    _player.Media = Media;
            }

            //_player.Drawable = _video.Handle;
            _player.Play();

            ActivateOverlay();


        }

        #region full screen

        public bool IsFullScreen
        {
            get { return _isFullScreen; }
            set { _isFullScreen = value; }
        }

        public void ToggleFullScreen()
        {
            if (!_isFullScreen)
            {
                //save data
                _previousIsMaximized = WindowState;
                _previousFormLocation = Location;
                _previousFormSize = Size;
                _previousVideoPanelLocation = _pnlVideo.Location;
                _previousVideoPanelSize = _pnlVideo.Size;

                WindowState = FormWindowState.Normal;
                FormBorderStyle = FormBorderStyle.None;

                //change visual
                _pnlControlBarHolder.Controls.Remove(_mediaPlayerControl);
                _pnlControlBarHolder.Visible = false;
                _overlayForm.AddControlBar(_mediaPlayerControl);
                _menubar.Visible = false;
                Size = new Size((int)System.Windows.SystemParameters.PrimaryScreenWidth,
                               (int)System.Windows.SystemParameters.PrimaryScreenHeight);
                Location = new Point(0, 0);
                _pnlVideo.Location = new Point(0, 0);
                _pnlVideo.Size = new Size((int)System.Windows.SystemParameters.PrimaryScreenWidth,
                                          (int)System.Windows.SystemParameters.PrimaryScreenHeight);
                _pnlVideo.Refresh();
                _isFullScreen = true;

            }
            else
            {
                _overlayForm.RemoveControlBar();
                _pnlControlBarHolder.Controls.Add(_mediaPlayerControl);
                _pnlControlBarHolder.Visible = true;
                FormBorderStyle = FormBorderStyle.Sizable;
                Size = _previousFormSize;
                Location = _previousFormLocation;
                _menubar.Visible = true;
                _pnlVideo.Location = _previousVideoPanelLocation;
                _pnlVideo.Size = _previousVideoPanelSize;
                _overlayForm.Location = CalculateOverlayLocation();
                _overlayForm.Size = _pnlVideo.Size;
                //_pnlVideo.Anchor = (AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right);


                WindowState = _previousIsMaximized;
                _isFullScreen = false;
            }
        }

        private void ActivateOverlay()
        {
            //activate overlay
            if (_overlayForm == null)
                _overlayForm = new Overlay(this);
            if (!_overlayForm.Visible)
                _overlayForm.Show(this);
            _overlayForm.Focus();
            _overlayForm.Size = _pnlVideo.Size;
            _overlayForm.Location = CalculateOverlayLocation();
        }

        #endregion

        private void FormMove(object sender, EventArgs e)
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

        private void VideoPanelResize(object sender, EventArgs e)
        {
            _overlayForm.Redraw(_pnlVideo.Size);
        }

        private void FormKeyUp(object sender, KeyEventArgs e)
        {
            HandleKeys(e.KeyCode);
        }

        public void HandleKeys(Keys keys)
        {
            //video
            if (keys == Keys.F)
                ToggleFullScreen();
            else if (keys == Keys.Space)
                _player.Pause();

            //audio
            else if (keys == Keys.M)
                _player.Mute();

            //with control mask
            else if (ModifierKeys == Keys.Control)
            {
                //Audio
                if (keys == Keys.Up)
                {
                    _player.RaiseVolume();
                    _overlayForm.SetMessage("Volume " + _player.Volume);
                }
                else if (keys == Keys.Down)
                {
                    _player.LowerVolume();
                    _mediaPlayerControl.RefreshVolumeTrackbarPostion();
                    _overlayForm.SetMessage("Volume " + _player.Volume);
                }
            }
        }

        private void MMPlayerFormClosing(object sender, FormClosingEventArgs e)
        {
            _player.Release();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                _vlcInstance.Dispose();
            }
            base.Dispose(disposing);
        }

        #region menu items

        private void AlwaysOnTopToolStripMenuItemClick(object sender, EventArgs e)
        {
	        TopMost = !TopMost;
	        ((ToolStripMenuItem)sender).CheckState = TopMost ? CheckState.Unchecked : CheckState.Checked;
        }

	    #endregion
    }
}
