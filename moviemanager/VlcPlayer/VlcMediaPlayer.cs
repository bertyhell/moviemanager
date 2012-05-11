using System;
using System.Threading;
using MovieManager.PLAYER;
using VlcPlayer.Enums;
using VlcPlayer.Events;

namespace VlcPlayer
{
    public class VlcMediaPlayer
    {
        internal IntPtr Handle;
        private IntPtr _drawable;
        private bool _playing, _paused;

        public delegate void OnIsBusyChanged(object sender, EventArgs e);

        public event OnIsBusyChanged IsBusyChanged;

        public VlcMediaPlayer(VlcMedia media, MMPlayer parentForm)
        {
            Handle = LibVlc.libvlc_media_player_new_from_media(media.Handle);
            _eventManager = new VlcEventManager(this);
            if (Handle == IntPtr.Zero) throw new VlcException();
            else Media = media;
        }

        public void Dispose()
        {
            HandleClosing();
        }

        public IntPtr Drawable
        {
            get
            {
                return _drawable;
            }
            set
            {
                LibVlc.libvlc_media_player_set_hwnd(Handle, value);
                _drawable = value;
            }
        }

        public VlcMedia Media
        {
            get
            {
                IntPtr media = LibVlc.libvlc_media_player_get_media(Handle);
                if (media == IntPtr.Zero) return null;
                return new VlcMedia(media);
            }
            set
            {
                LibVlc.libvlc_media_player_set_media(Handle, value.Handle);
            }
        }


        private readonly VlcEventManager _eventManager;
        public VlcEventManager EventManager
        {
            get { return _eventManager; }
        }

        #region methods

        public void Play()
        {
            int Ret = LibVlc.libvlc_media_player_play(Handle);
            if (Ret == -1)
                throw new VlcException();

            _playing = true;
            _paused = false;
        }

        public void Pause()
        {
            LibVlc.libvlc_media_player_pause(Handle);

            if (_playing)
                _paused ^= true;
        }

        private void HandleClosing()
        {
            while (Media.State != MediaPlayerState.Stopped)
            {
                Thread.Sleep(50);
            }
            Media.Release();
            LibVlc.libvlc_media_player_release(Handle);
        }

        public void Stop()
        {
            if (Media.State != MediaPlayerState.Stopped)
            {
                LibvlcException ex = new LibvlcException();
                LibVlc.libvlc_media_player_stop(Handle, ref ex);
                if (ex.b_raised != 0)
                    throw new VlcException(ex.Message);

                LibVlc.libvlc_media_player_release(Handle);
            }
        }

        public void Mute()
        {
            LibVlc.libvlc_audio_toggle_mute(Handle);
        }

        public long VideoLength
        {
            get { return LibVlc.libvlc_media_player_get_length(Handle); }
        }

        public long CurrentTimestamp
        {
            get { return LibVlc.libvlc_media_player_get_time(Handle); }
            set { LibVlc.libvlc_media_player_set_time(Handle, value); }
        }

        #endregion

        #region volume

        public int Volume
        {
            get { return LibVlc.libvlc_audio_get_volume(Handle); }
            set { LibVlc.libvlc_audio_set_volume(Handle, value); }
        }

#endregion

        #region events

        #endregion
    }
}
