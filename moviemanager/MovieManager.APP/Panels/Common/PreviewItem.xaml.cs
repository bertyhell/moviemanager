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
    public partial class PreviewItem : UserControl
    {
        private readonly PreviewItemController _controller = new PreviewItemController();

        public PreviewItem()
        {
            InitializeComponent();
            _contentGrid.DataContext = _controller;
        }

        public static readonly DependencyProperty ITEM_PROPERTY =
            DependencyProperty.Register("Item", typeof(IPreviewInfoRetriever), typeof(PreviewItem), new PropertyMetadata(ItemPropertyChanged));

        public static void ItemPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((PreviewItem)sender)._controller.Item = (IPreviewInfoRetriever)e.NewValue;
        }


        public IPreviewInfoRetriever Item
        {
            get { return _controller.Item; }
            set
            {
                _controller.Item = value;
            }
        }
    }

    public class PreviewItemController : INotifyPropertyChanged
    {
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

        protected void OnPropChanged(string field)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(field));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
