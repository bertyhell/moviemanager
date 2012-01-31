using System.Windows;

namespace VlcPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class VlcWindow
    {
        public VlcWindow()
        {
            InitializeComponent();
        }

        public VlcPlayerControl Player { get { return _vlcPlayer; } }
    }
}
