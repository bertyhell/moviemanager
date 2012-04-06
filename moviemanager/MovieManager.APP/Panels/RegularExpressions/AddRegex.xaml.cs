using System.Windows;

namespace MovieManager.APP.Panels.RegularExpressions
{
    /// <summary>
    /// Interaction logic for AddRegex.xaml
    /// </summary>
    public partial class AddRegex
    {
        public AddRegex()
        {
            InitializeComponent();
            
        }

        public string RegularExpression
        {
            get { return _txtRegex.Text; }
            set { _txtRegex.Text = value; }
        }

        private void BtnOkClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
