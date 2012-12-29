using System;
using System.Windows.Forms;
using Tmc.WinUI.Player.enums;
using Tmc.WinUI.Player.interop;

namespace Tmc.WinUI.Player
{
    public class VlcMediaPlayer
    {
        #region interop

        internal IntPtr _handle;

        #endregion
        
        public VlcMediaPlayer(VlcMedia media, Panel panel)
        {
            VlcLibInterop.InitializeMediaPlayer(this, media);
            VlcLibInterop.SetDisplayPanelForPlayer(this, panel);
        }

        public VlcMedia Media
        {
            get { return VlcLibInterop.GetMediaFromPlayer(this); }
            set
            {
                VlcLibInterop.SetMediaForPlayer(this, value);
            }

        }

        #region methods

        /// <summary>
        /// Start playing video
        /// </summary>
        public void Play()
        {
            VlcLibInterop.PlayVideo(this);
        }

        /// <summary>
        /// Pause the current video
        /// </summary>
        public void Pause()
        {
            VlcLibInterop.PauseVideo(this);
        }

        /// <summary>
        /// Stop the current video
        /// </summary>
        public void Stop()
        {
            if (Media.State != MediaPlayerState.Stopped)
            {
                VlcLibInterop.StopVideo(this);
            }
        }

        /// <summary>
        /// Releases the media player in vlc lib
        /// </summary>
        public void Release()
        {
            VlcLibInterop.StopVideo(this);
        }

        public void Mute()
        {
            VlcLibInterop.MutePlayer(this);
        }

        public long VideoLength
        {
            get { return VlcLibInterop.GetMediaLength(this); }
        }

        public long CurrentTimestamp
        {
            get { return VlcLibInterop.GetCurrentTimestamp(this); }
            set { VlcLibInterop.SetCurrentTimestamp(this, value); }
        }

        #endregion

        #region volume

        public int Volume
        {
            get { return VlcLibInterop.GetAudioVolume(this); }
            set { VlcLibInterop.SetAudioVolume(this, value); }
        }

        public void RaiseVolume()
        {
            Volume = (Volume < 175 ? Volume + 25 : 200);
        }

        public void LowerVolume()
        {
            Volume = (Volume > 25 ? Volume - 25 : 0);
        }

        #endregion

        #region events

        #endregion
    }
}
