using System;
using Model;

namespace MovieManager.APP.Panels.Filter
{
    /// <summary>
    /// Interaction logic for FilterText.xaml
    /// </summary>
    public partial class FilterText : FilterControl
    {
        public FilterText()
        {
            InitializeComponent();
        }

        public Filters FilterType { get; set; }

        private string _filterInput = "";
        public String FilterInput
        {
            get { return _filterInput; }
            set { _filterInput = value; 
            MainController.Instance.VideosView.Refresh();}
        }

        public override bool FilterSucceeded(Video video)
        {
            return video.Name.Contains(FilterInput);
        }
    }
}
