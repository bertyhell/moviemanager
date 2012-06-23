using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Model;

namespace MovieManager.APP.Panels.Filter
{
    /// <summary>
    /// Interaction logic for FilterText.xaml
    /// </summary>
    public partial class FilterText
    {
        private readonly string _property;

        public enum TextOperations
        {
            Contains, DoesntContain, StartsWith, EndsWith, Regex
        }

        public FilterText(string property, string label)
        {
            InitializeComponent();
            txtLabel.Text = label + ":";
            _property = property;
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
            try
            {
                String Text = (String) typeof (Video).GetProperty(_property).GetValue(video, null);
                switch ((TextOperations) cbbOperation.SelectedIndex)
                {
                    case TextOperations.Contains:
                        return Text.Contains(FilterInput);
                    case TextOperations.DoesntContain:
                        return !Text.Contains(FilterInput);
                    case TextOperations.StartsWith:
                        return !Text.StartsWith(FilterInput);
                    case TextOperations.EndsWith:
                        return !Text.EndsWith(FilterInput);
                    case TextOperations.Regex:
                        return Regex.IsMatch(Text, FilterInput);
                }
            }catch(ArgumentException)
            {
            }
            return false;
        }

        private void CbbOperationSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            MainController.Instance.VideosView.Refresh();
        }
    }
}
