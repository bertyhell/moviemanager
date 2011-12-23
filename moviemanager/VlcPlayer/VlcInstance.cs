using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public void Dispose()
        {
            LibVlc.libvlc_release(Handle);
        }
    }
}
