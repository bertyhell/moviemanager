using System;
using System.Runtime.InteropServices;

namespace VlcPlayer.Events
{
    [StructLayout(LayoutKind.Explicit)]
    public struct libvlc_event_t
    {
        [FieldOffset(0)]
        public libvlc_event_e type;

        [FieldOffset(4)]
        public IntPtr p_obj;

        [FieldOffset(8)]
        public media_meta_changed media_meta_changed;

        [FieldOffset(8)]
        public media_subitem_added media_subitem_added;

        [FieldOffset(8)]
        public media_duration_changed media_duration_changed;

        [FieldOffset(8)]
        public media_parsed_changed media_parsed_changed;

        [FieldOffset(8)]
        public media_freed media_freed;

        [FieldOffset(8)]
        public media_state_changed media_state_changed;

        [FieldOffset(8)]
        public media_player_position_changed media_player_position_changed;

        [FieldOffset(8)]
        public media_player_time_changed media_player_time_changed;

        [FieldOffset(8)]
        public media_player_title_changed media_player_title_changed;

        [FieldOffset(8)]
        public media_player_seekable_changed media_player_seekable_changed;

        [FieldOffset(8)]
        public media_player_pausable_changed media_player_pausable_changed;

        [FieldOffset(8)]
        public media_list_item_added media_list_item_added;

        [FieldOffset(8)]
        public media_list_will_add_item media_list_will_add_item;

        [FieldOffset(8)]
        public media_list_item_deleted media_list_item_deleted;

        [FieldOffset(8)]
        public media_list_will_delete_item media_list_will_delete_item;

        [FieldOffset(8)]
        public media_list_player_next_item_set media_list_player_next_item_set;


        [FieldOffset(8)]
        public media_player_length_changed media_player_length_changed;

        [FieldOffset(8)]
        public vlm_media_event vlm_media_event;

        [FieldOffset(8)]
        public media_player_media_changed media_player_media_changed;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct media_meta_changed
    {
        public libvlc_meta_t meta_type;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct media_subitem_added
    {
        public IntPtr new_child;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct media_duration_changed
    {
        public long new_duration;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct media_parsed_changed
    {
        public int new_status;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct media_freed
    {
        public IntPtr md;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct media_state_changed
    {
        public libvlc_state_t new_state;
    }

    /* media instance */
    [StructLayout(LayoutKind.Sequential)]
    public struct media_player_position_changed
    {
        public float new_position;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct media_player_time_changed
    {
        public long new_time;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct media_player_title_changed
    {
        public int new_title;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct media_player_seekable_changed
    {
        public int new_seekable;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct media_player_pausable_changed
    {
        public int new_pausable;
    }

    /* media list */
    [StructLayout(LayoutKind.Sequential)]
    public struct media_list_item_added
    {
        public IntPtr item;
        public int index;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct media_list_will_add_item
    {
        public IntPtr item;
        public int index;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct media_list_item_deleted
    {
        public IntPtr item;
        public int index;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct media_list_will_delete_item
    {
        public IntPtr item;
        public int index;
    }

    /* media list player */
    [StructLayout(LayoutKind.Sequential)]
    public struct media_list_player_next_item_set
    {
        public IntPtr item;
    }
    
    /* Length changed */
    [StructLayout(LayoutKind.Sequential)]
    public struct media_player_length_changed
    {
        public long new_length;
    }

    /* VLM media */
    [StructLayout(LayoutKind.Sequential)]
    public struct vlm_media_event
    {
        public IntPtr psz_media_name;
        public IntPtr psz_instance_name;
    }

    /* Extra MediaPlayer */
    [StructLayout(LayoutKind.Sequential)]
    public struct media_player_media_changed
    {
        public IntPtr new_media;
    }

}
