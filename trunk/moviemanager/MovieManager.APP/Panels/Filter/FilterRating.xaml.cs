using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using Model;

namespace MovieManager.APP.Panels.Filter
{
    /// <summary>
    /// Interaction logic for FilterRating.xaml
    /// </summary>
    public partial class FilterRating : INotifyPropertyChanged
    {
        private readonly string _property;

        public enum TextOperations
        {
            Before, After, InBetween, NotBetween
        }

        public FilterRating(string property, string label)
        {
            InitializeComponent();

            txtLabel.Text = label + ":";
            _property = property;
        }

        public List<String> TextOperationsLabels
        {
            get { return new List<String> { "Below", "Above", "In between", "Not between" }; }
        }

        public Filters FilterType { get; set; }

        private double _filterInputStart = 7;
        public double FilterInputStart
        {
            get { return _filterInputStart; }
            set
            {
                _filterInputStart = value;
                MainController.Instance.VideosView.Refresh();
            }
        }

        public Visibility DisplaySecondRating { get; set; }

        private double _filterInputEnd = 10;
        public double FilterInputEnd
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
            double Rating = ((double) typeof (Video).GetProperty(_property).GetValue(video, null));
            switch ((TextOperations)cbbOperation.SelectedIndex)
            {
                
                case TextOperations.Before:
                    return Rating < FilterInputStart;
                case TextOperations.After:
                    return Rating > FilterInputStart;
                case TextOperations.InBetween:
                    return (Rating > FilterInputStart) && (Rating > FilterInputEnd);
                case TextOperations.NotBetween:
                    return !(Rating > FilterInputStart) && (Rating > FilterInputEnd);
            }
            return false;
        }

        private void CbbOperationSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            DisplaySecondRating = cbbOperation.SelectedIndex < 2 ? Visibility.Collapsed : Visibility.Visible;
            PropChanged("DisplaySecondRating");
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
