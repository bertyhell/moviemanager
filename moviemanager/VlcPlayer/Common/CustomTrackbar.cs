using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
