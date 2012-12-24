using System.Collections.Generic;
using System.ComponentModel;
using Model;

namespace Tmc.WinUI.Application.ViewModels.Panels
{
    class SerieSelectionViewModel : INotifyPropertyChanged
    {
        private bool _isCreateNewSerieSelected;
        private string _newSerieName;
        private List<Serie> _series;
        private Serie _selectedSerie;

        public SerieSelectionViewModel()
        {
            _isCreateNewSerieSelected = true;
        }

        public bool IsCreateNewSerieSelected
        {
            get { return _isCreateNewSerieSelected; }
            set
            {
                if (_isCreateNewSerieSelected != value)
                {
                    _isCreateNewSerieSelected = value;
                    OnPropertyChanged("IsCreateNewSerieSelected");
                    OnPropertyChanged("IsSelectSerieSelected");
                }
            }
        }

        public bool IsSelectSerieSelected
        {
            get { return !_isCreateNewSerieSelected; }
            set
            {
                if (_isCreateNewSerieSelected == value)
                {
                    _isCreateNewSerieSelected = !value;
                    OnPropertyChanged("IsCreateNewSerieSelected");
                    OnPropertyChanged("IsSelectSerieSelected");
                }
            }
        }

        public string NewSerieName
        {
            get { return _newSerieName; }
            set
            {
                if (_newSerieName != value)
                {
                    _newSerieName = value;
                    OnPropertyChanged("NewSerieName");
                }
            }
        }

        public List<Serie> Series
        {
            get { return _series; }
        }

        public Serie SelectedSerie
        {
            get { return _selectedSerie; }
            set
            {
                if (_selectedSerie != value)
                {
                    _selectedSerie = value;
                    OnPropertyChanged("SelectedSerie");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
