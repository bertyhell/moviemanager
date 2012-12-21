using System;
using System.Windows.Input;

namespace MovieManager.APP.Panels
{
    /// <summary>
    /// Interaction logic for RatingEditorControl.xaml
    /// </summary>
    public class RatingEditorControl : RatingControl, IDisposable
    {

        public static Uri MouseOverHalfSelectedStar = new Uri("/MovieManager;component/Images/MouseOverHalfStar.png", UriKind.Relative);
        public static Uri MouseOverSelectedStar = new Uri("/MovieManager;component/Images/MouseOverStar.png", UriKind.Relative);
        private double _oldMouseOverRating = -1.0;
        private double _mouseOverRating = -1.0;
        private readonly int _width;

        public RatingEditorControl()
        {
            InitializeComponent();
            _width = StarCount * 16;
        }
        protected override void Init()
        {
            base.Init();
            MouseMove += RatingEditorControlMouseMove;
            MouseUp += RatingEditorControlMouseUp;
            MouseLeave += RatingEditorControlMouseLeave;
        }

        private void RatingEditorControlMouseLeave(object sender, MouseEventArgs e)
        {
            _oldMouseOverRating = -1;
            _mouseOverRating = -1;
            RefreshStars(Rating, SelectedStar, HalfSelectedStar, EmptyStar);
        }

        private void RatingEditorControlMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.GetPosition(this).X <= _width + 5)
            {
                Rating = _mouseOverRating;
                RefreshStars(Rating, SelectedStar, HalfSelectedStar, EmptyStar);
            }
        }



        public void RatingEditorControlMouseMove(object sender, MouseEventArgs e)
        {
            double MousePositionX = e.GetPosition(this).X;
            if (MousePositionX > _width)
            {
                MousePositionX = _width;
            }
            _oldMouseOverRating = _mouseOverRating;

            //Determine voted score
            _mouseOverRating = MousePositionX * 2.0 / 16;
            double Hulp = Math.Floor(_mouseOverRating);
            _mouseOverRating = Hulp + ((_mouseOverRating - Hulp < 0.5) ? 0 : 1);

            if (Math.Abs(_oldMouseOverRating - _mouseOverRating) > 0.005)
            {
                RefreshStars(_mouseOverRating, MouseOverSelectedStar, MouseOverHalfSelectedStar, EmptyStar);
            }
        }



        public void Dispose()
        {
            MouseMove -= RatingEditorControlMouseMove;
            MouseUp -= RatingEditorControlMouseUp;
            MouseLeave -= RatingEditorControlMouseLeave;

        }

    }
}
