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
    /// Interaction logic for RatingEditorControl.xaml
    /// </summary>
    public class RatingEditorControl : RatingControl, IDisposable
    {

        public static Uri MouseOverHalfSelectedStar = new Uri("/MovieManager.APP;component/Images/MouseOverHalfStar.png", UriKind.Relative);
        public static Uri MouseOverSelectedStar = new Uri("/MovieManager.APP;component/Images/MouseOverStar.png", UriKind.Relative);
        private double _oldMouseOverRating = -1.0;
        private double _mouseOverRating = -1.0;
        private readonly int _width;

        public RatingEditorControl()
            : base()
        {
            InitializeComponent();
            _width = StarCount * 16;
        }
        protected override void Init()
        {
            base.Init();
            MouseMove += new MouseEventHandler(RatingEditorControl_MouseMove);
            MouseUp += new MouseButtonEventHandler(RatingEditorControl_MouseUp);
            MouseLeave += new MouseEventHandler(RatingEditorControl_MouseLeave);
        }

        private void RatingEditorControl_MouseLeave(object sender, MouseEventArgs e)
        {
            _oldMouseOverRating = -1;
            _mouseOverRating = -1;
            RefreshStars(Rating, SelectedStar, HalfSelectedStar, EmptyStar);
        }

        private void RatingEditorControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.GetPosition(this).X <= _width + 5)
            {
                Rating = _mouseOverRating;
                RefreshStars(Rating, SelectedStar, HalfSelectedStar, EmptyStar);
            }
        }



        public void RatingEditorControl_MouseMove(object sender, MouseEventArgs e)
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

            if (_oldMouseOverRating != _mouseOverRating)
            {
                RefreshStars(_mouseOverRating, MouseOverSelectedStar, MouseOverHalfSelectedStar, EmptyStar);
            }
        }



        public void Dispose()
        {
            MouseMove -= new MouseEventHandler(RatingEditorControl_MouseMove);
            MouseUp -= new MouseButtonEventHandler(RatingEditorControl_MouseUp);
            MouseLeave -= new MouseEventHandler(RatingEditorControl_MouseLeave);

        }

    }
}
