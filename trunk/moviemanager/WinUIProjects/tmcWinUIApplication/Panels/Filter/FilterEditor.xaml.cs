using System.Linq;
using Tmc.SystemFrameworks.Model;

namespace Tmc.WinUI.Application.Panels.Filter
{
    /// <summary>
    /// Interaction logic for FilterEditor.xaml
    /// </summary>
    public partial class FilterEditor
    {

        private readonly FilterController _controller;
        public FilterEditor()
        {
            InitializeComponent();
            _controller = new FilterController();
            DataContext = _controller;
        }

        public bool FilterVideo(object video)
        {
            return _controller.AppliedFilters.All(filterControl => filterControl.FilterSucceeded((Video) video));
        }

        private void ImageMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _controller.AppliedFilters.Remove((FilterControl) lstFilters.SelectedItem);
            MainController.Instance.Refresh();
        }

        private void ComboBoxSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            cbbFilters.SelectedIndex = -1;
        }
    }
}
