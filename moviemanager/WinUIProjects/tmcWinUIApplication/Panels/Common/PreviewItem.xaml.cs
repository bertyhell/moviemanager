﻿using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Tmc.SystemFrameworks.Model.Interfaces;

namespace Tmc.WinUI.Application.Panels.Common
{
    /// <summary>
    /// Interaction logic for PreviewItem.xaml
    /// </summary>
    public partial class PreviewItem : UserControl, INotifyPropertyChanged
    {
        public PreviewItem()
        {
            InitializeComponent();
            _contentGrid.DataContext = this;
        }

        public static readonly DependencyProperty ITEM_PROPERTY =
            DependencyProperty.Register("Item", typeof(IPreviewInfoRetriever), typeof(PreviewItem), new PropertyMetadata(ItemPropertyChanged));

        public static void ItemPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((PreviewItem)sender).Item = (IPreviewInfoRetriever)e.NewValue;
        }

        public static readonly DependencyProperty LABEL_VISIBILITY_PROPERTY =
    DependencyProperty.Register("LabelVisibility", typeof(Visibility), typeof(PreviewItem), new PropertyMetadata(LabelVisibilityPropertyChanged));

        public static void LabelVisibilityPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((PreviewItem)sender).LabelVisibility = (Visibility)e.NewValue;
        }

        private IPreviewInfoRetriever _item;
        public IPreviewInfoRetriever Item
        {
            get
            {
                return _item;
            }
            set
            {
                _item = value;
                OnPropChanged("Item");
            }
        }


        private Visibility _labelVisibility = Visibility.Visible;//TODO 030 get value from usersettings
        public Visibility LabelVisibility
        {
            get { return _labelVisibility; }
            set
            {
                _labelVisibility = value;
                OnPropChanged("LabelVisibility");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
