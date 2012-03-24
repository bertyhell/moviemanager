using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VlcPlayer
{
    public enum libvlc_state_t
    {
        libvlc_NothingSpecial = 0,
        libvlc_Opening,
        libvlc_Buffering,
        libvlc_Playing,
        libvlc_Paused,
        libvlc_Stopped,
        libvlc_Ended,
        libvlc_Error
    }

    public enum libvlc_meta_t
    {
        libvlc_meta_Title,
        libvlc_meta_Artist,
        libvlc_meta_Genre,
        libvlc_meta_Copyright,
        libvlc_meta_Album,
        libvlc_meta_TrackNumber,
        libvlc_meta_Description,
        libvlc_meta_Rating,
        libvlc_meta_Date,
        libvlc_meta_Setting,
        libvlc_meta_URL,
        libvlc_meta_Language,
        libvlc_meta_NowPlaying,
        libvlc_meta_Publisher,
        libvlc_meta_EncodedBy,
        libvlc_meta_ArtworkURL,
        libvlc_meta_TrackID
    }

    public enum libvlc_event_e
    {
        libvlc_MediaMetaChanged = 0,
        libvlc_MediaSubItemAdded,
        libvlc_MediaDurationChanged,
        libvlc_MediaParsedChanged,
        libvlc_MediaFreed,
        libvlc_MediaStateChanged,

        libvlc_MediaPlayerMediaChanged = 0x100,
        libvlc_MediaPlayerNothingSpecial,
        libvlc_MediaPlayerOpening,
        libvlc_MediaPlayerBuffering,
        libvlc_MediaPlayerPlaying,
        libvlc_MediaPlayerPaused,
        libvlc_MediaPlayerStopped,
        libvlc_MediaPlayerForward,
        libvlc_MediaPlayerBackward,
        libvlc_MediaPlayerEndReached,
        libvlc_MediaPlayerEncounteredError,
        libvlc_MediaPlayerTimeChanged,
        libvlc_MediaPlayerPositionChanged,
        libvlc_MediaPlayerSeekableChanged,
        libvlc_MediaPlayerPausableChanged,
        libvlc_MediaPlayerTitleChanged,
        libvlc_MediaPlayerSnapshotTaken,
        libvlc_MediaPlayerLengthChanged,

        libvlc_MediaListItemAdded = 0x200,
        libvlc_MediaListWillAddItem,
        libvlc_MediaListItemDeleted,
        libvlc_MediaListWillDeleteItem,

        libvlc_MediaListViewItemAdded = 0x300,
        libvlc_MediaListViewWillAddItem,
        libvlc_MediaListViewItemDeleted,
        libvlc_MediaListViewWillDeleteItem,

        libvlc_MediaListPlayerPlayed = 0x400,
        libvlc_MediaListPlayerNextItemSet,
        libvlc_MediaListPlayerStopped,

        libvlc_MediaDiscovererStarted = 0x500,
        libvlc_MediaDiscovererEnded,

        libvlc_VlmMediaAdded = 0x600,
        libvlc_VlmMediaRemoved,
        libvlc_VlmMediaChanged,
        libvlc_VlmMediaInstanceStarted,
        libvlc_VlmMediaInstanceStopped,
        libvlc_VlmMediaInstanceStatusInit,
        libvlc_VlmMediaInstanceStatusOpening,
        libvlc_VlmMediaInstanceStatusPlaying,
        libvlc_VlmMediaInstanceStatusPause,
        libvlc_VlmMediaInstanceStatusEnd,
        libvlc_VlmMediaInstanceStatusError,
    }

    /// <summary>
    /// Possible state of the media objects
    /// </summary>
    public enum MediaState
    {
        NothingSpecial = 0,
        Opening,
        Buffering,
        Playing,
        Paused,
        Stopped,
        Ended,
        Error
    }

}
