using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;

namespace VlcPlayer.Events
{
    internal static class VlcEventManagerInterop
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void VlcEventHandlerDelegate(ref libvlc_event_t libvlc_event, IntPtr userData);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurityAttribute]
        public static extern IntPtr libvlc_media_player_event_manager(IntPtr libvlc_media_player_t);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int libvlc_event_attach(IntPtr p_event_manager, libvlc_event_e i_event_type, IntPtr f_callback, IntPtr user_data);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_event_detach(IntPtr p_event_manager, libvlc_event_e i_event_type, IntPtr f_callback, IntPtr user_data);

    }

    public class VlcEventManager
    {
        protected VlcMediaPlayer m_eventProvider;
        readonly List<VlcEventManagerInterop.VlcEventHandlerDelegate> m_callbacks = new List<VlcEventManagerInterop.VlcEventHandlerDelegate>();
        readonly IntPtr hCallback1;
        private readonly IntPtr _mediaPlayer;

        public VlcEventManager(VlcMediaPlayer eventProvider)
        {
            m_eventProvider = eventProvider;
            _mediaPlayer = VlcEventManagerInterop.libvlc_media_player_event_manager(eventProvider.Handle);

            VlcEventManagerInterop.VlcEventHandlerDelegate callback1 = MediaPlayerEventOccured;

            hCallback1 = Marshal.GetFunctionPointerForDelegate(callback1);
            m_callbacks.Add(callback1);

            GC.KeepAlive(callback1);
        }

        protected void Attach(libvlc_event_e eType)
        {
            if (VlcEventManagerInterop.libvlc_event_attach(_mediaPlayer, eType, hCallback1, IntPtr.Zero) != 0)
            {
                throw new OutOfMemoryException("Failed to subscribe to event notification");
            }
        }

        protected void Dettach(libvlc_event_e eType)
        {
            VlcEventManagerInterop.libvlc_event_detach(_mediaPlayer, eType, hCallback1, IntPtr.Zero);
        }

        protected void MediaPlayerEventOccured(ref libvlc_event_t libvlc_event, IntPtr userData)
        {
            switch (libvlc_event.type)
            {
                case libvlc_event_e.libvlc_MediaPlayerTimeChanged:
                    RaiseTimeChanged(libvlc_event.media_player_time_changed.new_time);
                    break;

                case libvlc_event_e.libvlc_MediaPlayerEndReached:
                    RaiseMediaEnded();
                    break;

                case libvlc_event_e.libvlc_MediaPlayerOpening:
                    if (m_playerOpening != null)
                    {
                        m_playerOpening(m_eventProvider, EventArgs.Empty);
                    }
                    break;

                case libvlc_event_e.libvlc_MediaPlayerBuffering:
                    if (m_playerBuffering != null)
                    {
                        m_playerBuffering(m_eventProvider, EventArgs.Empty);
                    }
                    break;

                case libvlc_event_e.libvlc_MediaPlayerPlaying:
                    if (m_playerPlaying != null)
                    {
                        m_playerPlaying(m_eventProvider, EventArgs.Empty);
                    }
                    break;

                case libvlc_event_e.libvlc_MediaPlayerPaused:
                    if (m_playerPaused != null)
                    {
                        m_playerPaused(m_eventProvider, EventArgs.Empty);
                    }
                    break;

                case libvlc_event_e.libvlc_MediaPlayerStopped:
                    if (m_playerStopped != null)
                    {
                        m_playerStopped(m_eventProvider, EventArgs.Empty);
                    }
                    break;

                case libvlc_event_e.libvlc_MediaPlayerForward:
                    if (m_playerForward != null)
                    {
                        m_playerForward(m_eventProvider, EventArgs.Empty);
                    }
                    break;

                case libvlc_event_e.libvlc_MediaPlayerBackward:
                    if (m_playerPaused != null)
                    {
                        m_playerPaused(m_eventProvider, EventArgs.Empty);
                    }
                    break;

                case libvlc_event_e.libvlc_MediaPlayerEncounteredError:
                    if (m_playerEncounteredError != null)
                    {
                        m_playerEncounteredError(m_eventProvider, EventArgs.Empty);
                    }
                    break;

                case libvlc_event_e.libvlc_MediaPlayerPositionChanged:
                    if (m_playerPositionChanged != null)
                    {
                        m_playerPositionChanged(m_eventProvider, new MediaPlayerPositionChanged(libvlc_event.media_player_position_changed.new_position));
                    }
                    break;

                case libvlc_event_e.libvlc_MediaPlayerSeekableChanged:
                    if (m_playerSeekableChanged != null)
                    {
                        m_playerSeekableChanged(m_eventProvider, new MediaPlayerSeekableChanged(libvlc_event.media_player_seekable_changed.new_seekable));
                    }
                    break;

                case libvlc_event_e.libvlc_MediaPlayerPausableChanged:
                    if (m_playerPausableChanged != null)
                    {
                        m_playerPausableChanged(m_eventProvider, new MediaPlayerPausableChanged(libvlc_event.media_player_pausable_changed.new_pausable));
                    }
                    break;

                case libvlc_event_e.libvlc_MediaPlayerLengthChanged:
                    if (m_playerLengthChanged != null)
                    {
                        m_playerLengthChanged(m_eventProvider, new MediaPlayerLengthChanged(libvlc_event.media_player_length_changed.new_length));
                    }
                    break;
            }

        }

        private void RaiseTimeChanged(long p)
        {
            if (m_timeChanged != null)
            {
                m_timeChanged(m_eventProvider, new MediaPlayerTimeChanged(p));
            }
        }

        internal void RaiseMediaEnded()
        {
            if (m_mediaEnded != null)
            {
                m_mediaEnded.BeginInvoke(m_eventProvider, EventArgs.Empty, null, null);
            }
        }

        private event EventHandler<MediaPlayerTimeChanged> m_timeChanged;
        public event EventHandler<MediaPlayerTimeChanged> TimeChanged
        {
            add
            {
                if (m_timeChanged == null)
                {
                    Attach(libvlc_event_e.libvlc_MediaPlayerTimeChanged);
                }
                m_timeChanged += value;
            }
            remove
            {
                if (m_timeChanged != null)
                {
                    m_timeChanged -= value;
                    if (m_timeChanged == null)
                    {
                        Dettach(libvlc_event_e.libvlc_MediaPlayerTimeChanged);
                    }
                }
            }
        }

        private event EventHandler m_mediaEnded;
        public event EventHandler MediaEnded
        {
            add
            {
                if (m_mediaEnded == null)
                {
                    Attach(libvlc_event_e.libvlc_MediaPlayerEndReached);
                }
                m_mediaEnded += value;
            }
            remove
            {
                if (m_mediaEnded != null)
                {
                    m_mediaEnded -= value;
                    if (m_mediaEnded == null)
                    {
                        Dettach(libvlc_event_e.libvlc_MediaPlayerEndReached);
                    }
                }
            }
        }

        #region IEventBroker Members

        event EventHandler m_playerOpening;
        public event EventHandler PlayerOpening
        {
            add
            {
                if (m_playerOpening == null)
                {
                    Attach(libvlc_event_e.libvlc_MediaPlayerOpening);
                }
                m_playerOpening += value;
            }
            remove
            {
                if (m_playerOpening != null)
                {
                    m_playerOpening -= value;
                    if (m_playerOpening == null)
                    {
                        Dettach(libvlc_event_e.libvlc_MediaPlayerOpening);
                    }
                }
            }
        }

        event EventHandler m_playerBuffering;
        public event EventHandler PlayerBuffering
        {
            add
            {
                if (m_playerBuffering == null)
                {
                    Attach(libvlc_event_e.libvlc_MediaPlayerBuffering);
                }
                m_playerBuffering += value;
            }
            remove
            {
                if (m_playerBuffering != null)
                {
                    m_playerBuffering -= value;
                    if (m_playerBuffering == null)
                    {
                        Dettach(libvlc_event_e.libvlc_MediaPlayerBuffering);
                    }
                }
            }
        }

        event EventHandler m_playerPlaying;
        public event EventHandler PlayerPlaying
        {
            add
            {
                if (m_playerPlaying == null)
                {
                    Attach(libvlc_event_e.libvlc_MediaPlayerPlaying);
                }
                m_playerPlaying += value;
            }
            remove
            {
                if (m_playerPlaying != null)
                {
                    m_playerPlaying -= value;
                    if (m_playerPlaying == null)
                    {
                        Dettach(libvlc_event_e.libvlc_MediaPlayerPlaying);
                    }
                }
            }
        }

        event EventHandler m_playerPaused;
        public event EventHandler PlayerPaused
        {
            add
            {
                if (m_playerPaused == null)
                {
                    Attach(libvlc_event_e.libvlc_MediaPlayerPaused);
                }
                m_playerPaused += value;
            }
            remove
            {
                if (m_playerPaused != null)
                {
                    m_playerPaused -= value;
                    if (m_playerPaused == null)
                    {
                        Dettach(libvlc_event_e.libvlc_MediaPlayerPaused);
                    }
                }
            }
        }

        event EventHandler m_playerStopped;
        public event EventHandler PlayerStopped
        {
            add
            {
                if (m_playerStopped == null)
                {
                    Attach(libvlc_event_e.libvlc_MediaPlayerStopped);
                }
                m_playerStopped += value;
            }
            remove
            {
                if (m_playerStopped != null)
                {
                    m_playerStopped -= value;
                    if (m_playerStopped == null)
                    {
                        Dettach(libvlc_event_e.libvlc_MediaPlayerStopped);
                    }
                }
            }
        }

        event EventHandler m_playerForward;
        public event EventHandler PlayerForward
        {
            add
            {
                if (m_playerForward == null)
                {
                    Attach(libvlc_event_e.libvlc_MediaPlayerForward);
                }
                m_playerForward += value;
            }
            remove
            {
                if (m_playerForward != null)
                {
                    m_playerForward -= value;
                    if (m_playerForward == null)
                    {
                        Dettach(libvlc_event_e.libvlc_MediaPlayerForward);
                    }
                }
            }
        }

        event EventHandler m_playerBackward;
        public event EventHandler PlayerBackward
        {
            add
            {
                if (m_playerBackward == null)
                {
                    Attach(libvlc_event_e.libvlc_MediaPlayerBackward);
                }
                m_playerBackward += value;
            }
            remove
            {
                if (m_playerBackward != null)
                {
                    m_playerBackward -= value;
                    if (m_playerBackward == null)
                    {
                        Dettach(libvlc_event_e.libvlc_MediaPlayerBackward);
                    }
                }
            }
        }

        event EventHandler m_playerEncounteredError;
        public event EventHandler PlayerEncounteredError
        {
            add
            {
                if (m_playerEncounteredError == null)
                {
                    Attach(libvlc_event_e.libvlc_MediaPlayerEncounteredError);
                }
                m_playerEncounteredError += value;
            }
            remove
            {
                if (m_playerEncounteredError != null)
                {
                    m_playerEncounteredError -= value;
                    if (m_playerEncounteredError == null)
                    {
                        Dettach(libvlc_event_e.libvlc_MediaPlayerEncounteredError);
                    }
                }
            }
        }

        event EventHandler<MediaPlayerPositionChanged> m_playerPositionChanged;
        public event EventHandler<MediaPlayerPositionChanged> PlayerPositionChanged
        {
            add
            {
                if (m_playerPositionChanged == null)
                {
                    Attach(libvlc_event_e.libvlc_MediaPlayerPositionChanged);
                }
                m_playerPositionChanged += value;
            }
            remove
            {
                if (m_playerPositionChanged != null)
                {
                    m_playerPositionChanged -= value;
                    if (m_playerPositionChanged == null)
                    {
                        Dettach(libvlc_event_e.libvlc_MediaPlayerPositionChanged);
                    }
                }
            }
        }

        event EventHandler<MediaPlayerSeekableChanged> m_playerSeekableChanged;
        public event EventHandler<MediaPlayerSeekableChanged> PlayerSeekableChanged
        {
            add
            {
                if (m_playerSeekableChanged == null)
                {
                    Attach(libvlc_event_e.libvlc_MediaPlayerSeekableChanged);
                }
                m_playerSeekableChanged += value;
            }
            remove
            {
                if (m_playerSeekableChanged != null)
                {
                    m_playerSeekableChanged -= value;
                    if (m_playerSeekableChanged == null)
                    {
                        Dettach(libvlc_event_e.libvlc_MediaPlayerSeekableChanged);
                    }
                }
            }
        }

        event EventHandler<MediaPlayerPausableChanged> m_playerPausableChanged;
        public event EventHandler<MediaPlayerPausableChanged> PlayerPausableChanged
        {
            add
            {
                if (m_playerPausableChanged == null)
                {
                    Attach(libvlc_event_e.libvlc_MediaPlayerPausableChanged);
                }
                m_playerPausableChanged += value;
            }
            remove
            {
                if (m_playerPausableChanged != null)
                {
                    m_playerPausableChanged -= value;
                    if (m_playerPausableChanged == null)
                    {
                        Dettach(libvlc_event_e.libvlc_MediaPlayerPausableChanged);
                    }
                }
            }
        }

        event EventHandler<MediaPlayerLengthChanged> m_playerLengthChanged;
        public event EventHandler<MediaPlayerLengthChanged> PlayerLengthChanged
        {
            add
            {
                if (m_playerLengthChanged == null)
                {
                    Attach(libvlc_event_e.libvlc_MediaPlayerLengthChanged);
                }
                m_playerLengthChanged += value;
            }
            remove
            {
                if (m_playerLengthChanged != null)
                {
                    m_playerLengthChanged -= value;
                    if (m_playerLengthChanged == null)
                    {
                        Dettach(libvlc_event_e.libvlc_MediaPlayerLengthChanged);
                    }
                }
            }
        }

        #endregion

    }
}
