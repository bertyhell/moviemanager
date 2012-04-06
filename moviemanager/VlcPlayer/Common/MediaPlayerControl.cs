﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common;

namespace VlcPlayer.Common
{
    public partial class MediaPlayerControl : UserControl
    {
        private VlcMediaPlayer _player;
        private VlcWinForm _form;
        private Point _previousLocation;
        private int _previousWidth;
        
        public MediaPlayerControl()
        {
            InitializeComponent();     
        }

        public MediaPlayerControl(VlcMediaPlayer player, VlcWinForm form)
        {
            _player = player;
            _form = form;
            InitializeComponent();
        }

        public VlcWinForm VlcWinForm
        {
            get { return _form; }
            set { _form = value; }
        }

        public void ToggleFullScreen()
        {
            if(!_form.IsFullScreen)
            {
                _previousLocation = this.Location;
                _previousWidth = this.Width;
                Location = new Point(0, (int)System.Windows.SystemParameters.PrimaryScreenHeight - 100);
                Width = (int) System.Windows.SystemParameters.PrimaryScreenWidth;
            }
            else
            {
                Location = _previousLocation;
                Width = _previousWidth;
            }
        }


        #region button event handlers

        private void BtnPauseClick(object sender, EventArgs e)
        {
            _form.Pause();
        }

        private void BtnPlayClick(object sender, EventArgs e)
        {
            _form.PlayVideo();
        }

        private void BtnStopClick(object sender, EventArgs e)
        {
           _form.Stop();
        }

        private void BtnMuteClick(object sender, EventArgs e)
        {
            _player.Mute();
        }

        private void BtnFullScreenClick(object sender, EventArgs e)
        {
            _form.ToggleFullScreen();
        }

        #endregion

        public VlcMediaPlayer Player
        {
            get { return _player; }
            set { _player = value; }
        }

        public void SetVideoTimestamp()
        {
            _lblTimestamp.Text = TimestampUtilities.longToTimestampString(_player.CurrentTimestamp) + "/" +
                                 TimestampUtilities.longToTimestampString(_player.VideoLength);
        }

        public void SetTimestampTrackBarPosition()
        {
            //change trackbar
            _trbTimestamp.SuspendChangedEvent = true;
            _trbTimestamp.Value = (int) (_player.CurrentTimestamp*1.0*_trbTimestamp.Maximum/_player.VideoLength);
            _trbTimestamp.SuspendChangedEvent = false;
        }


        #region event manager methods

        public void AttachToEvents()
        {
            _player.EventManager.TimeChanged += new EventHandler<VlcPlayer.Events.MediaPlayerTimeChanged>(EventManager_TimeChanged);
        }

        public void DetachFromEvents()
        {
            _player.EventManager.TimeChanged -= new EventHandler<VlcPlayer.Events.MediaPlayerTimeChanged>(EventManager_TimeChanged);
        }
        
        void EventManager_TimeChanged(object sender, Events.MediaPlayerTimeChanged e)
        {
            _lblTimestamp.Invoke(Delegate.CreateDelegate(typeof(SetTimeStamp), this, "SetVideoTimestamp"));
            _trbTimestamp.Invoke(Delegate.CreateDelegate(typeof(SetTimeStamp), this, "SetTimestampTrackBarPosition"));

        }

        private delegate void SetTimeStamp();

        #endregion


        #region volume

        private void customTrackbar1_ValueChanged(object sender, EventArgs e)
        {
            _player.Volume = _trbVolume.Value;
        }

        #endregion

        private void _trbTimestamp_ValueChanged(object sender, EventArgs e)
        {
            _player.CurrentTimestamp = (long)(_trbTimestamp.Value * 1.0 / _trbTimestamp.Maximum * _player.VideoLength);
        }

    }
}