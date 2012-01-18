using System.Windows.Controls;
using Model;

namespace MovieManager.APP.Panels.Filter
{
    /// <summary>
    /// Interaction logic for FilterEditor.xaml
    /// </summary>
    public partial class FilterEditor : UserControl
    {

        private FilterController _controller;
        public FilterEditor()
        {
            InitializeComponent();
            _controller = new FilterController();
            DataContext = _controller;
        }

        public bool FilterVideo(object video)
        {

            foreach (FilterControl FilterControl in _controller.AppliedFilters)
            {
                if (!FilterControl.FilterSucceeded((Video)video)) return false;
            }
            return true;
        }
    }
}
