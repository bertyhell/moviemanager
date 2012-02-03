using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using Model;

namespace MovieManager.APP.Panels.Filter
{
    /// <summary>
    /// Interaction logic for FilterText.xaml
    /// </summary>
    public partial class FilterMultiOption
    {
        private readonly string _property;

        public enum TextOperations
        {
            Is, IsAll, IsNot
        }

        public FilterMultiOption(string property, IEnumerable<string> options)
        {
            InitializeComponent();

            _property = property;
            cbbOptions.SetItems(options);
            cbbOptions.OptionsCombobox.DropDownClosed += CbbOptionsDropDownClosed;
        }



        private static void CbbOptionsDropDownClosed(object sender, EventArgs e)
        {
            //update filter
            MainController.Instance.VideosView.Refresh();
        }
        

        public List<String> MultiOptionOperationsLabels { 
            get { return new List<String> {"Is", "Is all", "Is not"}; }
        }

        public Filters FilterType { get; set; }


        public override bool FilterSucceeded(Video video)
        {
            switch ((TextOperations)cbbOperation.SelectedIndex)
            {
                case TextOperations.Is:
                    //return ((String)typeof(Video).GetProperty(_property).GetValue(video, null)).Contains(FilterInput);
                    List<String> videoOptions = ((List<String>) typeof (Video).GetProperty(_property).GetValue(video, null));
                    foreach (string selectedOption in cbbOptions.SelectedItems)
                    {
                        if(videoOptions.Contains(selectedOption))
                        {
                            return true;
                        }
                    }
                    return false;
                case TextOperations.IsAll:
                    videoOptions = ((List<String>)typeof(Video).GetProperty(_property).GetValue(video, null));
                    foreach (string selectedOption in cbbOptions.SelectedItems)
                    {
                        if (!videoOptions.Contains(selectedOption))
                        {
                            return false;
                        }
                    }
                    return true;
                case TextOperations.IsNot:
                    videoOptions = ((List<String>)typeof(Video).GetProperty(_property).GetValue(video, null));
                    foreach (string selectedOption in cbbOptions.SelectedItems)
                    {
                        if (videoOptions.Contains(selectedOption))
                        {
                            return false;
                        }
                    }
                    return true;
            }
            return false;
        }
    }
}
