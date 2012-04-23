using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MovieManager.APP.Common
{
    /// <summary>
    /// Interaction logic for NumericBox.xaml
    /// </summary>
    public partial class NumericBox : INotifyPropertyChanged
    {


        public NumericBox()
        {
            InitializeComponent();
        }

        private string _text;//TODO 001 change this to dependancy property -> binding becomes possible
        public String Text
        {
            get { return _text; }
            set
            {
                _text = value;
                PropChanged("Text");
            }
        }


        private static bool IsTextAllowed(string text)
        {
            var Regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
            return !Regex.IsMatch(text);
        }

        private void CheckPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void TextBoxPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                var Text = (String)e.DataObject.GetData(typeof(String));
                if (!IsTextAllowed(Text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;


        public void PropChanged(string title)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(title));
            }
        }
    }
}
