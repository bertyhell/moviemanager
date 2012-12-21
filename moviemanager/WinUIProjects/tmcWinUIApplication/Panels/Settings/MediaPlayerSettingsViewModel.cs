using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MovieManager.APP.Panels.Settings
{
    public class MediaPlayerSettingsViewModel : INotifyPropertyChanged
    {
        public MediaPlayerSettingsViewModel()
        {

        }

        private bool _playOnDoubleCLick;

        public bool PlayOnDoubleCLick
        {
            get { return _playOnDoubleCLick; }
            set
            {
                _playOnDoubleCLick = value;
                OnPropChanged("PlayOnDoubleCLick");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
