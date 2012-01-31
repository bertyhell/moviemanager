using System.Windows.Forms;

namespace VlcPlayer
{
    public partial class Overlay : Form
    {
        private readonly VlcWinForm _parent;

        public Overlay(VlcWinForm parent)
        {
            InitializeComponent();
            _parent = parent;
        }

        private void OverlayKeyUp(object sender, KeyEventArgs e)
        {
            _parent.HandleKeys(e.KeyCode);
        }

        private void OverlayMouseDoubleClick(object sender, MouseEventArgs e)
        {
            _parent.ToggleFullScreen();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }
    }
}
