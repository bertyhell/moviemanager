﻿using System;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VlcPlayer
{
    class OverlayAdorner : Adorner
    {

        private Control _child;

        // Be sure to call the base class constructor.
        public OverlayAdorner(UIElement adornedElement)
            : base(adornedElement) { }


        protected override int VisualChildrenCount
        {
            get
            {
                return 1;
            }
        }

        protected override Visual GetVisualChild(int index)
        {
            if (index != 0) throw new ArgumentOutOfRangeException();
            return _child;
        }

        public Control Child
        {
            get { return _child; }
            set
            {
                if (_child != null)
                {
                    RemoveVisualChild(_child);
                }
                _child = value;
                if (_child != null)
                {
                    AddVisualChild(_child);
                }
            }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            _child.Measure(constraint);
            return _child.DesiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            _child.Arrange(new Rect(new Point(0, 0), finalSize));
            return new Size(_child.ActualWidth, _child.ActualHeight);
        }
    }
}
