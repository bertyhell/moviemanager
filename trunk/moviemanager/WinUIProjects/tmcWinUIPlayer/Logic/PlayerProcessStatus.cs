using System.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Tmc.SystemFrameworks.Common;
using Tmc.SystemFrameworks.Log;
using Tmc.SystemFrameworks.Model;

namespace Tmc.WinUI.Player.Logic
{
    public static class PlayerProcesses
    {
        private static readonly object STATUS_LOCK = new object();
        private static readonly Dictionary<Process, Video> PLAYERS_STATUSES;

        static PlayerProcesses()
        {
            PLAYERS_STATUSES = new Dictionary<Process, Video>();
        }

        public static void StartVideo(Video video, string mediaPlayer)
        {
            if (video.Path != null)
            {
                if (mediaPlayer == "INTERNAL")
                {
                    MMPlayer Vlc = new MMPlayer();
                    Vlc.Show();
                    Vlc.PlayVideo(video);
                }
                else if (mediaPlayer == "VLC")
                {
                    try
                    {
                        Process Process = CommandHelper.ExecuteCommandSync(Path.Combine(RegistryHelper.GetInstallationPath("VLC"), "vlc"), "\"" + video.Path + "\"");
                        PLAYERS_STATUSES.Add(Process, video);
                        Process.Exited += ProcessExited;
                        Process.Disposed += ProcessExited;
                    }
                    catch (Exception Ex)
                    {
                        GlobalLogger.Instance.VlcPlayerLogger.Fatal(GlobalLogger.FormatExceptionForLog("PlayerProcesses", "StartVideo", Ex));
                    }
                }
            }
        }

        static void ProcessExited(object sender, EventArgs e)
        {
            lock (STATUS_LOCK)
            {
                Process Process = (Process) sender;
                if(PLAYERS_STATUSES.ContainsKey(Process))
                {
                    Video Video = PLAYERS_STATUSES[Process];
                    if(Video != null)
                        Video.CheckVideoSeen(100,100,false);// TODO 010: check timestamp with vlc
                }
            }
        }


    }
}
