using VlcPlayer.Common;
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
            this._pnlControls = new VlcPlayer.Common.MediaPlayerControl();
            this._menubar.SuspendLayout();
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
            this._pnlVideo.Size = new System.Drawing.Size(492, 426);
            this._pnlVideo.TabIndex = 1;
            this._pnlVideo.Resize += new System.EventHandler(this.PnlVideoResize);
            // 
            // _pnlControls
            // 
            this._pnlControls.Location = new System.Drawing.Point(0, 457);
            this._pnlControls.Name = "_pnlControls";
            this._pnlControls.Player = null;
            this._pnlControls.Size = new System.Drawing.Size(492, 75);
            this._pnlControls.TabIndex = 2;
            this._pnlControls.VlcWinForm = null;
            // 
            // VlcWinForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 533);
            this.Controls.Add(this._pnlControls);
            this.Controls.Add(this._pnlVideo);
            this.Controls.Add(this._menubar);
            this.MainMenuStrip = this._menubar;
            this.Name = "VlcWinForm";
            this.Text = "VlcWinForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VlcWinFormFormClosing);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.VlcWinFormKeyUp);
            this.Move += new System.EventHandler(this.VlcWinFormMove);
            this._menubar.ResumeLayout(false);
            this._menubar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip _menubar;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Panel _pnlVideo;
        private MediaPlayerControl _pnlControls;
    }
}