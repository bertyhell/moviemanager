using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Point = System.Windows.Point;

namespace MovieManager.APP.Common
{
    /// <summary>
    /// Interaction logic for ParameteredStringBuilder.xaml
    /// </summary>
    public partial class ParameteredStringBuilder : INotifyPropertyChanged
    {
        private bool _isDragging;
        private UIElement _insertionHint;

        public ParameteredStringBuilder()
        {
            InitializeComponent();

            Parameters = new List<UIElement> { new TextBox { Text = "test" }, new Button { Content = "aaaaaa" }, new Button { Content = "bbbbbb" }, new Button { Content = "cccccc" } };

            _parameterGrid.MouseLeftButtonUp += ParameterMouseUp; // TODO 020: Release drag between elements
            _parameterGrid.MouseMove += ParameterMouseMove;
            _insertionHint = new Rectangle() { Fill = new SolidColorBrush(Colors.Gray), Width = 5, VerticalAlignment = VerticalAlignment.Stretch };



#if DEBUG
            for (int I = 1; I < Parameters.Count; I++)
            {
                _parameterGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                Button Button = new Button { Content = ((Button)Parameters[I]).Content };
                Rectangle Rect = new Rectangle { Tag = Button, Fill = new SolidColorBrush(Color.FromArgb(0, 1, 0, 0)) };
                Rect.MouseLeftButtonDown += ParameterMouseDown;
                //Rect.MouseMove += ParameterMouseMove;
                Rect.MouseUp += ParameterMouseUp;
                Panel.SetZIndex(Rect, 1);

                Grid NewParameter = new Grid();
                NewParameter.Children.Add(Button);
                NewParameter.Children.Add(Rect);

                NewParameter.AllowDrop = true;

                _parameterGrid.Children.Add(NewParameter);
                Grid.SetColumn(NewParameter, I - 1);
            }
#endif
        }

        #region properties

        //private UIElement removing
        private UIElement _draggedElement;

        private String _label;
        public String Label
        {
            get
            {
                return _label;
            }
            set
            {
                _label = value;
                PropChanged("Label");
            }
        }

        private List<UIElement> _parameters;
        private Point _startPoint;

        public List<UIElement> Parameters
        {
            get
            {
                return _parameters;
            }
            set
            {
                _parameters = value;
                PropChanged("Parameters");
            }
        }

        #endregion

        private UIElement GetChildHitByMouse(Grid grid)
        {
            foreach (UIElement Child in grid.Children)
            {
                Point Position = Mouse.GetPosition(Child);
                if (Position.X > 0 && Position.X < Child.RenderSize.Width && Position.Y > 0 && Position.Y < Child.RenderSize.Height)
                {
                    return Child;
                }
            }
            return null;
        }

        #region Dragging

        private void ParameterMouseDown(object sender, MouseButtonEventArgs e)
        {
            Console.Write("v");
            _startPoint = e.GetPosition(null);
            _draggedElement = (UIElement)sender;
            if (_draggedElement is Rectangle)
            {
                _draggedElement = (UIElement)((Rectangle)sender).Parent;
            }
        }

        private void ParameterMouseMove(object sender, MouseEventArgs e)
        {



            Console.Write(".");

            //if (e.LeftButton == MouseButtonState.Pressed)
            //{
            //    Rectangle Control = (Rectangle)sender;
            //    Control.ReleaseMouseCapture();

            //}


            // Get the current mouse position
            Point MousePos = e.GetPosition(null);
            Vector Diff = _startPoint - MousePos;

            if (!_isDragging && e.LeftButton == MouseButtonState.Pressed && (
                                                                    Math.Abs(Diff.X) >
                                                                    SystemParameters.MinimumHorizontalDragDistance ||
                                                                    Math.Abs(Diff.Y) >
                                                                    SystemParameters.MinimumVerticalDragDistance))
            {
                _isDragging = true;
                //Mouse.Capture(_parameterGrid);

                //TODO 020: show drag icon
            }

            if (_isDragging)
            {
                //display insertion hint

                Point Position = e.GetPosition(_parameterGrid);
                if (Position.X > 0 && Position.X < _parameterGrid.RenderSize.Width && Position.Y > 0 && Position.Y < _parameterGrid.RenderSize.Height)
                {
                    UIElement HitByMouse = GetChildHitByMouse(_parameterGrid);
                    if (HitByMouse == null)
                    {
                        return;
                    }

                    DeleteParameter(_insertionHint);


                    //reposition component depending on x location
                    //get new and old column

                    int ColNew = Grid.GetColumn(HitByMouse);

                    //insert insertionhint
                    if (e.GetPosition(HitByMouse).X > HitByMouse.RenderSize.Width / 2)
                    {
                        ColNew++;
                    }
                    InsertParameter(_insertionHint, ColNew);
                    //Console.WriteLine("newcol: " + ColNew + " element: " + ((Button)((Grid)HitByMouse).Children[0]).Content);
                }
                else
                {
                    //display remove parameter icon
                    //remove rectangle isertionHint
                    DeleteParameter(_insertionHint);
                }
            }
        }

