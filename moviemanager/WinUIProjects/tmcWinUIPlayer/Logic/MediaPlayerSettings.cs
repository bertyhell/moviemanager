using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Collections;
using System.ComponentModel;
using Tmc.SystemFrameworks.Common;

namespace Tmc.WinUI.Player.Logic
{
    public class MediaPlayerSettings
    {
        public MediaPlayerSettings()
        {
            _mediaPlayers = new Dictionary<string, string>();
            _mediaPlayers.Add("Internal Player", "");
            _mediaPlayers.Add("VLC Media Player", Path.Combine(RegistryHelper.GetInstallationPath("VLC"), "vlc.exe"));
            _selectedMediaPlayer = "VLC Media Player";
        }

        private string _selectedMediaPlayer;
        public string SelectedMediaPlayer
        {
            get { return _selectedMediaPlayer; }
            set
            {
                _selectedMediaPlayer = value;
            }
        }

        private Dictionary<string, string> _mediaPlayers;
        [XmlIgnore]
        public Dictionary<string, string> MediaPlayers
        {
            get { return _mediaPlayers; }
            set
            {
                _mediaPlayers = value;
                if (_mediaPlayers.Count == 0)
                    _selectedMediaPlayer = null;
                else if (_selectedMediaPlayer == null || !_mediaPlayers.ContainsKey(_selectedMediaPlayer))
                    _selectedMediaPlayer = _mediaPlayers.GetEnumerator().Current.Key;
            }
        }

        [XmlArray(ElementName = "MediaPlayers")]
        [XmlArrayItem(ElementName = "MediaPlayer")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public List<DictionaryEntry> MediaPlayersList
        {
            get
            {
                List<DictionaryEntry> RetVal = new List<DictionaryEntry>();
                foreach (KeyValuePair<string, string> KeyValuePair in _mediaPlayers)
                {
                    RetVal.Add(new DictionaryEntry(KeyValuePair.Key, KeyValuePair.Value));
                }
                return RetVal;
            }
            set
            {
                if(value != null)
                {
                    _mediaPlayers = new Dictionary<string, string>();
                    foreach (DictionaryEntry DictionaryEntry in value)
                    {
                        _mediaPlayers.Add((string)DictionaryEntry.Key, (string)DictionaryEntry.Value);
                    }
                }
            }
        }
    }
}
