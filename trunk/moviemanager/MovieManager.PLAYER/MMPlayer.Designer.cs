namespace MovieManager.PLAYER
{
    partial class MMPlayer
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
            this._pnlVideo = new System.Windows.Forms.Panel();
            this._menubar = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._mediaPlayerControl = new MovieManager.PLAYER.Common.MediaPlayerControl();
            this._menubar.SuspendLayout();
            this.SuspendLayout();
            // 
            // _pnlVideo
            // 
            this._pnlVideo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._pnlVideo.BackColor = System.Drawing.Color.Black;
            this._pnlVideo.Location = new System.Drawing.Point(0, 23);
            this._pnlVideo.Name = "_pnlVideo";
            this._pnlVideo.Size = new System.Drawing.Size(543, 378);
            this._pnlVideo.TabIndex = 0;
            this._pnlVideo.Resize += new System.EventHandler(this.VideoPanelResize);
            // 
            // _menubar
            // 
            this._menubar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this._menubar.Location = new System.Drawing.Point(0, 0);
            this._menubar.Name = "_menubar";
            this._menubar.Size = new System.Drawing.Size(543, 24);
            this._menubar.TabIndex = 1;
            this._menubar.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.openToolStripMenuItem.Text = "Open";
            // 
            // _mediaPlayerControl
            // 
            this._mediaPlayerControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._mediaPlayerControl.Location = new System.Drawing.Point(0, 401);
            this._mediaPlayerControl.Name = "_mediaPlayerControl";
            this._mediaPlayerControl.Size = new System.Drawing.Size(543, 76);
            this._mediaPlayerControl.TabIndex = 0;
            this._mediaPlayerControl.VideoEndReached = false;
            // 
            // MMPlayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(543, 477);
            this.Controls.Add(this._mediaPlayerControl);
            this.Controls.Add(this._pnlVideo);
            this.Controls.Add(this._menubar);
            this.MainMenuStrip = this._menubar;
            this.Name = "MMPlayer";
            this.Text = "MMPlayer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MMPlayer_FormClosing);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FormKeyUp);
            this.Move += new System.EventHandler(this.FormMove);
            this._menubar.ResumeLayout(false);
            this._menubar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel _pnlVideo;
        private System.Windows.Forms.MenuStrip _menubar;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private Common.MediaPlayerControl _mediaPlayerControl;
    }
}