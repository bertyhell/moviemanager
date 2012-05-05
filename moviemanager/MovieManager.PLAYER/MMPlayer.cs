using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Model;
using MovieManager.PLAYER.interop;
using VlcPlayer;

namespace MovieManager.PLAYER
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
        private bool _isFullScreen;
        Overlay _overlayForm;

        public MMPlayer()
        {
            InitializeComponent();
            string[] Args = new[] {
                "--ignore-config",
                //@"--plugin-path=" + Path.Combine(PluginPath , "plugins")                
                //@"--plugin-path=C:\Program Files (x86)\VideoLAN\VLC\plugins"
                //,"--vout-filter=deinterlace", "--deinterlace-mode=blend"
            };

            _vlcInstance = new VlcInstance(Args);
        }


        public void PlayVideo(Video video)
        {
            this.Text = video.Name;
            _video = video;
            if (video != null && video.Path != null)
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
            /*
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
            }
        */
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
            _overlayForm.Size = _pnlVideo.Size;
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
                _player.Stop();

            //audio
            else if (keys == Keys.M)
                _player.Mute();
        }

        private void MMPlayer_FormClosing(object sender, FormClosingEventArgs e)
        {
            _player.Release();
        }
    }
}
