using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace VlcPlayer
{
    internal class VlcEventManagerInterop
    {
        public VlcEventManagerInterop()
        {

        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void VlcEventHandlerDelegate(IntPtr libvlc_event, IntPtr userData);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurityAttribute]
        public static extern IntPtr libvlc_media_player_event_manager(IntPtr libvlc_media_player_t, ref libvlc_exception_t exception);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_event_attach(IntPtr p_event_manager, libvlc_event_type_t i_event_type, IntPtr f_callback, IntPtr user_data);

    }

    public class VlcEventManager
    {
        private VlcMediaPlayer _player;
        private List<IVlcEventReceiver> _receivers =new List<IVlcEventReceiver>(); 

        /// <summary>
        /// List for store used delegates. This delegates should not be disposed before disposing VlcPlayer.
        /// </summary>
        private readonly List<Delegate> _eventDelegates = new List<Delegate>();

        public VlcEventManager(VlcMediaPlayer player)
        {
            if (player != null)
            {
                _player = player;
            }
        }

        public List<IVlcEventReceiver> EventReceivers
        {
            get { return _receivers; }
            set { _receivers = value; }
        } 

        public void InitializeEventManager()
        {
            libvlc_exception_t exc = new libvlc_exception_t();
            IntPtr EventManager = VlcEventManagerInterop.libvlc_media_player_event_manager(_player.Handle, ref exc);
            if (0 != exc.b_raised)
            {
                throw new VlcException(exc.Message);
            }
            // Attaching to first player
            AttachToEvent(EventManager, new VlcEventManagerInterop.VlcEventHandlerDelegate(mediaPlayer_TimeChanged), libvlc_event_type_t.libvlc_MediaPlayerTimeChanged);
            AttachToEvent(EventManager, new VlcEventManagerInterop.VlcEventHandlerDelegate(mediaPlayer_Paused), libvlc_event_type_t.libvlc_MediaPlayerPaused);
            AttachToEvent(EventManager, new VlcEventManagerInterop.VlcEventHandlerDelegate(mediaPlayer_Paused), libvlc_event_type_t.libvlc_MediaPlayerPlaying);
            AttachToEvent(EventManager, new VlcEventManagerInterop.VlcEventHandlerDelegate(mediaPlayer_Paused), libvlc_event_type_t.libvlc_MediaPlayerStopped);
            AttachToEvent(EventManager, new VlcEventManagerInterop.VlcEventHandlerDelegate(mediaPlayer_Paused), libvlc_event_type_t.libvlc_MediaPlayerOpening);
        }

        private void AttachToEvent(IntPtr eventManager, Delegate eventHandlerDelegate, libvlc_event_type_t eventType)
        {
            if (eventManager == IntPtr.Zero)
            {
                throw new ArgumentException("IntPtr.Zero is invalid value.", "eventManager");
            }
            if (eventHandlerDelegate == null)
            {
                throw new ArgumentNullException("eventHandlerDelegate");
            }
            VlcEventManagerInterop.libvlc_event_attach(eventManager, eventType, Marshal.GetFunctionPointerForDelegate(eventHandlerDelegate), IntPtr.Zero);
            // Save delegate to a private list (to suppress finalizing it)
            _eventDelegates.Add(eventHandlerDelegate);
        }

        private void mediaPlayer_TimeChanged(IntPtr libvlc_event, IntPtr data)
        {
            foreach (IVlcEventReceiver receiver in _receivers)
            {
                receiver.OnTimeChanged();
            }
        }

        private void mediaPlayer_Paused(IntPtr libvlc_event, IntPtr userdata)
        {
            foreach (IVlcEventReceiver receiver in _receivers)
            {
                receiver.OnPausedChanged();
            }
        }

    }
}
