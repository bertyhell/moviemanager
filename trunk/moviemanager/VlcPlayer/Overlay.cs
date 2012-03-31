using System.Windows.Forms;

namespace VlcPlayer
{
    public partial class Overlay : Form
    {
        private readonly VlcWinForm _parent;
        private bool _isControlPanelVisible = true;

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

        private void Overlay_MouseMove(object sender, MouseEventArgs e)
        {
            if(_parent.IsFullScreen && _parent.ControlPanel != null)
            {
                //if(!_isControlPanelVisible && e.Y > (int)System.Windows.SystemParameters.PrimaryScreenHeight - 100)
                //{
                //    this.Controls.Add(_parent.ControlPanel);
                //    _parent.ControlPanel.AttachToEvents();
                //    _isControlPanelVisible = true;
                //}
                //else if (_isControlPanelVisible && e.Y < (int)System.Windows.SystemParameters.PrimaryScreenHeight - 100)
                //{
                //    _parent.ControlPanel.DetachFromEvents();
                //    this.Controls.Remove(_parent.ControlPanel);
                //    _isControlPanelVisible = false;
                //}
            }
        }
    }
}
