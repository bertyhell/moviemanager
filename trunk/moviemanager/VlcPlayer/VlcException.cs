using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace VlcPlayer
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct libvlc_exception_t
    {
        public int b_raised;
        public int i_code;
        private IntPtr psz_message;

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
            IntPtr errorPointer = LibVlc.libvlc_errmsg();
            Err = errorPointer == IntPtr.Zero ? "VLC Exception"
                : Marshal.PtrToStringAuto(errorPointer);
        }

        public VlcException(string exception)
        {
            Err = exception;
        }

        public override string Message { get { return Err; } }
    }
}
