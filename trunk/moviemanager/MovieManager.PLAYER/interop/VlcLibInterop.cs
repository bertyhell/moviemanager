using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MovieManager.PLAYER.enums;
using System.Windows.Forms;

namespace MovieManager.PLAYER.interop
{
    public class VlcLibInterop
    {
        public static void InitializeVlcInstance(VlcInstance instance, string[] args)
        {
            instance.Handle = VlcLib.libvlc_new(args.Length, args);
            if (instance.Handle == IntPtr.Zero) throw new VlcException();
        }

        public static void ReleaseVlcInstance(VlcInstance instance)
        {
            if (instance.Handle != IntPtr.Zero)
            {
                VlcLib.libvlc_release(instance.Handle);
                instance.Handle = IntPtr.Zero;
            }
        }

        #region player

        public static void InitializeMediaPlayer(VlcMediaPlayer player, VlcMedia media)
        {
            IntPtr Handle = VlcLib.libvlc_media_player_new_from_media(media.Handle);

            if (Handle == IntPtr.Zero) throw new VlcException();

            player.Handle = Handle;
        }

        public static void PlayVideo(VlcMediaPlayer player)
        {
            VlcLib.libvlc_media_player_play(player.Handle);
        }

        public static void PauseVideo(VlcMediaPlayer player)
        {
            VlcLib.libvlc_media_player_pause(player.Handle);
        }

        public static void StopVideo(VlcMediaPlayer player)
        {
            LibvlcException ex = new LibvlcException();
            VlcLib.libvlc_media_player_stop(player.Handle, ref ex);
            if (ex.b_raised != 0)
                throw new VlcException(ex.Message);
        }

        public static long GetCurrentTimestamp(VlcMediaPlayer player)
        {
            return VlcLib.libvlc_media_player_get_time(player.Handle);
        }

        public static void SetCurrentTimestamp(VlcMediaPlayer player, long timestamp)
        {
            VlcLib.libvlc_media_player_set_time(player.Handle, timestamp);
        }


        #endregion

        public static void SetMediaForPlayer(VlcMediaPlayer player, VlcMedia media)
        {
            VlcLib.libvlc_media_player_set_media(player.Handle, media.Handle);
        }

        public static void SetDisplayPanelForPlayer(VlcMediaPlayer player, System.Windows.Forms.Panel panel)
        {
            VlcLib.libvlc_media_player_set_hwnd(player.Handle, panel.Handle);
        }

        #region media

        public static void InitializeMedia(VlcMedia media, VlcInstance instance, string url)
        {
            media.Handle = VlcLib.libvlc_media_new_path(instance.Handle, url);
            if (media.Handle == IntPtr.Zero) throw new VlcException();
        }

        public static void ReleaseMedia(VlcMedia media)
        {
            VlcLib.libvlc_media_release(media.Handle);
        }

        public static long GetMediaLength(VlcMediaPlayer player)
        {
            return VlcLib.libvlc_media_player_get_length(player.Handle);
        }

        public static MediaPlayerState GetMediaState(VlcMedia media)
        {
            return VlcLib.libvlc_media_get_state(media.Handle);
        }

        public static VlcMedia GetMediaFromPlayer(VlcMediaPlayer player)
        {

            IntPtr media = VlcLib.libvlc_media_player_get_media(player.Handle);
            if (media == IntPtr.Zero) return null;
            return new VlcMedia(media);
        }

        #endregion

        #region audio

        public static int GetAudioVolume(VlcMediaPlayer player)
        {
            return VlcLib.libvlc_audio_get_volume(player.Handle);
        }

        public static void SetAudioVolume(VlcMediaPlayer player, int volume)
        {
            VlcLib.libvlc_audio_set_volume(player.Handle, volume);
        }

        public static void MutePlayer(VlcMediaPlayer player)
        {
            VlcLib.libvlc_audio_toggle_mute(player.Handle);
        }

        #endregion

    }
}
