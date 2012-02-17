namespace VlcPlayer
{
    partial class VlcWinForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._menubar = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._pnlVideo = new System.Windows.Forms.Panel();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this._btnStop = new System.Windows.Forms.Button();
            this._btnPause = new System.Windows.Forms.Button();
            this._btnPlay = new System.Windows.Forms.Button();
            this._pnlControls = new System.Windows.Forms.Panel();
            this._btnFullScreen = new System.Windows.Forms.Button();
            this._btnMute = new System.Windows.Forms.Button();
            this._lblTimestamp = new System.Windows.Forms.Label();
            this._menubar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this._pnlControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // _menubar
            // 
            this._menubar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this._menubar.Location = new System.Drawing.Point(0, 0);
            this._menubar.Name = "_menubar";
            this._menubar.Size = new System.Drawing.Size(492, 24);
            this._menubar.TabIndex = 0;
            this._menubar.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // playToolStripMenuItem
            // 
            this.playToolStripMenuItem.Name = "playToolStripMenuItem";
            this.playToolStripMenuItem.Size = new System.Drawing.Size(96, 22);
            this.playToolStripMenuItem.Text = "Play";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(96, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // _pnlVideo
            // 
            this._pnlVideo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._pnlVideo.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this._pnlVideo.Enabled = false;
            this._pnlVideo.Location = new System.Drawing.Point(0, 25);
            this._pnlVideo.Name = "_pnlVideo";
            this._pnlVideo.Size = new System.Drawing.Size(492, 411);
            this._pnlVideo.TabIndex = 1;
            this._pnlVideo.Resize += new System.EventHandler(this.PnlVideoResize);
            // 
            // trackBar1
            // 
            this.trackBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBar1.Location = new System.Drawing.Point(19, 5);
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(460, 45);
            this.trackBar1.TabIndex = 3;
            this.trackBar1.TabStop = false;
            // 
            // _btnStop
            // 
            this._btnStop.Location = new System.Drawing.Point(24, 29);
            this._btnStop.Name = "_btnStop";
            this._btnStop.Size = new System.Drawing.Size(39, 23);
            this._btnStop.TabIndex = 2;
            this._btnStop.TabStop = false;
            this._btnStop.Text = "Stop";
            this._btnStop.UseVisualStyleBackColor = true;
            this._btnStop.Click += new System.EventHandler(this.BtnStopClick);
            // 
            // _btnPause
            // 
            this._btnPause.Location = new System.Drawing.Point(69, 29);
            this._btnPause.Name = "_btnPause";
            this._btnPause.Size = new System.Drawing.Size(45, 23);
            this._btnPause.TabIndex = 2;
            this._btnPause.TabStop = false;
            this._btnPause.Text = "Pause";
            this._btnPause.UseVisualStyleBackColor = true;
            this._btnPause.Click += new System.EventHandler(this.BtnPauseClick);
            // 
            // _btnPlay
            // 
            this._btnPlay.Location = new System.Drawing.Point(120, 29);
            this._btnPlay.Name = "_btnPlay";
            this._btnPlay.Size = new System.Drawing.Size(38, 23);
            this._btnPlay.TabIndex = 2;
            this._btnPlay.TabStop = false;
            this._btnPlay.Text = "Play";
            this._btnPlay.UseVisualStyleBackColor = true;
            this._btnPlay.Click += new System.EventHandler(this.BtnPlayClick);
            // 
            // _pnlControls
            // 
            this._pnlControls.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._pnlControls.Controls.Add(this._lblTimestamp);
            this._pnlControls.Controls.Add(this._btnFullScreen);
            this._pnlControls.Controls.Add(this._btnMute);
            this._pnlControls.Controls.Add(this._btnPlay);
            this._pnlControls.Controls.Add(this._btnPause);
            this._pnlControls.Controls.Add(this._btnStop);
            this._pnlControls.Controls.Add(this.trackBar1);
            this._pnlControls.Location = new System.Drawing.Point(0, 442);
            this._pnlControls.Name = "_pnlControls";
            this._pnlControls.Size = new System.Drawing.Size(492, 52);
            this._pnlControls.TabIndex = 4;
            // 
            // _btnFullScreen
            // 
            this._btnFullScreen.Location = new System.Drawing.Point(164, 29);
            this._btnFullScreen.Name = "_btnFullScreen";
            this._btnFullScreen.Size = new System.Drawing.Size(72, 23);
            this._btnFullScreen.TabIndex = 5;
            this._btnFullScreen.TabStop = false;
            this._btnFullScreen.Text = "Full Screen";
            this._btnFullScreen.UseVisualStyleBackColor = true;
            this._btnFullScreen.Click += new System.EventHandler(this.BtnFullScreenClick);
            // 
            // _btnMute
            // 
            this._btnMute.Location = new System.Drawing.Point(242, 29);
            this._btnMute.Name = "_btnMute";
            this._btnMute.Size = new System.Drawing.Size(75, 23);
            this._btnMute.TabIndex = 4;
            this._btnMute.TabStop = false;
            this._btnMute.Text = "Mute";
            this._btnMute.UseVisualStyleBackColor = true;
            this._btnMute.Click += new System.EventHandler(this.BtnMuteClick);
            // 
            // _lblTimestamp
            // 
            this._lblTimestamp.AutoSize = true;
            this._lblTimestamp.Location = new System.Drawing.Point(403, 35);
            this._lblTimestamp.Name = "_lblTimestamp";
            this._lblTimestamp.Size = new System.Drawing.Size(35, 13);
            this._lblTimestamp.TabIndex = 6;
            this._lblTimestamp.Text = "label1";
            // 
            // VlcWinForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 499);
            this.Controls.Add(this._pnlVideo);
            this.Controls.Add(this._menubar);
            this.Controls.Add(this._pnlControls);
            this.MainMenuStrip = this._menubar;
            this.Name = "VlcWinForm";
            this.Text = "VlcWinForm";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.VlcWinFormKeyUp);
            this.Move += new System.EventHandler(this.VlcWinFormMove);
            this._menubar.ResumeLayout(false);
            this._menubar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this._pnlControls.ResumeLayout(false);
            this._pnlControls.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip _menubar;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.Panel _pnlVideo;
        private System.Windows.Forms.ToolStripMenuItem playToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Button _btnStop;
        private System.Windows.Forms.Button _btnPause;
        private System.Windows.Forms.Button _btnPlay;
        private System.Windows.Forms.Panel _pnlControls;
        private System.Windows.Forms.Button _btnMute;
        private System.Windows.Forms.Button _btnFullScreen;
        private System.Windows.Forms.Label _lblTimestamp;
    }
}