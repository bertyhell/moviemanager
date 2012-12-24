﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
    }
}
