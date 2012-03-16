using System.Windows;

namespace MovieManager.APP.Panels.RegularExpressions
{
    /// <summary>
    /// Interaction logic for AddRegex.xaml
    /// </summary>
    public partial class AddRegex : Window
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

        private void _btnOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }
    }
}
