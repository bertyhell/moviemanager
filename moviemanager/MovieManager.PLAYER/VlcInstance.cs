using System;
using MovieManager.PLAYER.interop;

namespace MovieManager.PLAYER
{
    public class VlcInstance : IDisposable
    {
        internal IntPtr Handle;

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
