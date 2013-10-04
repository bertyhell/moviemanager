using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using Tmc.SystemFrameworks.Common;
using Tmc.SystemFrameworks.Model;

namespace Tmc.WinUI.Application.Panels.Filter
{
    /// <summary>
    /// Interaction logic for FilterText.xaml
    /// </summary>
    public partial class FilterDate : INotifyPropertyChanged
    {
        private readonly string _property;

        public enum TextOperations
        {
            Before, After, InBetween, NotBetween
        }

        public FilterDate(string property, string label)
        {
            InitializeComponent();
            txtLabel.Text = label + ":";
            _property = property;
        }

        public List<String> TextOperationsLabels
        {
            get { return new List<String> { "Before", "After", "In between", "Not between" }; }
        }

        public Filters FilterType { get; set; }

        private DateTime _filterInputStart = DefaultValues.FILTER_START_DATE;
        public DateTime FilterInputStart
        {
            get { return _filterInputStart; }
            set
            {
                _filterInputStart = value;
                MainController.Instance.VideosView.Refresh();
            }
        }

        public Visibility DisplaySecondDate { get; set; }

        private DateTime _filterInputEnd = DefaultValues.FILTER_END_DATE;
        public DateTime FilterInputEnd
        {
            get { return _filterInputEnd; }
            set
            {
                _filterInputEnd = value;
                MainController.Instance.VideosView.Refresh();
            }
        }

        public override bool FilterSucceeded(Video video)
        {
            DateTime release = ((DateTime) typeof (Video).GetProperty(_property).GetValue(video, null));
            switch ((TextOperations)cbbOperation.SelectedIndex)
            {
                case TextOperations.Before:
                    return release < FilterInputStart;
                case TextOperations.After:
                    return release > FilterInputStart;
                case TextOperations.InBetween:
                    return (release > FilterInputStart) && (release < FilterInputEnd);
                case TextOperations.NotBetween:
                    return (release < FilterInputStart) && (release > FilterInputEnd);
            }
            return false;
        }

        private void CbbOperationSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            DisplaySecondDate = cbbOperation.SelectedIndex < 2 ? Visibility.Collapsed : Visibility.Visible;
            PropChanged("DisplaySecondDate");
            MainController.Instance.VideosView.Refresh();
        }

        public void PropChanged(String field)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(field));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
