using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Model;

namespace MovieManager.APP.Panels.Filter
{
    /// <summary>
    /// Interaction logic for FilterText.xaml
    /// </summary>
    public partial class FilterText : FilterControl
    {

        public enum TextOperations
        {
            Contains, DoesntContain, StartsWith, EndsWith, Regex
        }

        public FilterText()
        {
            InitializeComponent();
        }

        public List<String> TextOperationsLabels { 
            get { return new List<String> {"Contains", "Doesn't Contain", "Starts with", "Ends with", "Regex"}; }
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
            switch ((TextOperations)cbbOperation.SelectedIndex)
            {
                case TextOperations.Contains:
                    return video.Name.Contains(FilterInput);
                case TextOperations.DoesntContain:
                    return !video.Name.Contains(FilterInput);
                case TextOperations.StartsWith:
                    return !video.Name.StartsWith(FilterInput);
                case TextOperations.EndsWith:
                    return !video.Name.EndsWith(FilterInput);
                case TextOperations.Regex:
                    return Regex.IsMatch(video.Name, FilterInput);
            }
            return false;

        }
    }
}
