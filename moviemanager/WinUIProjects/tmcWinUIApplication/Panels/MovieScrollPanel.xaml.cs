using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Model;
using MovieManager.APP.Panels.Search;
using MovieManager.Common;
using SearchOptions = Tmc.WinUI.Application.Panels.Search.SearchOptions;

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

        private const double REPOSITION_SPEED = 100;

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
                for (int I = 0; I < panel.Images.Count; I++)
                {
                    //add new elements
                    Image NewImage = new Image
                                         {
                                             Source = ImageFactory.GetImage(panel.Images[I].Uri).Source,
                                             Tag = panel.Images[I],
                                             Margin = new Thickness(5)
                                         };
                    //set sources
                    NewImage.MouseUp += panel.NewImageMouseUp;

                    panel._layoutRoot.Children.Add(NewImage);
                    panel._layoutRoot.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength() });
                    Grid.SetColumn(NewImage, I);
                    panel._imagesElements.Add(NewImage);
                }
            }
        }

        private void NewImageMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var Image = sender as Image;
            if (Image != null)
            {
                ImageInfo Info = Image.Tag as ImageInfo;
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
        }

        private static void OnImagesChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            UpdateImages(o as MovieScrollPanel);
        }

        private void BtnLeftClick(object sender, RoutedEventArgs e)
        {
            _ImgScrollViewer.ScrollToHorizontalOffset(_currentScrollViewPosition - REPOSITION_SPEED);
        }

        private void BtnRightClick(object sender, RoutedEventArgs e)
        {
            _ImgScrollViewer.ScrollToHorizontalOffset(_currentScrollViewPosition + REPOSITION_SPEED);
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
