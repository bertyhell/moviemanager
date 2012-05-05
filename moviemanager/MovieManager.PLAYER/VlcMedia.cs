using System;
using MovieManager.PLAYER.enums;
using MovieManager.PLAYER.interop;

namespace MovieManager.PLAYER
{
    public class VlcMedia : IDisposable
    {
        #region interop
        internal IntPtr Handle;
        #endregion

        public VlcMedia(VlcInstance instance, string url)
        {
            VlcLibInterop.InitializeMedia(this, instance, url);
        }

        internal VlcMedia(IntPtr handle)
        {
            Handle = handle;
        }

        public MediaPlayerState State
        {
            get { return VlcLibInterop.GetMediaState(this); }
        }

        public void Release()
        {
            VlcLibInterop.ReleaseMedia(this);
        }

        public void Dispose()
        {
        }
    }
}
