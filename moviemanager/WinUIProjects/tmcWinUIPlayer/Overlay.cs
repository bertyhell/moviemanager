using System.Drawing;
using System.Windows.Forms;
using Tmc.WinUI.Player.Common;

namespace Tmc.WinUI.Player
{
    public partial class Overlay : Form
    {
        private readonly MMPlayer _parent;
        private MediaPlayerControl _controlBar;
        private readonly Timer _aTimer;

        public Overlay(MMPlayer parent)
        {
            InitializeComponent();
            _parent = parent; 
            _aTimer = new Timer();
            _aTimer.Tick += ATimerTick;
            // Set the Interval to 5 seconds.
            _aTimer.Interval = 3000;

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

        private void OverlaySizeChanged(object sender, System.EventArgs e)
        {
            _controlBar.Width = Width;
        }



        private void OverlayMouseMove(object sender, MouseEventArgs e)
        {
            if (_parent.IsFullScreen && _controlBar != null)
            {
                if (!_pnlControlBarHolder.Visible && e.Y > (int)System.Windows.SystemParameters.PrimaryScreenHeight - 100)
                {
                    _pnlControlBarHolder.Visible = true;

                }
                else if (_pnlControlBarHolder.Visible && e.Y < (int)System.Windows.SystemParameters.PrimaryScreenHeight - 100)
                {
                    _pnlControlBarHolder.Visible = false;
                    Redraw();
                }
            }
        }



        public void AddControlBar(MediaPlayerControl controlBar)
        {
            RemoveControlBar();
            _controlBar = controlBar;
            _pnlControlBarHolder.Controls.Add(_controlBar);
            _pnlControlBarHolder.Visible = false;
            SizeChanged += OverlaySizeChanged;
        }

        public void RemoveControlBar()
        {
            if (_controlBar != null)
            {
                SizeChanged -= OverlaySizeChanged;
                _pnlControlBarHolder.Controls.Remove(_controlBar);
                _controlBar = null;
                _pnlControlBarHolder.Visible = false;
            }
        }

        public void Redraw()
        {

            Redraw(Size);
        }

        public void Redraw(Size newSize)
        {
            if (!IsDisposed)
            {
                Visible = false;
                Visible = true;
                Size = new Size(0, 0);
                Size = newSize;
            }
        }

        public void SetMessage(string message)
        {
            ClearMessage();
            _txtMessage.Text = message;
            Redraw();
            _aTimer.Enabled = true;
        }

        private void ClearMessage()
        {
            _txtMessage.Text = "";
            Redraw();
        }


        void ATimerTick(object sender, System.EventArgs e)
        {
            ClearMessage();
        }

        protected override void OnClosed(System.EventArgs e)
        {
            _parent.Close();
            base.OnClosed(e);
        }
    }
}
