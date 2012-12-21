using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MovieManager.APP.Menubar
{
    public class MenuButtonData : ControlData
    {
        public bool IsVerticallyResizable
        {
            get
            {
                return _isVerticallyResizable;
            }

            set
            {
                if (_isVerticallyResizable != value)
                {
                    _isVerticallyResizable = value;
                    OnPropertyChanged("IsVerticallyResizable");
                }
            }
        }

        public bool IsHorizontallyResizable
        {
            get
            {
                return _isHorizontallyResizable;
            }

            set
            {
                if (_isHorizontallyResizable != value)
                {
                    _isHorizontallyResizable = value;
                    OnPropertyChanged("IsHorizontallyResizable");
                }
            }
        }

    }
}
