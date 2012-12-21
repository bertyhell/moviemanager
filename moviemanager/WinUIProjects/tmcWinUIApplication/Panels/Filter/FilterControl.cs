using System.Windows.Controls;
using Model;

namespace MovieManager.APP.Panels.Filter
{
    public class FilterControl : UserControl
    {
        public virtual bool FilterSucceeded(Video video)
        {
            return true;
        }
    }
}
