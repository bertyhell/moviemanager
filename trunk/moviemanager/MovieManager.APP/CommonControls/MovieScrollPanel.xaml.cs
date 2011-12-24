using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace MovieManager.APP.CommonControls
{
    /// <summary>
    /// Interaction logic for MovieScrollPanel.xaml
    /// </summary>
    public partial class MovieScrollPanel : UserControl
    {
        private List<Image> _imagesElements;
        public readonly static DependencyProperty ImagesProperty = DependencyProperty.Register("Images", typeof(List<Uri>),
                                                                          typeof(MovieScrollPanel), new PropertyMetadata(new List<Uri>(), OnImagesChanged));

        private double _imageWidth = 150;
        private double _repositionSpeed = 100;

        private double _currentScrollViewPosition = 0;

        public MovieScrollPanel()
        {
            InitializeComponent();
            _imagesElements = new List<Image>();
            _ImgScrollViewer.ScrollChanged += new ScrollChangedEventHandler(_ImgScrollViewer_ScrollChanged);
        }

        public List<Uri> Images
        {
            get { return (List<Uri>)GetValue(ImagesProperty); }
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
                    NewImage.Width = panel._imageWidth;
                    Canvas.SetLeft(NewImage, 10 + panel._imageWidth * i);
                    //set sources
                    NewImage.Source = Common.ImageFactory.GetImage(panel.Images[i]).Source;
                    NewImage.Margin = new Thickness(5);

                    panel._layoutRoot.Children.Add(NewImage);
                    panel._layoutRoot.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength() });
                    Grid.SetColumn(NewImage, i);
                    panel._imagesElements.Add(NewImage);
                }
            }
        }

        private static void OnImagesChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            UpdateImages(o as MovieScrollPanel);
        }

        private void _btnLeft_Click(object sender, RoutedEventArgs e)
        {
            _ImgScrollViewer.ScrollToHorizontalOffset(_currentScrollViewPosition - _repositionSpeed);
        }

        private void _btnRight_Click(object sender, RoutedEventArgs e)
        {
            _ImgScrollViewer.ScrollToHorizontalOffset(_currentScrollViewPosition + _repositionSpeed);
        }



        public void _ImgScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            _currentScrollViewPosition = e.HorizontalOffset;
        }

        private void _ImgScrollViewer_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            _ImgScrollViewer.ScrollToHorizontalOffset(_currentScrollViewPosition + e.Delta);
        }

        private void _layoutRoot_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            
            _ImgScrollViewer.ScrollToHorizontalOffset(_currentScrollViewPosition + e.Delta);
        }
    }
}
