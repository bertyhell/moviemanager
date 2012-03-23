﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using Model;

namespace MovieManager.APP.Panels.Filter
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

        private DateTime _filterInputStart = DateTime.Today.Add(new TimeSpan(90, 0, 0, 0));
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

        private DateTime _filterInputEnd = DateTime.Today.Add(new TimeSpan(90, 0, 0, 0));
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
            DateTime Release = ((DateTime) typeof (Video).GetProperty(_property).GetValue(video, null));
            switch ((TextOperations)cbbOperation.SelectedIndex)
            {
                case TextOperations.Before:
                    return Release < FilterInputStart;
                case TextOperations.After:
                    return Release > FilterInputStart;
                case TextOperations.InBetween:
                    return (Release > FilterInputStart) && (Release > FilterInputEnd);
                case TextOperations.NotBetween:
                    return !(Release > FilterInputStart) && (Release > FilterInputEnd);
            }
            return false;
        }

        private void CbbOperationSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if(cbbOperation.SelectedIndex < 2)
            {
                //hide second date picker
                DisplaySecondDate = Visibility.Collapsed;
            }else
            {
                DisplaySecondDate = Visibility.Visible;
            }
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
