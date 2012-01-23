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
        private readonly string _property;

        public enum TextOperations
        {
            Contains, DoesntContain, StartsWith, EndsWith, Regex
        }

        public FilterText(string property)
        {
            _property = property;
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
                    return ((String)typeof(Video).GetProperty(_property).GetValue(video, null)).Contains(FilterInput);
                case TextOperations.DoesntContain:
                    return !((String)typeof(Video).GetProperty(_property).GetValue(video, null)).Contains(FilterInput);
                case TextOperations.StartsWith:
                    return !((String)typeof(Video).GetProperty(_property).GetValue(video, null)).StartsWith(FilterInput);
                case TextOperations.EndsWith:
                    return !((String)typeof(Video).GetProperty(_property).GetValue(video, null)).EndsWith(FilterInput);
                case TextOperations.Regex:
                    return Regex.IsMatch(((String)typeof(Video).GetProperty(_property).GetValue(video, null)), FilterInput);
            }
            return false;

        }
    }
}
