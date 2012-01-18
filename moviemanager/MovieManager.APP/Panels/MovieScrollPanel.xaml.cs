using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Common;
using Model;
using MovieManager.APP.Search;

namespace MovieManager.APP.CommonControls
{


    public delegate void ClickedOnSearch(SearchEventArgs e);

    /// <summary>
    /// Interaction logic for MovieScrollPanel.xaml
    /// </summary>
    public partial class MovieScrollPanel : UserControl
    {
        public event ClickedOnSearch SearchEvent;


        private List<Image> _imagesElements;
        public readonly static DependencyProperty ImagesProperty = DependencyProperty.Register("Images", typeof(List<ImageInfo>),
                                                                          typeof(MovieScrollPanel), new PropertyMetadata(new List<ImageInfo>(), OnImagesChanged));


        private const double REPOSITION_SPEED = 100;

        private double _currentScrollViewPosition = 0;

        public MovieScrollPanel()
        {
            InitializeComponent();
            _imagesElements = new List<Image>();
            _ImgScrollViewer.ScrollChanged += new ScrollChangedEventHandler(_ImgScrollViewer_ScrollChanged);
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
                    Image NewImage = new Image();
                    //set sources
                    NewImage.Source = ImageFactory.GetImage(panel.Images[i].Uri).Source;
                    NewImage.Tag = panel.Images[i];
                    NewImage.Margin = new Thickness(5);
                    NewImage.MouseUp += panel.NewImage_MouseUp;

                    panel._layoutRoot.Children.Add(NewImage);
                    panel._layoutRoot.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength() });
                    Grid.SetColumn(NewImage, i);
                    panel._imagesElements.Add(NewImage);
                }
            }
        }

        private void NewImage_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ImageInfo Info = (sender as Image).Tag as ImageInfo;
            if (Info != null && SearchEvent != null)
            {
                SearchEvent(new SearchEventArgs
                {
                    SearchOptions = new SearchOptions
                    {
                        SearchForMovies = (Info.Type == typeof(Movie)),
                        SearchForActors = (Info.Type == typeof(Actor)),
                        SearchOnTmdb = true,
                        SearchTerm = Info.Tag,
                    }
                });
            }
        }

        private static void OnImagesChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            UpdateImages(o as MovieScrollPanel);
        }

        private void _btnLeft_Click(object sender, RoutedEventArgs e)
        {
            _ImgScrollViewer.ScrollToHorizontalOffset(_currentScrollViewPosition - REPOSITION_SPEED);
        }

        private void _btnRight_Click(object sender, RoutedEventArgs e)
        {
            _ImgScrollViewer.ScrollToHorizontalOffset(_currentScrollViewPosition + REPOSITION_SPEED);
        }

        public void _ImgScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            _currentScrollViewPosition = e.HorizontalOffset;
        }

        private void _layoutRoot_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {

            _ImgScrollViewer.ScrollToHorizontalOffset(_currentScrollViewPosition - e.Delta);
        }
    }
}
