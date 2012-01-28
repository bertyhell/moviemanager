using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VlcPlayer
{
    public partial class Overlay : Form
    {
        private VlcWinForm _parent;

        public Overlay(VlcWinForm parent)
        {
            InitializeComponent();
            _parent = parent;
        }

        private void Overlay_KeyUp(object sender, KeyEventArgs e)
        {
            _parent.HandleKeys(e.KeyCode);
        }

        private void Overlay_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            _parent.ToggleFullScreen();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }
    }
}
