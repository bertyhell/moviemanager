﻿using System.IO;
using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using MovieManager.Common;
using MovieManager.LOG;

namespace MovieManager.PLAYER.Logic
{
    public static class PlayerProcesses
    {
        private static object _statusLock = new object();
        private static Dictionary<Process, Video> _playersStatuses;

        static PlayerProcesses()
        {
            _playersStatuses = new Dictionary<Process, Video>();
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

                    Process Process;
                    try
                    {
                        Process = CommandHelper.ExecuteCommandSync(Path.Combine(RegistryHelper.GetInstallationPath("VLC"), "vlc"), "\"" + video.Path + "\"");
                        _playersStatuses.Add(Process, video);
                        Process.Exited += Process_Exited;
                        Process.Disposed += Process_Exited;
                    }
                    catch (Exception Ex)
                    {
                        GlobalLogger.Instance.VlcPlayerLogger.Fatal(GlobalLogger.FormatExceptionForLog("PlayerProcesses", "StartVideo", Ex));
                    }
                }
            }
        }

        static void Process_Exited(object sender, EventArgs e)
        {
            lock (_statusLock)
            {
                Process Process = (Process) sender;
                if(_playersStatuses.ContainsKey(Process))
                {
                    Video Video = _playersStatuses[Process];
                    if(Video != null)
                        Video.CheckVideoSeen(100,100,false);// TODO 010: check timestamp with vlc
                }
            }
        }


    }
}
