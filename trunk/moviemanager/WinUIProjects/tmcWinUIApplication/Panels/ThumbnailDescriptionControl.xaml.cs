using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Model;
using Model.Interfaces;

namespace MovieManager.APP.Panels
{
    /// <summary>
    /// Interaction logic for ThumbnailDescriptionControl.xaml
    /// </summary>
    public partial class ThumbnailDescriptionControl : UserControl
    {
        private ThumbnailDescriptionControlController _controller = new ThumbnailDescriptionControlController();
        public ThumbnailDescriptionControl()
        {
            InitializeComponent();
            _contentGrid.DataContext = _controller;
        }
        public static readonly DependencyProperty THUMBNAIL_DESCRIPTION_PROPERTY =
            DependencyProperty.Register("ThumbnailDescription", typeof(IThumbnailInfoRetriever), typeof(ThumbnailDescriptionControl), new PropertyMetadata(ThumbnailDescriptionPropertyChanged));

        public static void ThumbnailDescriptionPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((ThumbnailDescriptionControl)sender)._controller.Item = (IThumbnailInfoRetriever)e.NewValue;
        }

        public IThumbnailInfoRetriever ThumbnailDescription
        {
            get { return _controller.Item; }
            set
            {
                _controller.Item = value;
            }
        }

    }

    public class ThumbnailDescriptionControlController : INotifyPropertyChanged
    {
        public ThumbnailDescriptionControlController()
        {

        }

        private IThumbnailInfoRetriever _item;

        public IThumbnailInfoRetriever Item
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
