using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MovieManager.APP.Menubar;

namespace MovieManager.APP.ViewModel
{
    public class SplitMenuItemData : MenuItemData
    {
        public SplitMenuItemData()
            : this(false)
        {
        }

        public SplitMenuItemData(bool isApplicationMenu)
            : base(isApplicationMenu)
        {
        }
    }
}
