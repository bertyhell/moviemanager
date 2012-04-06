using System;
using System.Windows.Forms;

namespace VlcPlayer.Common
{
    class CustomTrackbar : TrackBar
    {
        public bool SuspendChangedEvent
        { get; set; }

        protected override void OnValueChanged(EventArgs e)
        {
            if (!SuspendChangedEvent) base.OnValueChanged(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            int MouseX = e.X - 10 - Location.X < 0 ? 0 : e.X - 10 - Location.X > Maximum ? Maximum : e.X - 10 - Location.X; //check bounderies of trackbar
            Value = MouseX * (Maximum - Minimum) / (Width-20);
        }
    }
}
