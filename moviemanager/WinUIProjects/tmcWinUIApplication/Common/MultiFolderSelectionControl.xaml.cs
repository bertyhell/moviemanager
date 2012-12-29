using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using Ookii.Dialogs.Wpf;
using Tmc.WinUI.Application.Localization;
using UserControl = System.Windows.Controls.UserControl;

namespace Tmc.WinUI.Application.Common
{
    /// <summary>
    /// Interaction logic for MultiFolderSelectionControl.xaml
    /// </summary>
    public partial class MultiFolderSelectionControl : UserControl, INotifyPropertyChanged
    {
        private ObservableCollection<Uri> _folders;
        private Uri _defaultPath;
        private Uri _selectedFolder;

        public MultiFolderSelectionControl()
        {
            InitializeComponent();
            _folders = new ObservableCollection<Uri>();
            _defaultPath = new Uri(@"C:\");
            DataContext = this;
        }

        public ObservableCollection<Uri> Folders
        {
            get { return _folders; }
            set
            {
                if (_folders != value)
                {
                    _folders = value;
                    OnPropertyChanged("Folders");
                }
            }
        }

        public Uri SelectedFolder
        {
            get { return _selectedFolder; }
            set
            {
                if (_selectedFolder != value)
                {
                    _selectedFolder = value;
                    OnPropertyChanged("SelectedFolder");
                }
            }
        }

        public Uri DefaultPath
        {
            get { return _defaultPath; }
            set
            {
                if (_defaultPath != value)
                {
                    _defaultPath = value;
                    OnPropertyChanged("DefaultPath");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog Dialog = new VistaFolderBrowserDialog { Description = Resource.PleaseSelectAFolder, UseDescriptionForTitle = true, SelectedPath = DefaultPath.AbsolutePath };
            bool? ShowDialogResult = Dialog.ShowDialog();
            if (ShowDialogResult != null && (bool)ShowDialogResult)
            {
                _folders.Add(new Uri(Dialog.SelectedPath));
            }
        }

        private void ButtonRemove_OnClick(object sender, RoutedEventArgs e)
        {
            if (_selectedFolder != null)
                _folders.Remove(_selectedFolder);

        }

        private void ButtonEdit_OnClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button Button = sender as System.Windows.Controls.Button;
            if (Button == null)
                return;
            _selectedFolder = Button.DataContext as Uri;
            VistaFolderBrowserDialog Dialog = new VistaFolderBrowserDialog { Description = Resource.PleaseSelectAFolder, UseDescriptionForTitle = true, SelectedPath = _selectedFolder.AbsolutePath };

            bool? ShowDialogResult = Dialog.ShowDialog();
            if (ShowDialogResult != null && (bool)ShowDialogResult)
            {
                int Index = _folders.IndexOf(_selectedFolder);
                if (Index >= 0 && Index < _folders.Count)
                {
                    _folders[Index] = new Uri(Dialog.SelectedPath);
                    _selectedFolder = _folders[Index];
                    OnPropertyChanged("Folders");
                    OnPropertyChanged("SelectedFolder");
                }
            }
        }
    }
}
