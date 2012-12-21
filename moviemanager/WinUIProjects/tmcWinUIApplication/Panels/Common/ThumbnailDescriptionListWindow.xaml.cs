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
    /// Interaction logic for ThumbnailDescriptionListWindow.xaml
    /// </summary>
    public partial class ThumbnailDescriptionListWindow : Window
    {
        private ThumbnailDescriptionListWindowController _controller = new ThumbnailDescriptionListWindowController();

        public ThumbnailDescriptionListWindow()
        {
            InitializeComponent();
            this.DataContext = _controller;
        }

        public List<IPreviewInfoRetriever> ThumbnailDescriptionItems
        {
            get
            {
                return _controller.ThumbnailDescriptionItems;
            }
            set
            {
                _controller.ThumbnailDescriptionItems = value;
            }
        }

        public IPreviewInfoRetriever SelectedPreviewDescription
        {
            get { return _controller.SelectedDescription; }
        }


        private void BtnOkOnClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }

    class ThumbnailDescriptionListWindowController : INotifyPropertyChanged
    {
        public ThumbnailDescriptionListWindowController()
        {
                
        }

        private List<IPreviewInfoRetriever> _itemsSource;
        public List<IPreviewInfoRetriever> ThumbnailDescriptionItems
        {
            get
            {
                return _itemsSource;
            }
            set
            {
                _itemsSource = value;
                OnPropChanged("ThumbnailDescriptionItems");
            }
        }

        private IPreviewInfoRetriever _selectedDescription;
        public IPreviewInfoRetriever SelectedDescription
        {
            get { return _selectedDescription; }
            set
            {
                _selectedDescription = value;
                OnPropChanged("SelectedDescription");
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
