using System;
using Tmc.WinUI.Player.enums;
using Tmc.WinUI.Player.interop;

namespace Tmc.WinUI.Player
{
    public class VlcMedia : IDisposable
    {
        #region interop
        internal IntPtr _handle;
        #endregion

        public VlcMedia(VlcInstance instance, string url)
        {
            VlcLibInterop.InitializeMedia(this, instance, url);
        }

        internal VlcMedia(IntPtr handle)
        {
            _handle = handle;
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
