using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Common;
using Model;
using MovieManager.APP.Panels.Search;

namespace MovieManager.APP.Panels
{


    public delegate void ClickedOnSearch(SearchEventArgs e);

    /// <summary>
    /// Interaction logic for MovieScrollPanel.xaml
    /// </summary>
    public partial class MovieScrollPanel
    {
        public event ClickedOnSearch SearchEvent;


        private List<Image> _imagesElements;
        public readonly static DependencyProperty ImagesProperty = DependencyProperty.Register("Images", typeof(List<ImageInfo>),
                                                                          typeof(MovieScrollPanel), new PropertyMetadata(new List<ImageInfo>(), OnImagesChanged));


        private const double RepositionSpeed = 100;

        private double _currentScrollViewPosition;

        public MovieScrollPanel()
        {
            InitializeComponent();
            _imagesElements = new List<Image>();
            _ImgScrollViewer.ScrollChanged += ImgScrollViewerScrollChanged;//new ScrollChangedEventHandler(ImgScrollViewerScrollChanged);
        }

        public List<ImageInfo> Images
        {
            get { return (List<ImageInfo>)GetValue(ImagesProperty); }
            set
            {
                SetValue(ImagesProperty, value);
            }
        }


        private static void UpdateImages(MovieScrollPanel panel)
        {
            panel._imagesElements.Clear();
            panel._layoutRoot.Children.Clear();

            if (panel.Images != null && panel.Images.Count > 0)
            {
                panel._imagesElements = new List<Image>();
                for (int i = 0; i < panel.Images.Count; i++)
                {
                    //add new elements
                    Image newImage = new Image();
                    //set sources
                    newImage.Source = ImageFactory.GetImage(panel.Images[i].Uri).Source;
                    newImage.Tag = panel.Images[i];
                    newImage.Margin = new Thickness(5);
                    newImage.MouseUp += panel.NewImageMouseUp;

                    panel._layoutRoot.Children.Add(newImage);
                    panel._layoutRoot.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength() });
                    Grid.SetColumn(newImage, i);
                    panel._imagesElements.Add(newImage);
                }
            }
        }

        private void NewImageMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ImageInfo info = (sender as Image).Tag as ImageInfo;
            if (info != null && SearchEvent != null)
            {
                SearchEvent(new SearchEventArgs
                {
                    SearchOptions = new SearchOptions
                    {
                        SearchForMovies = (info.Type == typeof(Movie)),
                        SearchForActors = (info.Type == typeof(Actor)),
                        SearchOnTmdb = true,
                        SearchTerm = info.Tag,
                    }
                });
            }
        }

        private static void OnImagesChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            UpdateImages(o as MovieScrollPanel);
        }

        private void BtnLeftClick(object sender, RoutedEventArgs e)
        {
            _ImgScrollViewer.ScrollToHorizontalOffset(_currentScrollViewPosition - RepositionSpeed);
        }

        private void BtnRightClick(object sender, RoutedEventArgs e)
        {
            _ImgScrollViewer.ScrollToHorizontalOffset(_currentScrollViewPosition + RepositionSpeed);
        }

        public void ImgScrollViewerScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            _currentScrollViewPosition = e.HorizontalOffset;
        }

        private void LayoutRootMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {

            _ImgScrollViewer.ScrollToHorizontalOffset(_currentScrollViewPosition - e.Delta);
        }
    }
}
