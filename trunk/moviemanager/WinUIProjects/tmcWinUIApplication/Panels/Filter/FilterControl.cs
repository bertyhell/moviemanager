using System.Windows.Controls;
using Tmc.SystemFrameworks.Model;

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
