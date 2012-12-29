using System;
using Tmc.WinUI.Player.interop;

namespace Tmc.WinUI.Player
{
    public class VlcInstance : IDisposable
    {
        internal IntPtr _handle;

        public VlcInstance(string[] args)
        {
            VlcLibInterop.InitializeVlcInstance(this, args);
        }

        public void Release()
        {
            VlcLibInterop.ReleaseVlcInstance(this);
        }

        public void Dispose()
        {
            Release();
        }
    }
}
