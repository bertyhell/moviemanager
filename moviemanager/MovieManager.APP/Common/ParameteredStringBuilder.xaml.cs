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
        public ParameteredStringBuilder()
        {
            InitializeComponent();

            Parameters = new List<UIElement> { new TextBox { Text = "test" }, new Button { Content = "aaaaaa" }, new Button { Content = "bbbbbb" }, new Button { Content = "cccccc" } };

            //_parameteredString.DragLeave += _parameteredString_DragLeave;
            _parameteredString.MouseLeftButtonUp += _parameteredString_MouseLeftButtonUp;
            _parameteredString.Drop += _parameteredString_Drop;

#if DEBUG
            for (int I = 1; I < Parameters.Count; I++)
            {
                _parameteredString.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                Button Button = new Button { Content = ((Button)Parameters[I]).Content };
                Rectangle Rect = new Rectangle { Tag = Button, Fill = new SolidColorBrush(Color.FromArgb(0, 1, 0, 0)) };
                Rect.MouseLeftButtonDown += ListMouseLeftButtonDown;
                Rect.MouseMove += ParameterStringMouseMove;
                Panel.SetZIndex(Rect, 1);

                Grid NewParameter = new Grid();
                NewParameter.Children.Add(Button);
                NewParameter.Children.Add(Rect);

                NewParameter.DragEnter += DragEnterParameter;
                NewParameter.Drop += DropParameter;
                NewParameter.AllowDrop = true;

                _parameteredString.Children.Add(NewParameter);
                Grid.SetColumn(NewParameter, I - 1);
            }
#endif
        }

        void _parameteredString_Drop(object sender, DragEventArgs e)
        {
        }

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

        void _parameteredString_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UIElement HitByMouse = GetChildHitByMouse(_parameteredString);

            if (_draggedElement == null || HitByMouse == null) return;
            Point Position = e.GetPosition(_parameteredString);
            if (!(Position.X > 0 && Position.X < _parameteredString.RenderSize.Width && Position.Y > 0 && Position.Y < _parameteredString.RenderSize.Height))
            {
                //delete element from grid
                int Col = Grid.GetColumn(_draggedElement);
                _parameteredString.ColumnDefinitions.RemoveAt(Col);
                _parameteredString.Children.Remove(_draggedElement);

                foreach (UIElement Child in _parameteredString.Children)
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
                //reposition component depending on x location
                int ColNew = Grid.GetColumn((UIElement)HitByMouse);
                int ColOld = Grid.GetColumn(_draggedElement);
                if (e.GetPosition((UIElement)HitByMouse).X > ((UIElement)HitByMouse).RenderSize.Width / 2)
                {
                    if (ColNew < ColOld)
                        ColNew++;
                }
                else if (ColNew > ColOld)
                {
                    ColNew--;
                }
                if (ColNew > ColOld)
                {
                    //move inbetween items to the left
                    foreach (UIElement UIElement in _parameteredString.Children)
                    {
                        int Column = Grid.GetColumn(UIElement);
                        if (Column > ColOld && Column <= ColNew)
                        {
                            Grid.SetColumn(UIElement, Column - 1);
                        }
                    }
                }
                else if (ColNew < ColOld)
                {
                    //move inbetween items to the right
                    foreach (UIElement UIElement in _parameteredString.Children)
                    {
                        int Column = Grid.GetColumn(UIElement);
                        if (Column < ColOld && Column >= ColNew)
                        {
                            Grid.SetColumn(UIElement, Column + 1);
                        }
                    }
                }
                Grid.SetColumn(_draggedElement, ColNew);
            }

            _draggedElement = null;

            Mouse.Capture(null);
        }

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

        public void PropChanged(string field)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(field));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void ListMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _startPoint = e.GetPosition(null);
        }

        private void ComboItemMouseMove(object sender, MouseEventArgs e)
        {
            // Get the current mouse position
            Point MousePos = e.GetPosition(null);
            Vector Diff = _startPoint - MousePos;

            if (e.LeftButton == MouseButtonState.Pressed && (
                Math.Abs(Diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(Diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                try
                {
                    //get parameter
                    UIElement DragControl = (UIElement)((Rectangle)e.OriginalSource).Tag;


                    //// Initialize the drag & drop operation
                    DataObject DragData = new DataObject(typeof(UIElement), DragControl);
                    DragDrop.DoDragDrop((Rectangle)e.OriginalSource, DragData, DragDropEffects.Move);
                }
                catch
                {
                }
            }
        }

        private void ParameterStringMouseMove(object sender, MouseEventArgs e)
        {
            // Get the current mouse position
            Point MousePos = e.GetPosition(null);
            Vector Diff = _startPoint - MousePos;

            if (e.LeftButton == MouseButtonState.Pressed && (
                Math.Abs(Diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(Diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                try
                {
                    //get parameter
                    //UIElement DragControl = (UIElement)((Rectangle)e.OriginalSource).Tag;

                    //// Initialize the drag & drop operation
                    //DataObject DragData = new DataObject(typeof(UIElement), DragControl);
                    //DragDrop.DoDragDrop((Rectangle)e.OriginalSource, DragData, DragDropEffects.Move);

                    _draggedElement = (UIElement)((Rectangle)e.OriginalSource).Parent;
                    Mouse.Capture(_parameteredString);
                }
                catch
                {
                }
            }
        }

        private void DropParameter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(UIElement)))
            {

                UIElement Parameter = (UIElement)e.Data.GetData(typeof(UIElement));

                //UIElement Parameter = Parameters.First(
                //                                t =>
                //                                t is TextBox && ((TextBox)t).Text == DragControl ||
                //                                t is Button && ((Button)t).Content.ToString() == DragControl);
                UIElement NewParameter;
                if (Parameter is TextBox)
                {
                    NewParameter = new TextBox { Text = ((TextBox)Parameter).Text };

                }
                else if (Parameter is Button)
                {
                    Button Button = new Button { Content = ((Button)Parameter).Content };
                    Rectangle Rect = new Rectangle { Tag = Button, Fill = new SolidColorBrush(Color.FromArgb(0, 1, 0, 0)) };
                    Rect.MouseLeftButtonDown += ListMouseLeftButtonDown;
                    Rect.MouseMove += ParameterStringMouseMove;
                    Panel.SetZIndex(Rect, 1);

                    NewParameter = new Grid();
                    (NewParameter as Grid).Children.Add(Button);
                    (NewParameter as Grid).Children.Add(Rect);
                }
                else
                {
                    return;
                }
                NewParameter.DragEnter += DragEnterParameter;
                NewParameter.Drop += DropParameter;
                NewParameter.AllowDrop = true;


                if (_parameteredString.Children.Count == 0)
                {
                    _parameteredString.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    _parameteredString.Children.Add(NewParameter);
                }
                else if (sender != _parameteredString)
                {
                    int Col = Grid.GetColumn((UIElement)sender);
                    if (e.GetPosition((UIElement)sender).X > ((UIElement)sender).RenderSize.Width / 2)
                    {
                        Col++;
                    }
                    _parameteredString.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    foreach (UIElement Child in _parameteredString.Children)
                    {
                        int TempCol = Grid.GetColumn(Child);
                        if (TempCol >= Col)
                        {
                            Grid.SetColumn(Child, TempCol + 1);
                        }
                    }
                    _parameteredString.Children.Add(NewParameter);
                    Grid.SetColumn(NewParameter, Col);
                }
            }
        }

        private void DragEnterParameter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(UIElement)) || sender == e.Source)
            {
                e.Effects = DragDropEffects.Move;
            }
        }
    }
}
