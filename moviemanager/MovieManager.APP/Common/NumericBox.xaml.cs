using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MovieManager.APP.Common
{
    /// <summary>
    /// Interaction logic for NumericBox.xaml
    /// </summary>
    public partial class NumericBox
    {
        public static DependencyProperty NumberProperty = DependencyProperty.Register("Number", typeof (int), typeof (NumericBox), new PropertyMetadata(default(int)));

        public NumericBox()
        {
            InitializeComponent();
        }

        public int Number
        {
            get { return (int) GetValue(NumberProperty); }
            set { SetValue(NumberProperty, value); }
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

    }
}
