using System.Windows.Controls;
using Model;

namespace Tmc.WinUI.Application.Panels.Filter
{
    public class FilterControl : UserControl
    {
        public virtual bool FilterSucceeded(Video video)
        {
            return true;
        }
    }
}
