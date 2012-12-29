using System.Windows.Controls;
using Tmc.SystemFrameworks.Model;
using Tmc.WinUI.Application.ViewModels.Panels;

namespace Tmc.WinUI.Application.Panels
{
    /// <summary>
    /// Interaction logic for SerieSelectionControl.xaml
    /// </summary>
    public partial class SerieSelectionControl : UserControl
    {
        private readonly SerieSelectionViewModel _viewModel = new SerieSelectionViewModel();

        public SerieSelectionControl()
        {
            InitializeComponent();
            DataContext = _viewModel;
        }

        public Serie Serie
        {
            get
            {
                if (_viewModel.IsCreateNewSerieSelected)
                    return new Serie {Name = _viewModel.NewSerieName};
                if (_viewModel.SelectedSerie != null)
                    return _viewModel.SelectedSerie;
                return null;
            }
        }
    }
}
