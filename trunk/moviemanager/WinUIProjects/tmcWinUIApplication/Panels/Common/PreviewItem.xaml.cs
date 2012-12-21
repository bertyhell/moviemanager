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

        public static readonly DependencyProperty ItemProperty =
            DependencyProperty.Register("Item", typeof(IPreviewInfoRetriever), typeof(PreviewItem), new PropertyMetadata(ItemPropertyChanged));

        public static void ItemPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((PreviewItem)sender).Item = (IPreviewInfoRetriever)e.NewValue;
        }

        public static readonly DependencyProperty LabelVisibilityProperty =
    DependencyProperty.Register("LabelVisibility", typeof(Visibility), typeof(PreviewItem), new PropertyMetadata(LabelVisibilityPropertyChanged));

        public static void LabelVisibilityPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((PreviewItem)sender).LabelVisibility = (Visibility)e.NewValue;
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


        private Visibility _labelVisibility = Visibility.Visible;//TODO 030 get value from usersettings
        public Visibility LabelVisibility
        {
            get { return _labelVisibility; }
            set
            {
                _labelVisibility = value;
                OnPropChanged("LabelVisibility");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
