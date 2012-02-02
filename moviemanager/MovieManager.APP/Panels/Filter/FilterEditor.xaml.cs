using Model;

namespace MovieManager.APP.Panels.Filter
{
    /// <summary>
    /// Interaction logic for FilterEditor.xaml
    /// </summary>
    public partial class FilterEditor
    {

        private readonly FilterController _controller;
        public FilterEditor()
        {
            InitializeComponent();
            _controller = new FilterController();
            DataContext = _controller;
        }

        public bool FilterVideo(object video)
        {

            foreach (FilterControl filterControl in _controller.AppliedFilters)
            {
                if (!filterControl.FilterSucceeded((Video)video)) return false;
            }
            return true;
        }
    }
}
