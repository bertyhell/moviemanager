using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VlcPlayer
{
    public interface IVlcEventReceiver
    {
        void OnTimeChanged();

        void OnPausedChanged();
    }
}
