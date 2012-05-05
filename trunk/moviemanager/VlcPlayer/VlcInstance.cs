using System;

namespace VlcPlayer
{
    public class VlcInstance : IDisposable
    {
        internal IntPtr Handle;

        public VlcInstance(string[] args)
        {
            Handle = LibVlc.libvlc_new(args.Length, args);
            if (Handle == IntPtr.Zero) throw new VlcException();
        }

        public void Release()
        {
            if (Handle != IntPtr.Zero)
            {
                LibVlc.libvlc_release(Handle);
                Handle = IntPtr.Zero;
            }
        }

        public void Dispose()
        {
            Release();
        }
    }
}
