using System;
using Tmc.WinUI.Player.enums;

namespace Tmc.WinUI.Player.interop
{
    public class VlcLibInterop
    {
        public static void InitializeVlcInstance(VlcInstance instance, string[] args)
        {
            instance._handle = VlcLib.libvlc_new(args.Length, args);
            if (instance._handle == IntPtr.Zero) throw new VlcException();
        }

        public static void ReleaseVlcInstance(VlcInstance instance)
        {
            if (instance._handle != IntPtr.Zero)
            {
                VlcLib.libvlc_release(instance._handle);
                instance._handle = IntPtr.Zero;
            }
        }

        #region player

        public static void InitializeMediaPlayer(VlcMediaPlayer player, VlcMedia media)
        {
            IntPtr Handle = VlcLib.libvlc_media_player_new_from_media(media._handle);

            if (Handle == IntPtr.Zero) throw new VlcException();

            player._handle = Handle;
        }

        public static void PlayVideo(VlcMediaPlayer player)
        {
            VlcLib.libvlc_media_player_play(player._handle);
        }

        public static void PauseVideo(VlcMediaPlayer player)
        {
            VlcLib.libvlc_media_player_pause(player._handle);
        }

        public static void StopVideo(VlcMediaPlayer player)
        {
            LibvlcException Ex = new LibvlcException();
            VlcLib.libvlc_media_player_stop(player._handle, ref Ex);
            if (Ex.b_raised != 0)
                throw new VlcException(Ex.Message);
        }

        public static long GetCurrentTimestamp(VlcMediaPlayer player)
        {
            return VlcLib.libvlc_media_player_get_time(player._handle);
        }

        public static void SetCurrentTimestamp(VlcMediaPlayer player, long timestamp)
        {
            VlcLib.libvlc_media_player_set_time(player._handle, timestamp);
        }


        #endregion

        public static void SetMediaForPlayer(VlcMediaPlayer player, VlcMedia media)
        {
            VlcLib.libvlc_media_player_set_media(player._handle, media._handle);
        }

        public static void SetDisplayPanelForPlayer(VlcMediaPlayer player, System.Windows.Forms.Panel panel)
        {
            VlcLib.libvlc_media_player_set_hwnd(player._handle, panel.Handle);
        }

        #region media

        public static void InitializeMedia(VlcMedia media, VlcInstance instance, string url)
        {
            media._handle = VlcLib.libvlc_media_new_path(instance._handle, url);
            if (media._handle == IntPtr.Zero) throw new VlcException();
        }

        public static void ReleaseMedia(VlcMedia media)
        {
            VlcLib.libvlc_media_release(media._handle);
        }

        public static long GetMediaLength(VlcMediaPlayer player)
        {
            return VlcLib.libvlc_media_player_get_length(player._handle);
        }

        public static MediaPlayerState GetMediaState(VlcMedia media)
        {
            return VlcLib.libvlc_media_get_state(media._handle);
        }

        public static VlcMedia GetMediaFromPlayer(VlcMediaPlayer player)
        {

            IntPtr Media = VlcLib.libvlc_media_player_get_media(player._handle);
            return Media == IntPtr.Zero ? null : new VlcMedia(Media);
        }

        #endregion

        #region audio

        public static int GetAudioVolume(VlcMediaPlayer player)
        {
            return VlcLib.libvlc_audio_get_volume(player._handle);
        }

        public static void SetAudioVolume(VlcMediaPlayer player, int volume)
        {
            VlcLib.libvlc_audio_set_volume(player._handle, volume);
        }

        public static void MutePlayer(VlcMediaPlayer player)
        {
            VlcLib.libvlc_audio_toggle_mute(player._handle);
        }

        #endregion

    }
}
