using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace VlcPlayer
{
    public class VlcMediaPlayer : IDisposable
    {
        internal IntPtr Handle;
        private IntPtr _drawable;
        private bool _playing, _paused;
        private VlcEventManager _eventManager;

        public VlcMediaPlayer(VlcMedia media, VlcWinForm parentForm)
        {
            Handle = LibVlc.libvlc_media_player_new_from_media(media.Handle);
            _eventManager = new VlcEventManager(this);
            _eventManager.EventReceivers.Add(parentForm);
            _eventManager.InitializeEventManager();
            if (Handle == IntPtr.Zero) throw new VlcException();
        }

        public void Dispose()
        {
            LibVlc.libvlc_media_player_release(Handle);
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

        public bool IsPlaying { get { return _playing && !_paused; } }

        public bool IsPaused { get { return _playing && _paused; } }

        public bool IsStopped { get { return !_playing; } }

        #region methods

        public void Play()
        {
            int ret = LibVlc.libvlc_media_player_play(Handle);
            if (ret == -1)
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

        public void Stop()
        {
            LibVlc.libvlc_media_player_stop(Handle);

            _playing = false;
            _paused = false;
        }

        public void Mute()
        {
            LibVlc.libvlc_audio_toggle_mute(Handle);
        }

        public ulong GetVideoLength()
        {
            libvlc_exception_t Ex = new libvlc_exception_t();
            return (ulong)LibVlc.libvlc_media_player_get_length(Handle, ref Ex);
        }

        public ulong GetCurrentTimestamp()
        {
            libvlc_exception_t Ex = new libvlc_exception_t();
            return (ulong)LibVlc.libvlc_media_player_get_time(Handle, ref Ex);
        }

        public void SetCurrentTimestamp(ulong timestamp)
        {
            libvlc_exception_t Ex = new libvlc_exception_t();
            LibVlc.libvlc_media_player_set_time(Handle,(Int64)timestamp, ref Ex);
        }

        #endregion
    }
}
