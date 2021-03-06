﻿using System;
using System.Windows.Input;

namespace Tmc.WinUI.Application.Panels
{
    /// <summary>
    /// Interaction logic for RatingEditorControl.xaml
    /// </summary>
    public class RatingEditorControl : RatingControl, IDisposable
    {

        public static Uri _mouseOverHalfSelectedStar = new Uri("/Tmc.WinUI.Application;component/Images/MouseOverHalfStar.png", UriKind.Relative);
        public static Uri _mouseOverSelectedStar = new Uri("/Tmc.WinUI.Application;component/Images/MouseOverStar.png", UriKind.Relative);
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
            RefreshStars(Rating, _selectedStar, _halfSelectedStar, _emptyStar);
        }

        private void RatingEditorControlMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.GetPosition(this).X <= _width + 5)
            {
                Rating = _mouseOverRating;
                RefreshStars(Rating, _selectedStar, _halfSelectedStar, _emptyStar);
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
                RefreshStars(_mouseOverRating, _mouseOverSelectedStar, _mouseOverHalfSelectedStar, _emptyStar);
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
