using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Tmc.WinUI.Application;

namespace MovieManager.APP.Common
{
    /// <summary>
    /// Interaction logic for CheckCombobox.xaml
    /// </summary>
    public partial class CheckCombobox : INotifyPropertyChanged
    {

        public String SelectedItemsString
        {
            get { return (SelectedItems != null? string.Join(",",SelectedItems) : ""); }
        }

        public CheckCombobox()
        {
            Items = new List<CheckableString>();
            InitializeComponent();
        }

        public List<CheckableString> Items { get; set; }

        public List<String> SelectedItems
        {
            get
            {
                return (from Item in Items where Item.IsSelected select Item.Title).ToList();
            }
        }

        public void SetItems(IEnumerable<string> items)
        {
            Items.Clear();
            foreach (string Item in items)
            {
                Items.Add(new CheckableString { IsSelected = false, Title = Item });
                PropChanged("Items");
            }
        }

        public void PropChanged(String title)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(title));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void CheckBoxChecked(object sender, System.Windows.RoutedEventArgs e)
        {
            PropChanged("SelectedItemsString");
            MainController.Instance.VideosView.Refresh();
        }
    }
}
