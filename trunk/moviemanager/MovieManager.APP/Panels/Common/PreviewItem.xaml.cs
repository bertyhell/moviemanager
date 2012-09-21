using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Model.Interfaces;

namespace MovieManager.APP.Panels.Common
{
    /// <summary>
    /// Interaction logic for PreviewItem.xaml
    /// </summary>
    public partial class PreviewItem : UserControl, INotifyPropertyChanged
    {
        public PreviewItem()
        {
            InitializeComponent();
            _contentGrid.DataContext = this;
        }

        public static readonly DependencyProperty ITEM_PROPERTY =
            DependencyProperty.Register("Item", typeof(IPreviewInfoRetriever), typeof(PreviewItem), new PropertyMetadata(ItemPropertyChanged));

        public static void ItemPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((PreviewItem)sender).Item = (IPreviewInfoRetriever)e.NewValue;
        }

        private IPreviewInfoRetriever _item;
        public IPreviewInfoRetriever Item
        {
            get
            {
                return _item;
            }
            set
            {
                _item = value;
                OnPropChanged("Item");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
