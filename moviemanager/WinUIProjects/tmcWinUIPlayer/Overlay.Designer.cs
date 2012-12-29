namespace Tmc.WinUI.Player
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
            this._pnlControlBarHolder = new System.Windows.Forms.Panel();
            this._txtMessage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _pnlControlBarHolder
            // 
            this._pnlControlBarHolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._pnlControlBarHolder.BackColor = System.Drawing.SystemColors.Control;
            this._pnlControlBarHolder.Location = new System.Drawing.Point(0, 307);
            this._pnlControlBarHolder.Name = "_pnlControlBarHolder";
            this._pnlControlBarHolder.Size = new System.Drawing.Size(813, 100);
            this._pnlControlBarHolder.TabIndex = 0;
            this._pnlControlBarHolder.Visible = false;
            // 
            // _txtMessage
            // 
            this._txtMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._txtMessage.AutoSize = true;
            this._txtMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._txtMessage.ForeColor = System.Drawing.Color.Black;
            this._txtMessage.Location = new System.Drawing.Point(689, 9);
            this._txtMessage.Name = "_txtMessage";
            this._txtMessage.Size = new System.Drawing.Size(0, 24);
            this._txtMessage.TabIndex = 1;
            // 
            // Overlay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(812, 407);
            this.Controls.Add(this._txtMessage);
            this.Controls.Add(this._pnlControlBarHolder);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Overlay";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FullScreenForm";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OverlayKeyUp);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.OverlayMouseDoubleClick);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OverlayMouseMove);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel _pnlControlBarHolder;
        private System.Windows.Forms.Label _txtMessage;







    }
}