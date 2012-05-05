using System;
using VlcPlayer.Enums;

namespace VlcPlayer
{
    public class VlcMedia : IDisposable
    {
        internal IntPtr Handle;

        public VlcMedia(VlcInstance instance, string url)
        {
            Handle = LibVlc.libvlc_media_new_path(instance.Handle, url);
            if (Handle == IntPtr.Zero) throw new VlcException();
        }

        internal VlcMedia(IntPtr handle)
        {
            Handle = handle;
        }

        public MediaPlayerState State
        {
            get { return LibVlc.libvlc_media_get_state(Handle); }
        }

        public void Release()
        {
            LibVlc.libvlc_media_release(Handle);
        }

        public void Dispose()
        {
        }
    }
}
