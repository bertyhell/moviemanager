using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Model.Interfaces;

namespace Tmc.WinUI.Application.Panels.Common
{
    /// <summary>
    /// Interaction logic for ThumbnailDescriptionControl.xaml
    /// </summary>
    public partial class ThumbnailDescriptionControl : UserControl
    {
        private readonly ThumbnailDescriptionControlController _controller = new ThumbnailDescriptionControlController();
        public ThumbnailDescriptionControl()
        {
            InitializeComponent();
            _contentGrid.DataContext = _controller;
        }
        public static readonly DependencyProperty THUMBNAIL_DESCRIPTION_PROPERTY =
            DependencyProperty.Register("PreviewDescription", typeof(IPreviewInfoRetriever), typeof(ThumbnailDescriptionControl), new PropertyMetadata(ThumbnailDescriptionPropertyChanged));

        public static void ThumbnailDescriptionPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((ThumbnailDescriptionControl)sender)._controller.Item = (IPreviewInfoRetriever)e.NewValue;
        }

        public IPreviewInfoRetriever PreviewDescription
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
