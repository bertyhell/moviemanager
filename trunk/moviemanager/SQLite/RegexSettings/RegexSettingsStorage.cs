using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Common;
using SQLite.Properties;

namespace SQLite.RegexSettings
{
    public static class RegexSettingsStorage
    {
        private static SettingsSaver _settingsSaver;

        static RegexSettingsStorage()
        {
            _settingsSaver = new SettingsSaver(RegexConfigFileConstants.FILE_PATH,RegexConfigFileConstants.NAME_ROOTTAG);
            //_searchEpisodesRegularExpressions.Add(@"[Ss](\d{1,2})[Ee](\d{1,2})");
            //_searchEpisodesRegularExpressions.Add(@"[^a-zA-Z0-9](\d{1,2})(\d{2})[^a-zA-Z0-9]");
            LoadSettings();
        }

        private static ObservableCollection<String> _searchEpisodesRegularExpressions = new ObservableCollection<string>(); 

        public static ObservableCollection<string> EpisodeRegularExpressions
        {
            get { return _searchEpisodesRegularExpressions; }
            set { _searchEpisodesRegularExpressions = value; }
        }

        public static void SaveSettings()
        {
            _settingsSaver.CreateXmlWriter();
            _settingsSaver.WriteStringList(CollectionConverter<string>.ConvertObservableCollection(_searchEpisodesRegularExpressions), RegexConfigFileConstants.EPISODE_REGEX_LIST);
            _settingsSaver.CloseXmlWriter();
        }

        public static void LoadSettings()
        {
            EpisodeRegularExpressions = CollectionConverter<string>.ConvertList(_settingsSaver.ReadStringList(RegexConfigFileConstants.EPISODE_REGEX_LIST));
        }
    }
}
