using System;
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
        private readonly FrameworkElement _insertionHint;

        public ParameteredStringBuilder()
        {
            InitializeComponent();

            Parameters = new List<FrameworkElement> { new TextBox { Text = "test" }, new Button { Content = "aaaaaa" }, new Button { Content = "bbbbbb" }, new Button { Content = "cccccc" } };

            _parameterGrid.MouseLeftButtonUp += ParameterMouseUp; // TODO 020: Release drag between elements
            _parameterGrid.MouseMove += ParameterMouseMove;
            _insertionHint = new Rectangle { Fill = new SolidColorBrush(Colors.Gray), Width = 2, VerticalAlignment = VerticalAlignment.Stretch };
            ContentItemsMargin = new Thickness(5, 0, 5, 0);


#if DEBUG
            for (int I = 1; I < Parameters.Count; I++)
            {
                _parameterGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                Button Button = new Button { Content = ((Button)Parameters[I]).Content };
                Rectangle Rect = new Rectangle { Tag = Button, Fill = new SolidColorBrush(Color.FromArgb(0, 1, 0, 0)) };
                Rect.MouseLeftButtonDown += ParameterMouseDown;
                Rect.Cursor = Cursors.SizeAll;
                //Rect.MouseMove += ParameterMouseMove;
                Rect.MouseUp += ParameterMouseUp;
                Panel.SetZIndex(Rect, 1);

                Grid NewParameter = new Grid();
                NewParameter.Children.Add(Button);
                NewParameter.Children.Add(Rect);
                NewParameter.Margin = ContentItemsMargin;


                NewParameter.AllowDrop = true;

                _parameterGrid.Children.Add(NewParameter);
                Grid.SetColumn(NewParameter, I - 1);
            }
#endif
        }

        #region properties

        //private FrameworkElement removing
        private FrameworkElement _draggedElement;

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

        private List<FrameworkElement> _parameters;
        private Point _startPoint;

        public List<FrameworkElement> Parameters
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

        private Thickness _contentItemsMargin;
        public Thickness ContentItemsMargin
        {
            get { return _contentItemsMargin; }
            set
            {
                _contentItemsMargin = value;
                foreach (FrameworkElement Child in _parameterGrid.Children)
                {
                    Child.Margin = value;
                }
            }
        }

        #endregion

        private FrameworkElement GetChildHitByMouse(Grid grid)
        {
            foreach (FrameworkElement Child in grid.Children)
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
            Console.WriteLine("mouse down");
            _startPoint = e.GetPosition(null);
            if (sender is Rectangle)
            {
                _draggedElement = (FrameworkElement)((Rectangle)sender).Parent;
            }
        }

        private void ParameterMouseMove(object sender, MouseEventArgs e)
        {
            Console.Write(".");
            // Get the current mouse position
            Point MousePos = e.GetPosition(null);
            Vector Diff = _startPoint - MousePos;

            if (_draggedElement != null && !_isDragging && e.LeftButton == MouseButtonState.Pressed && (
                                                                    Math.Abs(Diff.X) >
                                                                    SystemParameters.MinimumHorizontalDragDistance ||
                                                                    Math.Abs(Diff.Y) >
                                                                    SystemParameters.MinimumVerticalDragDistance))
            {
                _isDragging = true;
                //TODO 020: show drag icon
            }

            if (_isDragging)
            {
                //display insertion hint

                Point Position = e.GetPosition(_parameterGrid);
                if (Position.X > 0 && Position.X < _parameterGrid.RenderSize.Width && Position.Y > 0 && Position.Y < _parameterGrid.RenderSize.Height)
                {
                    FrameworkElement HitByMouse = GetChildHitByMouse(_parameterGrid);
                    if (HitByMouse == null)
                    {
                        _isDragging = false;
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
            Console.WriteLine("mouse up");
            if (_isDragging && _draggedElement != null)
            {

                Point Position = e.GetPosition(_parameterGrid);

                if (!(Position.X > 0 && Position.X < _parameterGrid.RenderSize.Width && Position.Y > 0 && Position.Y < _parameterGrid.RenderSize.Height))
                {
                    //delete element from grid (dropped outside _parameterGrid grid)
                    int Col = Grid.GetColumn(_draggedElement);
                    _parameterGrid.ColumnDefinitions.RemoveAt(Col);
                    _parameterGrid.Children.Remove(_draggedElement);

                    foreach (FrameworkElement Child in _parameterGrid.Children)
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
                    //FrameworkElement HitByMouse = GetChildHitByMouse(_parameterGrid);
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

                    //reposition
                    MoveParameter(_draggedElement, ColNew); //TODO 090 when release between controls -> move draggeditem to replace insertion hint
                }
            }

            DeleteParameter(_insertionHint);
            _draggedElement = null;
            _isDragging = false;
            Mouse.Capture(null);
        }

        #endregion

        private void MoveParameter(FrameworkElement param, int toCol)
        {
            int FromCol = Grid.GetColumn(param);
            if (FromCol != toCol)
            {
                _parameterGrid.ColumnDefinitions[toCol].Width = new GridLength(1, GridUnitType.Star);

                //move param
                if (toCol > FromCol)
                {
                    //move inbetween items to the left
                    foreach (FrameworkElement FrameworkElement in _parameterGrid.Children)
                    {
                        int Column = Grid.GetColumn(FrameworkElement);
                        if (Column > FromCol && Column <= toCol)
                        {
                            Grid.SetColumn(FrameworkElement, Column - 1);
                        }
                    }
                }
                else if (toCol < FromCol)
                {
                    //move inbetween items to the right
                    foreach (FrameworkElement FrameworkElement in _parameterGrid.Children)
                    {
                        int Column = Grid.GetColumn(FrameworkElement);
                        if (Column < FromCol && Column >= toCol)
                        {
                            Grid.SetColumn(FrameworkElement, Column + 1);
                        }
                    }
                }
                Grid.SetColumn(param, toCol);
            }
        }

        private void InsertParameter(FrameworkElement param, int col)
        {
            //foreach (var ColDef in _parameterGrid.ColumnDefinitions)
            //{
            //    ColDef.Width = new GridLength(1, GridUnitType.Star);
            //}
            _parameterGrid.ColumnDefinitions.Insert(col, new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

            foreach (FrameworkElement FrameworkElement in _parameterGrid.Children)
            {
                int Column = Grid.GetColumn(FrameworkElement);
                if (Column >= col)
                {
                    Grid.SetColumn(FrameworkElement, Column + 1);
                }
            }

            _parameterGrid.Children.Add(param);
            Grid.SetColumn(param, col);
        }

        private void DeleteParameter(FrameworkElement param)
        {
            if (_parameterGrid.Children.Contains(param))
            {
                int Col = Grid.GetColumn(param);
                _parameterGrid.ColumnDefinitions.RemoveAt(Col);

                foreach (FrameworkElement FrameworkElement in _parameterGrid.Children)
                {
                    int Column = Grid.GetColumn(FrameworkElement);
                    if (Column > Col)
                    {
                        Grid.SetColumn(FrameworkElement, Column - 1);
                    }
                }
                _parameterGrid.Children.Remove(param);
            }
        }


        private void CbbParametersSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
                return;

            FrameworkElement Parameter = (FrameworkElement)e.AddedItems[0];

            FrameworkElement NewParameter;
            if (Parameter is TextBox)
            {
                NewParameter = new TextBox { Text = ((TextBox)Parameter).Text };
                NewParameter.Margin = ContentItemsMargin;
                NewParameter.MouseUp += ParameterMouseUp;
            }
            else if (Parameter is Button)
            {
                Button Button = new Button { Content = ((Button)Parameter).Content };
                Rectangle Rect = new Rectangle { Tag = Button, Fill = new SolidColorBrush(Color.FromArgb(0, 1, 0, 0)) };
                Rect.MouseLeftButtonDown += ParameterMouseDown;
                Rect.Cursor = Cursors.SizeAll;
                //Rect.MouseMove += ParameterMouseMove;
                Rect.MouseUp += ParameterMouseUp;
                Panel.SetZIndex(Rect, 1);

                NewParameter = new Grid();
                (NewParameter as Grid).Children.Add(Button);
                (NewParameter as Grid).Children.Add(Rect);
                NewParameter.Margin = ContentItemsMargin;
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
            cbbParameters.SelectedItem = null;
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
