using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace MovieManager.APP.Common
{
    /// <summary>
    /// Interaction logic for CheckCombobox.xaml
    /// </summary>
    public partial class CheckCombobox : INotifyPropertyChanged
    {
        public CheckCombobox()
        {
            InitializeComponent();
        }

        public List<CheckableString> Items { get; set; }

        public List<String> SelectedItems
        {
            get
            {
                return (from item in Items where item.IsSelected select item.Title).ToList();
            }
        }

        public void SetItems(IEnumerable<string> items)
        {
            Items = new List<CheckableString>();
            foreach (string item in items)
            {
                Items.Add(new CheckableString { IsSelected = false, Title = item });
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

        public String OptionsLabel
        {
            get { return String.Join(", ", SelectedItems); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
