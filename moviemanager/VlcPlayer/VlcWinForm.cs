using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Model;
using MovieManager.PLAYER.interop;
using VlcPlayer.Common;
using VlcPlayer.Enums;

namespace VlcPlayer
{
    public partial class VlcWinForm : Form
    {
        private readonly VlcInstance _vlcInstance;
        private VlcMediaPlayer _player;
        private Video _video;

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
            string PluginPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("VLC_PLUGIN_PATH", Path.Combine(PluginPath, "plugins"));
            if (string.IsNullOrEmpty(PluginPath))
            {
                throw new FileNotFoundException("VLC plugins not found");
            }
            string[] Args = new[] {
                "--ignore-config",
                //@"--plugin-path=" + Path.Combine(PluginPath , "plugins")                
                //@"--plugin-path=C:\Program Files (x86)\VideoLAN\VLC\plugins"
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

        private void PlayVideo()
        {
            OpenFileDialog OpenFileDialog1 = new OpenFileDialog();


            if (OpenFileDialog1.ShowDialog() == DialogResult.OK)
                return;

            PlayVideo(OpenFileDialog1.FileName);
        }

        private void PlayVideo(String fileName)
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

        public void PlayVideo(Video video)
        {
            _video = video;
            if (video != null && video.Path != null)
            {
                PlayVideo(video.Path);
                _player.CurrentTimestamp = (long)video.LastPlayLocation;
            }
        }


        public void Pause()
        {
            if (_player.Media.State != MediaPlayerState.Paused)
                _player.Pause();
            else
                _player.Play();
        }

        public void Stop()
        {
            Thread t = new Thread(new ThreadStart(_player.Stop));
            t.Start();
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
                Pause();

            //audio
            else if (keys == Keys.M)
                _player.Mute();
        }

        private void VlcWinForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            VlcLibInterop.StopVideo(_player);
            //Thread t = new Thread(new ThreadStart(HandleCloseForm));
            //t.Start();
            //while (t.IsAlive)
            //{
            //    Thread.Sleep(50);
            //}
            //_overlayForm.Close();
        }

        private void HandleCloseForm()
        {
            if (_player.Media.State != MediaPlayerState.Stopped)
            {
                if (_player.VideoLength > 0 && _player.CurrentTimestamp > 0)
                    _video.MarkAsSeen((ulong) _player.VideoLength, (ulong) _player.CurrentTimestamp,
                                      _pnlControls.VideoEndReached);
            }
            _pnlControls.DetachFromEvents();
            _player.Dispose();
            _vlcInstance.Release();

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
                _player.Stop();
                Thread t = new Thread(HandleCloseForm);
                t.Start();
                
            }
            base.Dispose(disposing);
        }


    }
}
