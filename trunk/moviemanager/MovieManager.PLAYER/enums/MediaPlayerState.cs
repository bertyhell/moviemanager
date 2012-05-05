namespace MovieManager.PLAYER.enums
{
    /// <summary>
    /// Possible state of the media objects
    /// </summary>
    public enum MediaPlayerState
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