using System;
using System.Runtime.InteropServices;

namespace VlcPlayer
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct libvlc_exception_t
    {
        public int b_raised;
        public int i_code;
        private readonly IntPtr psz_message;

        public string Message
        {
            get
            {
                return (psz_message != IntPtr.Zero) ? (Marshal.PtrToStringAnsi(psz_message)) : (null);
            }
        }
    }

    public class VlcException : Exception
    {
        protected string Err;

        public VlcException()
        {
            IntPtr ErrorPointer = LibVlc.libvlc_errmsg();
            Err = ErrorPointer == IntPtr.Zero ? "VLC Exception"
                : Marshal.PtrToStringAuto(ErrorPointer);
        }

        public VlcException(string exception)
        {
            Err = exception;
        }

        public override string Message { get { return Err; } }
    }
}
