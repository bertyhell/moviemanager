namespace Tmc.WinUI.Player.Common
{
    partial class MediaPlayerControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._lblTimestamp = new System.Windows.Forms.Label();
            this._btnMute = new System.Windows.Forms.Button();
            this._btnPlay = new System.Windows.Forms.Button();
            this._btnPause = new System.Windows.Forms.Button();
            this._btnStop = new System.Windows.Forms.Button();
            this._trbVolume = new CustomTrackbar();
            this._trbTimestamp = new CustomTrackbar();
            this._btnFullScreen = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this._trbVolume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._trbTimestamp)).BeginInit();
            this.SuspendLayout();
            // 
            // _lblTimestamp
            // 
            this._lblTimestamp.AutoSize = true;
            this._lblTimestamp.Location = new System.Drawing.Point(212, 44);
            this._lblTimestamp.Name = "_lblTimestamp";
            this._lblTimestamp.Size = new System.Drawing.Size(35, 13);
            this._lblTimestamp.TabIndex = 22;
            this._lblTimestamp.Text = "label1";
            // 
            // _btnMute
            // 
            this._btnMute.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this._btnMute.Image = global::Tmc.WinUI.Player.Properties.Resources.volume_muted_32;
            this._btnMute.Location = new System.Drawing.Point(374, 29);
            this._btnMute.Name = "_btnMute";
            this._btnMute.Size = new System.Drawing.Size(41, 38);
            this._btnMute.TabIndex = 20;
            this._btnMute.TabStop = false;
            this._btnMute.UseVisualStyleBackColor = true;
            this._btnMute.Click += new System.EventHandler(this.BtnMuteClick);
            // 
            // _btnPlay
            // 
            this._btnPlay.Image = global::Tmc.WinUI.Player.Properties.Resources.play_32;
            this._btnPlay.Location = new System.Drawing.Point(89, 31);
            this._btnPlay.Name = "_btnPlay";
            this._btnPlay.Size = new System.Drawing.Size(38, 36);
            this._btnPlay.TabIndex = 17;
            this._btnPlay.TabStop = false;
            this._btnPlay.UseVisualStyleBackColor = true;
            this._btnPlay.Click += new System.EventHandler(this.BtnPlayClick);
            // 
            // _btnPause
            // 
            this._btnPause.Image = global::Tmc.WinUI.Player.Properties.Resources.pause_32;
            this._btnPause.Location = new System.Drawing.Point(48, 31);
            this._btnPause.Name = "_btnPause";
            this._btnPause.Size = new System.Drawing.Size(37, 36);
            this._btnPause.TabIndex = 16;
            this._btnPause.TabStop = false;
            this._btnPause.UseVisualStyleBackColor = true;
            this._btnPause.Click += new System.EventHandler(this.BtnPauseClick);
            // 
            // _btnStop
            // 
            this._btnStop.Image = global::Tmc.WinUI.Player.Properties.Resources.stop_32;
            this._btnStop.Location = new System.Drawing.Point(4, 31);
            this._btnStop.Name = "_btnStop";
            this._btnStop.Size = new System.Drawing.Size(39, 36);
            this._btnStop.TabIndex = 18;
            this._btnStop.TabStop = false;
            this._btnStop.UseVisualStyleBackColor = true;
            this._btnStop.Click += new System.EventHandler(this.BtnStopClick);
            // 
            // _trbVolume
            // 
            this._trbVolume.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this._trbVolume.Location = new System.Drawing.Point(427, 2);
            this._trbVolume.Maximum = 200;
            this._trbVolume.Name = "_trbVolume";
            this._trbVolume.Orientation = System.Windows.Forms.Orientation.Vertical;
            this._trbVolume.Size = new System.Drawing.Size(45, 72);
            this._trbVolume.SuspendChangedEvent = false;
            this._trbVolume.TabIndex = 23;
            this._trbVolume.TickFrequency = 50;
            this._trbVolume.Value = 100;
            this._trbVolume.ValueChanged += new System.EventHandler(this.CustomTrackbar1ValueChanged);
            // 
            // _trbTimestamp
            // 
            this._trbTimestamp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._trbTimestamp.BackColor = System.Drawing.SystemColors.Control;
            this._trbTimestamp.Location = new System.Drawing.Point(-1, 7);
            this._trbTimestamp.Maximum = 1200;
            this._trbTimestamp.Name = "_trbTimestamp";
            this._trbTimestamp.Size = new System.Drawing.Size(417, 45);
            this._trbTimestamp.SuspendChangedEvent = false;
            this._trbTimestamp.TabIndex = 19;
            this._trbTimestamp.TabStop = false;
            this._trbTimestamp.TickStyle = System.Windows.Forms.TickStyle.None;
            this._trbTimestamp.ValueChanged += new System.EventHandler(this.TrbTimestampValueChanged);
            // 
            // _btnFullScreen
            // 
            this._btnFullScreen.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this._btnFullScreen.AutoSize = true;
            this._btnFullScreen.Image = global::Tmc.WinUI.Player.Properties.Resources.fullscreen_32;
            this._btnFullScreen.Location = new System.Drawing.Point(321, 29);
            this._btnFullScreen.Name = "_btnFullScreen";
            this._btnFullScreen.Size = new System.Drawing.Size(40, 38);
            this._btnFullScreen.TabIndex = 21;
            this._btnFullScreen.TabStop = false;
            this._btnFullScreen.UseVisualStyleBackColor = true;
            this._btnFullScreen.Click += new System.EventHandler(this.BtnFullScreenClick);
            // 
            // MediaPlayerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._trbVolume);
            this.Controls.Add(this._lblTimestamp);
            this.Controls.Add(this._btnFullScreen);
            this.Controls.Add(this._btnMute);
            this.Controls.Add(this._btnPlay);
            this.Controls.Add(this._btnPause);
            this.Controls.Add(this._btnStop);
            this.Controls.Add(this._trbTimestamp);
            this.Name = "MediaPlayerControl";
            this.Size = new System.Drawing.Size(470, 76);
            ((System.ComponentModel.ISupportInitialize)(this._trbVolume)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._trbTimestamp)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CustomTrackbar _trbVolume;
        private System.Windows.Forms.Label _lblTimestamp;
        private System.Windows.Forms.Button _btnMute;
        private System.Windows.Forms.Button _btnPlay;
        private System.Windows.Forms.Button _btnPause;
        private System.Windows.Forms.Button _btnStop;
        private CustomTrackbar _trbTimestamp;
        private System.Windows.Forms.Button _btnFullScreen;


    }
}
