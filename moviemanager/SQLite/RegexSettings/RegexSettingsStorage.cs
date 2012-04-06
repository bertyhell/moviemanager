using System;
using System.Collections.ObjectModel;
using Common;
using Common.SettingsStorage;

namespace SQLite.RegexSettings
{
    public static class RegexSettingsStorage
    {
        private static readonly SettingsSaver SETTINGS_SAVER;

        static RegexSettingsStorage()
        {
            SETTINGS_SAVER = new SettingsSaver(RegexConfigFileConstants.FILE_PATH,RegexConfigFileConstants.NAME_ROOTTAG);
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
            SETTINGS_SAVER.CreateXmlWriter();
            SETTINGS_SAVER.WriteStringList(CollectionConverter<string>.ConvertObservableCollection(_searchEpisodesRegularExpressions), RegexConfigFileConstants.EPISODE_REGEX_LIST);
            SETTINGS_SAVER.CloseXmlWriter();
        }

        public static void LoadSettings()
        {
            EpisodeRegularExpressions = CollectionConverter<string>.ConvertList(SETTINGS_SAVER.ReadStringList(RegexConfigFileConstants.EPISODE_REGEX_LIST));
        }
    }
}
