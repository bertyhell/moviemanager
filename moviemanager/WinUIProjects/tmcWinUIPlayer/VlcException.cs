using System;
using System.Runtime.InteropServices;
using Tmc.WinUI.Player.interop;

namespace Tmc.WinUI.Player
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct LibvlcException
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
        protected string _err;

        public VlcException()
        {
            IntPtr ErrorPointer = VlcLib.libvlc_errmsg();
            _err = ErrorPointer == IntPtr.Zero ? "VLC Exception"
                : Marshal.PtrToStringAuto(ErrorPointer);
        }

        public VlcException(string exception)
        {
            _err = exception;
        }

        public override string Message { get { return _err; } }
    }
}
