using System.Drawing;
using System.Timers;
using System.Windows.Forms;
using MovieManager.PLAYER.Common;
using Timer = System.Windows.Forms.Timer;

namespace MovieManager.PLAYER
{
    public partial class Overlay : Form
    {
        private readonly MMPlayer _parent;
        private MediaPlayerControl _controlBar;
        private Timer aTimer;

        public Overlay(MMPlayer parent)
        {
            InitializeComponent();
            _parent = parent; 
            aTimer = new Timer();
            aTimer.Tick += new System.EventHandler(aTimer_Tick);
            // Set the Interval to 5 seconds.
            aTimer.Interval = 3000;

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

        private void Overlay_SizeChanged(object sender, System.EventArgs e)
        {
            _controlBar.Width = this.Width;
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
            this.SizeChanged += new System.EventHandler(Overlay_SizeChanged);
        }

        public void RemoveControlBar()
        {
            if (_controlBar != null)
            {
                this.SizeChanged -= new System.EventHandler(Overlay_SizeChanged);
                _pnlControlBarHolder.Controls.Remove(_controlBar);
                _controlBar = null;
                _pnlControlBarHolder.Visible = false;
            }
        }

        public void Redraw()
        {

            Redraw(this.Size);
        }

        public void Redraw(Size newSize)
        {
            if (!IsDisposed)
            {
                this.Visible = false;
                this.Visible = true;
                this.Size = new Size(0, 0);
                this.Size = newSize;
            }
        }

        public void SetMessage(string message)
        {
            ClearMessage();
            _txtMessage.Text = message;
            Redraw();
            aTimer.Enabled = true;
        }

        private void ClearMessage()
        {
            _txtMessage.Text = "";
            Redraw();
        }


        void aTimer_Tick(object sender, System.EventArgs e)
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