        void ParameterMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_isDragging && _draggedElement != null)
            {

                Point Position = e.GetPosition(_parameterGrid);

                if (!(Position.X > 0 && Position.X < _parameterGrid.RenderSize.Width && Position.Y > 0 && Position.Y < _parameterGrid.RenderSize.Height))
                {
                    //delete element from grid (dropped outside _parameterGrid grid)
                    int Col = Grid.GetColumn(_draggedElement);
                    _parameterGrid.ColumnDefinitions.RemoveAt(Col);
                    _parameterGrid.Children.Remove(_draggedElement);

                    foreach (UIElement Child in _parameterGrid.Children)
                    {
                        int TempCol = Grid.GetColumn(Child);
                        if (TempCol > Col)
                        {
                            Grid.SetColumn(Child, TempCol - 1);
                        }
                    }
                }
                else
                {
                    //dropped inside _parameterGrid --> move to mouselocation
                    //UIElement HitByMouse = GetChildHitByMouse(_parameterGrid);
                    if (!_parameterGrid.Children.Contains(_insertionHint))
                    {
                        _draggedElement = null;
                        _isDragging = false;
                        Mouse.Capture(null);
                        return;
                    }


                    //reposition component depending on x location
                    //get new and old column
                    int ColOld = Grid.GetColumn(_draggedElement);
                    int ColNew = Grid.GetColumn(_insertionHint);
                    if (ColOld < ColNew)
                        ColNew--;

                    //int ColNew = Grid.GetColumn(HitByMouse);
                    //if (e.GetPosition(HitByMouse).X > HitByMouse.RenderSize.Width / 2)
                    //{
                    //    if (ColNew < ColOld)
                    //        ColNew++;
                    //}
                    //else if (ColNew > ColOld)
                    //{
                    //    ColNew--;
                    //}

                    //reposition
                    MoveParameter(_draggedElement, ColNew);
                }
            }

            DeleteParameter(_insertionHint);
            _draggedElement = null;
            _isDragging = false;
            Mouse.Capture(null);
        }

        #endregion

        private void MoveParameter(UIElement param, int toCol)
        {
            int FromCol = Grid.GetColumn(param);
            if (FromCol != toCol)
            {
                //move param
                if (toCol > FromCol)
                {
                    //move inbetween items to the left
                    foreach (UIElement UIElement in _parameterGrid.Children)
                    {
                        int Column = Grid.GetColumn(UIElement);
                        if (Column > FromCol && Column <= toCol)
                        {
                            Grid.SetColumn(UIElement, Column - 1);
                        }
                    }
                }
                else if (toCol < FromCol)
                {
                    //move inbetween items to the right
                    foreach (UIElement UIElement in _parameterGrid.Children)
                    {
                        int Column = Grid.GetColumn(UIElement);
                        if (Column < FromCol && Column >= toCol)
                        {
                            Grid.SetColumn(UIElement, Column + 1);
                        }
                    }
                }
                Grid.SetColumn(param, toCol);
            }
        }

        private void InsertParameter(UIElement param, int col)
        {
            _parameterGrid.ColumnDefinitions.Insert(col, new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            foreach (UIElement UIElement in _parameterGrid.Children)
            {
                int Column = Grid.GetColumn(UIElement);
                if (Column >= col)
                {
                    Grid.SetColumn(UIElement, Column + 1);
                }
            }

            _parameterGrid.Children.Add(param);
            Grid.SetColumn(param, col);
        }

        private void DeleteParameter(UIElement param)
        {
            if (_parameterGrid.Children.Contains(param))
            {
                int Col = Grid.GetColumn(param);
                _parameterGrid.ColumnDefinitions.RemoveAt(Col);

                foreach (UIElement UIElement in _parameterGrid.Children)
                {
                    int Column = Grid.GetColumn(UIElement);
                    if (Column > Col)
                    {
                        Grid.SetColumn(UIElement, Column - 1);
                    }
                }
                _parameterGrid.Children.Remove(param);
            }
        }


        private void cbbParameters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (e.AddedItems.Count == 0)
                return;

            UIElement Parameter = (UIElement)e.AddedItems[0];

            UIElement NewParameter;
            if (Parameter is TextBox)
            {
                NewParameter = new TextBox { Text = ((TextBox)Parameter).Text };
                NewParameter.MouseUp += ParameterMouseUp;

            }
            else if (Parameter is Button)
            {
                Button Button = new Button { Content = ((Button)Parameter).Content };
                Rectangle Rect = new Rectangle { Tag = Button, Fill = new SolidColorBrush(Color.FromArgb(0, 1, 0, 0)) };
                Rect.MouseLeftButtonDown += ParameterMouseDown;
                //Rect.MouseMove += ParameterMouseMove;
                Rect.MouseUp += ParameterMouseUp;
                Panel.SetZIndex(Rect, 1);

                NewParameter = new Grid();
                (NewParameter as Grid).Children.Add(Button);
                (NewParameter as Grid).Children.Add(Rect);
            }
            else
            {
                return;
            }
            NewParameter.AllowDrop = true;

            if (_parameterGrid.Children.Count == 0)
            {
                _parameterGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                _parameterGrid.Children.Add(NewParameter);
            }
            else if (sender != _parameterGrid)
            {
                _parameterGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                Grid.SetColumn(NewParameter, _parameterGrid.Children.Count);
                _parameterGrid.Children.Add(NewParameter);
            }
        }



        public void PropChanged(string field)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(field));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
