using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

            Parameters = new List<Control> { new TextBox { Text = "test", Tag = 1 }, new Button { Content = "joske", Tag = 2 } };
        }

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

        private List<Control> _parameters;
        private Point _startPoint;

        public List<Control> Parameters
        {
            get
            {
                return _parameters;
            }
            set
            {
                _parameters = value; PropChanged("Parameters");
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

        private void ListMouseMove(object sender, MouseEventArgs e)
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
                    string DragText = ((TextBlock)e.OriginalSource).Text;


                    // Initialize the drag & drop operation
                    DataObject DragData = new DataObject(typeof(String), DragText);
                    DragDrop.DoDragDrop((TextBlock)e.OriginalSource, DragData, DragDropEffects.Move);
                }
                catch
                {
                }
            }
        }

        private void DropParameter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(String)))
            {

                String DragText = (String)e.Data.GetData(typeof(String));

                Control Parameter = Parameters.First(
                                                t =>
                                                t is TextBox && ((TextBox)t).Text == DragText ||
                                                t is Button && ((Button)t).Content.ToString() == DragText);
                Control NewParameter;
                if(Parameter is Button)
                {
                    
                }
                //Control NewParameter = new Control {DataContext = Parameter.DataContext};
                //_parameteredString.Children.Add(NewParameter);
            }
        }

        private void DragEnterParameter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(Control)) || sender == e.Source)
            {
                e.Effects = DragDropEffects.Move;
            }
        }
    }
}
