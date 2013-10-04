using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Tmc.SystemFrameworks.Model;

namespace Tmc.WinUI.Application.Panels.Filter
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
	            Object property = typeof (Video).GetProperty(_property).GetValue(video, null);
				if(property is IEnumerable<object>)
				{
					foreach (object propertyItem in ((IEnumerable<object>) property))
					{
						if (TextFilterSucceeded(propertyItem.ToString(), (TextOperations) cbbOperation.SelectedIndex)) return true;
					}
				}
				else
				{
					return TextFilterSucceeded(property.ToString(), (TextOperations)cbbOperation.SelectedIndex);
				}

			}catch(ArgumentException)
			{
				//ignore temporary invalid regualr expressions
				//TODO 001 color regex input field orange with tooltip of invalid regex
			}
            return false;
        }

        private void CbbOperationSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            MainController.Instance.VideosView.Refresh();
        }

		private bool TextFilterSucceeded(string text, TextOperations filterOperation)
		{
			switch (filterOperation)
			{
				case TextOperations.Contains:
					return text.ToLower().Contains(FilterInput.ToLower());
				case TextOperations.DoesntContain:
					return !text.ToLower().Contains(FilterInput.ToLower());
				case TextOperations.StartsWith:
					return text.ToLower().StartsWith(FilterInput.ToLower());
				case TextOperations.EndsWith:
					return text.ToLower().EndsWith(FilterInput.ToLower());
				case TextOperations.Regex:
					return Regex.IsMatch(text, FilterInput, RegexOptions.IgnoreCase);
				default:
					return false;
			}
		}
    }
}
