namespace VlcPlayer
{
    partial class Overlay
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
            this._pnlBackground = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // _pnlBackground
            // 
            this._pnlBackground.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._pnlBackground.Location = new System.Drawing.Point(-1, -1);
            this._pnlBackground.Name = "_pnlBackground";
            this._pnlBackground.Size = new System.Drawing.Size(815, 149);
            this._pnlBackground.TabIndex = 0;
            // 
            // Overlay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(812, 146);
            this.Controls.Add(this._pnlBackground);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Overlay";
            this.Opacity = 0.5D;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FullScreenForm";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OverlayKeyUp);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.OverlayMouseDoubleClick);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OverlayMouseMove);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel _pnlBackground;




    }
}