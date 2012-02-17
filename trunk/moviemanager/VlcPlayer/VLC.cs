using System;
using System.Runtime.InteropServices;
using System.Security;

namespace VlcPlayer
{
    // http://www.videolan.org/developers/vlc/doc/doxygen/html/group__libvlc.html

    internal static class LibVlc
    {
        #region core
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_new(int argc, [MarshalAs(UnmanagedType.LPArray,
          ArraySubType = UnmanagedType.LPStr)] string[] argv);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_release(IntPtr instance);
        #endregion

        #region media
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_media_new_location(IntPtr pINSTANCE,
          [MarshalAs(UnmanagedType.LPStr)] string pszMrl);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_release(IntPtr pMetaDesc);
        #endregion

        #region media player
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_media_player_new_from_media(IntPtr media);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_player_release(IntPtr player);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_player_set_hwnd(IntPtr player, IntPtr drawable);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_media_player_get_media(IntPtr player);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_player_set_media(IntPtr player, IntPtr media);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_media_player_play(IntPtr player);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_player_pause(IntPtr player);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_audio_toggle_mute(IntPtr player);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_player_stop(IntPtr player);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern Int64 libvlc_media_player_get_length(IntPtr libvlc_mediaplayer, ref libvlc_exception_t exception);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern Int64 libvlc_media_player_get_time(IntPtr libvlc_mediaplayer, ref libvlc_exception_t exception);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void libvlc_media_player_set_time(IntPtr libvlc_mediaplayer, Int64 time, ref libvlc_exception_t exception);

        #endregion

        #region exception
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_clearerr();

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_errmsg();
        #endregion
    }

    //public class VlcInstance : IDisposable
    //{
    //    internal IntPtr Handle;

    //    public VlcInstance(string[] args)
    //    {
    //        Handle = LibVlc.libvlc_new(args.Length, args);
    //        if (Handle == IntPtr.Zero) throw new VlcException();
    //    }

    //    public void Dispose()
    //    {
    //        LibVlc.libvlc_release(Handle);
    //    }
    //}

    //public class VlcMedia : IDisposable
    //{
    //    internal IntPtr Handle;

    //    public VlcMedia(VlcInstance instance, string url)
    //    {
    //        Handle = LibVlc.libvlc_media_new_location(instance.Handle, url);
    //        if (Handle == IntPtr.Zero) throw new VlcException();
    //    }

    //    internal VlcMedia(IntPtr handle)
    //    {
    //        Handle = handle;
    //    }

    //    public void Dispose()
    //    {
    //        LibVlc.libvlc_media_release(Handle);
    //    }
    //}

    //public class VlcMediaPlayer : IDisposable
    //{
    //    internal IntPtr Handle;
    //    private IntPtr _drawable;
    //    private bool _playing, _paused;
    //    private VlcEventManager _eventManager;

    //    public VlcMediaPlayer(VlcMedia media)
    //    {
    //        Handle = LibVlc.libvlc_media_player_new_from_media(media.Handle);
    //        _eventManager = new VlcEventManager(this);
    //        if (Handle == IntPtr.Zero) throw new VlcException();
    //    }

    //    public void Dispose()
    //    {
    //        LibVlc.libvlc_media_player_release(Handle);
    //    }

    //    public IntPtr Drawable
    //    {
    //        get
    //        {
    //            return _drawable;
    //        }
    //        set
    //        {
    //            LibVlc.libvlc_media_player_set_hwnd(Handle, value);
    //            _drawable = value;
    //        }
    //    }

    //    public VlcMedia Media
    //    {
    //        get
    //        {
    //            IntPtr media = LibVlc.libvlc_media_player_get_media(Handle);
    //            if (media == IntPtr.Zero) return null;
    //            return new VlcMedia(media);
    //        }
    //        set
    //        {
    //            LibVlc.libvlc_media_player_set_media(Handle, value.Handle);
    //        }
    //    }

    //    public bool IsPlaying { get { return _playing && !_paused; } }

    //    public bool IsPaused { get { return _playing && _paused; } }

    //    public bool IsStopped { get { return !_playing; } }

    //    #region methods

    //    public void Play()
    //    {
    //        int ret = LibVlc.libvlc_media_player_play(Handle);
    //        if (ret == -1)
    //            throw new VlcException();

    //        _playing = true;
    //        _paused = false;
    //    }

    //    public void Pause()
    //    {
    //        LibVlc.libvlc_media_player_pause(Handle);

    //        if (_playing)
    //            _paused ^= true;
    //    }

    //    public void Stop()
    //    {
    //        LibVlc.libvlc_media_player_stop(Handle);

    //        _playing = false;
    //        _paused = false;
    //    }

    //    public void Mute()
    //    {
    //        LibVlc.libvlc_audio_toggle_mute(Handle);
    //    }

    //    public ulong GetVideoLength()
    //    {
    //        libvlc_exception_t Ex = new libvlc_exception_t();
    //        return (ulong)LibVlc.libvlc_media_player_get_length(Handle, ref Ex);
    //    }

    //    #endregion
    //}
}
