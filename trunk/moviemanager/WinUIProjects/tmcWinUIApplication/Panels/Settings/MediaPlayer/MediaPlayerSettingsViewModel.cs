using System.Collections.Generic;
using System.ComponentModel;
using MovieManager.PLAYER.Logic;

namespace Tmc.WinUI.Application.Panels.Settings.MediaPlayer
{
    public class MediaPlayerSettingsViewModel : INotifyPropertyChanged
    {
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

        public MediaPlayerSettings MediaPlayerSettings
        {
            get
            {
                return new MediaPlayerSettings
                           {
                               MediaPlayers = _mediaPlayers,
                               SelectedMediaPlayer = _selectedMediaPlayer
                           };
            }
            set
            {
                if(value != null)
                {
                    _mediaPlayers = value.MediaPlayers;
                    _selectedMediaPlayer = value.SelectedMediaPlayer;
                    OnPropChanged("SelectedMediaPlayer");
                    OnPropChanged("MediaPlayers");
                    OnPropChanged("MediaPlayerSettings");
                }
            }
        }

        private string _selectedMediaPlayer;
        public string SelectedMediaPlayer
        {
            get { return _selectedMediaPlayer; }
            set
            {
                _selectedMediaPlayer = value;
                OnPropChanged("SelectedMediaPlayer");
            }
        }

        private Dictionary<string, string> _mediaPlayers;
        public Dictionary<string, string> MediaPlayers
        {
            get { return _mediaPlayers; }
            set
            {
                _mediaPlayers = value;
                OnPropChanged("MediaPlayers");
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
