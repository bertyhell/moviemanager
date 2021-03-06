﻿using System;
using System.Drawing;
using System.Windows.Forms;
using Tmc.SystemFrameworks.Common;

namespace Tmc.WinUI.Player.Common
{
    public partial class MediaPlayerControl : UserControl
    {
        private MMPlayer _form;
        private Point _previousLocation;
        private int _previousWidth;
        private bool _videoEndReached;
        private bool _attachedToEvents;

        public MediaPlayerControl()
        {
            InitializeComponent();
        }

        public MediaPlayerControl(MMPlayer form)
        {
            InitializeComponent();
            Initialize(form);
        }

        public void Initialize(MMPlayer form)
        {
            _form = form;
        }

        #region properties

        public MMPlayer MMPlayer
        {
            get { return _form; }
        }

        #endregion

        #region button event handlers

        private void BtnPauseClick(object sender, EventArgs e)
        {
            _form.Player.Pause();
        }

        private void BtnPlayClick(object sender, EventArgs e)
        {
            //_form.PlayVideo();
        }

        private void BtnStopClick(object sender, EventArgs e)
        {
            _form.Player.Stop();
        }

        private void BtnMuteClick(object sender, EventArgs e)
        {
            _form.Player.Mute();
        }

        private void BtnFullScreenClick(object sender, EventArgs e)
        {
            _form.ToggleFullScreen();
        }

        #endregion

        public bool VideoEndReached
        {
            get { return _videoEndReached; }
            set { _videoEndReached = value; }
        }

        public void ResetForNewVideo()
        {
            _videoEndReached = false;
        }

        public void SetVideoTimestamp()
        {
// ReSharper disable LocalizableElement
            _lblTimestamp.Text = TimestampUtilities.LongToTimestampString(_form.Player.CurrentTimestamp) + "/" +
// ReSharper restore LocalizableElement
                                 TimestampUtilities.LongToTimestampString(_form.Player.VideoLength);
        }

        /*
        public void SetTimestampTrackBarPosition()
        {
            //change trackbar
            _trbTimestamp.SuspendChangedEvent = true;
            _trbTimestamp.Value = (int) (_form.Player.CurrentTimestamp*1.0*_trbTimestamp.Maximum/_form.Player.VideoLength);
            _trbTimestamp.SuspendChangedEvent = false;
        }*/


        public void RefreshVolumeTrackbarPostion()
        {
            _trbVolume.SuspendChangedEvent = true;
            _trbVolume.Value = _form.Player.Volume;
            _trbVolume.SuspendChangedEvent = false;
        }

        #region event manager methods
        /*
        public void AttachToEvents()
        {
            if (!_attachedToEvents)
            {
                _form.Player.EventManager.TimeChanged += EventManagerTimeChanged;
                _form.Player.EventManager.MediaEnded += new EventHandler(EventManager_MediaEnded);
                _attachedToEvents = true;
            }
        }

        public void DetachFromEvents()
        {
            if (_attachedToEvents)
            {
                _form.Player.EventManager.MediaEnded -= EventManager_MediaEnded;
                _form.Player.EventManager.TimeChanged -= EventManagerTimeChanged;
                _attachedToEvents = false;
            }
        }
        
        void EventManagerTimeChanged(object sender, Events.MediaPlayerTimeChanged e)
        {
            try
            {
                _lblTimestamp.Invoke(Delegate.CreateDelegate(typeof (SetTimeStamp), this, "SetVideoTimestamp"));
                _trbTimestamp.Invoke(Delegate.CreateDelegate(typeof (SetTimeStamp), this, "SetTimestampTrackBarPosition"));
            }
            catch
            {
            }

        }

        void EventManager_MediaEnded(object sender, EventArgs e)
        {
            _videoEndReached = true;
        }

        private delegate void SetTimeStamp();
        */
        #endregion


        #region volume

        private void CustomTrackbar1ValueChanged(object sender, EventArgs e)
        {
            _form.Player.Volume = _trbVolume.Value;
        }

        #endregion

        private void TrbTimestampValueChanged(object sender, EventArgs e)
        {
            _form.Player.CurrentTimestamp = (long)(_trbTimestamp.Value * 1.0 / _trbTimestamp.Maximum * _form.Player.VideoLength);
        }//
    }
}
