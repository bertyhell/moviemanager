using System;
using System.Collections.Generic;
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
using Common;
using Model;

namespace MovieManager.APP.CommonControls
{
    /// <summary>
    /// Interaction logic for RatingEditor.xaml
    /// </summary>
    public partial class RatingControl : UserControl
    {
        public static readonly DependencyProperty RatingProperty =
            DependencyProperty.Register("Rating", typeof(Double), typeof(RatingControl), new PropertyMetadata(-2.0, RatingChanged));

        public static Uri EmptyStar = new Uri("/MovieManager.APP;component/Images/EmptyStar.png", UriKind.Relative);
        public static Uri SelectedStar = new Uri("/MovieManager.APP;component/Images/SelectedStar.png", UriKind.Relative);
        public static Uri HalfSelectedStar = new Uri("/MovieManager.APP;component/Images/HalfSelectedStar.png", UriKind.Relative);
        private List<Image> _stars = new List<Image>();
        private int _starCount = 5;
        private Boolean _isInitialized;

        public RatingControl()
        {
            InitializeComponent();
        }

        protected int StarCount
        {
            get { return _starCount; }
            set { _starCount = value; }
        }

        public List<Image> Stars
        {
            get { return _stars; }
            set { _stars = value; }
        }

        public Double Rating
        {
            get { return (Double)GetValue(RatingProperty); }
            set { SetValue(RatingProperty, value); }
        }

        protected virtual void Init()
        {
            if (!_isInitialized)
            {
                this.Width = _starCount*16;
                this.Height = 16;
                if (_layoutroot != null)
                {
                    _layoutroot.ColumnDefinitions.Clear();
                    _layoutroot.Children.Clear();
                }
                else
                    _layoutroot = new Grid();
                for (int i = 0; i < _starCount; i++)
                {
                    _layoutroot.ColumnDefinitions.Add(new ColumnDefinition {Width = new GridLength(16)});
                    Image LocalImage = ImageFactory.GetImage(EmptyStar);
                    Grid.SetColumn(LocalImage, i);
                    _stars.Add(LocalImage);
                    _layoutroot.Children.Add(LocalImage);
                }
                _isInitialized = true;
            }

            RefreshStars(Rating, SelectedStar, HalfSelectedStar, EmptyStar);
        }

        protected void RefreshStars(double rating, Uri selectedStar, Uri halfSelectedStar, Uri emptyStar)
        {
            if (rating < 0) rating = 0;
            // adapt the rating to the star count
            double NormalizedRating = rating / 10 * StarCount;

            //determine the amount of full colered stars
            int SelectedStarCount = (int)Math.Floor(NormalizedRating);

            //determine the remaining rating -> used to determine if a half colered star is needed
            double RatingRest = NormalizedRating - SelectedStarCount;
            SelectedStarCount += (RatingRest > 0.75 ? 1 : 0);
            int HalfSelectedStarCount = (RatingRest <= 0.75 && RatingRest >= 0.25 ? 1 : 0);

            for (int i = 0; i < _starCount; i++)
            {
                if (i < SelectedStarCount)
                    Stars[i].Source = ImageFactory.GetImageSource(selectedStar);
                else if (i < SelectedStarCount + HalfSelectedStarCount)
                    Stars[i].Source = ImageFactory.GetImageSource(halfSelectedStar);
                else
                    Stars[i].Source = ImageFactory.GetImageSource(emptyStar);
            }

        }

        private static void RatingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RatingEditorControl)
            {
                (d as RatingEditorControl).Init();
            }
            else
            {
                (d as RatingControl).Init();
            }
        }

    }
}
