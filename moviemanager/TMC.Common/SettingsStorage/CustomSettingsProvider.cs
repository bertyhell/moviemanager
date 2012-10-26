using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace MovieManager.Common.SettingsStorage
{
    public sealed class CustomSettingsProvider : SettingsProvider
    {
        private string _appSettingsPath;
        private string _userSettingsPath;
        private bool _userSettingsFileFound;
        private bool _appSettingsFileFound;

        private NameValueCollection settingValues = new NameValueCollection();
        private Configuration _myConfig;
        private ClientSettingsSection _usrSettingsSection;
        private ClientSettingsSection _appSettingsSection;
        private System.Type _settingsType;

        public CustomSettingsProvider()
        {}

        public override string ApplicationName
        {
            get
            {
                return System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
            }
            set
            {

            }
        }

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            base.Initialize(this.ApplicationName, settingValues);
            _userSettingsPath = Path.Combine(DefaultValues.PATH_USER_APPDATA, DefaultValues.PATH_APPLICATION_SUBDIR, DefaultValues.PATH_SETTINGS_SUBDIR,
                                 this.ApplicationName + ".Settings.config");
            _appSettingsPath = Path.Combine(DefaultValues.PATH_PROGRAM_DATA, DefaultValues.PATH_APPLICATION_SUBDIR, DefaultValues.PATH_SETTINGS_SUBDIR,
                                 this.ApplicationName + ".Settings.config");

            _userSettingsFileFound = File.Exists(_userSettingsPath);
            _appSettingsFileFound = File.Exists(_appSettingsPath);

        }

        private void CreateConfigFile(string filePath)
        {
            const string XML = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<configuration>\n</configuration>";

            Directory.CreateDirectory(Path.GetDirectoryName(filePath)); //else exception when not existing

            System.IO.StreamWriter FileWriter = new StreamWriter(filePath);
            FileWriter.WriteLine(XML);
            FileWriter.Close();
        }

        private void GetConfiguration(string exeLocation)
        {
            if (!_userSettingsFileFound)CreateConfigFile(_userSettingsPath);
            if(!_appSettingsFileFound) CreateConfigFile(_appSettingsPath);

            string fileName = _settingsType.Assembly.Location;

            ExeConfigurationFileMap ExeConfigurationFileMap = new ExeConfigurationFileMap();
            ExeConfigurationFileMap.ExeConfigFilename = fileName + ".config";
            ExeConfigurationFileMap.ExeConfigFilename = "C:\\ProgramData\\TheMovieCollector\\Settings\\moviemanager.settings.config";
            //ExeConfigurationFileMap.MachineConfigFilename = "C:\\ProgramData\\TheMovieCollector\\Settings\\moviemanager.settings.config";
            ExeConfigurationFileMap.MachineConfigFilename =  fileName + ".config";
            ExeConfigurationFileMap.RoamingUserConfigFilename = _userSettingsPath;
            ExeConfigurationFileMap.LocalUserConfigFilename = _userSettingsPath;
            _myConfig = ConfigurationManager.OpenMappedExeConfiguration(ExeConfigurationFileMap,
                                                                       ConfigurationUserLevel.PerUserRoamingAndLocal);


            // Get the collection of the section groups.
            ApplicationSettingsGroup AppSettingsGroup = (ApplicationSettingsGroup)_myConfig.SectionGroups["applicationSettings"];
            UserSettingsGroup UsrSettingsGroup = (UserSettingsGroup)_myConfig.SectionGroups["userSettings"];


            if (AppSettingsGroup == null)
            {
                ApplicationSettingsGroup AppGrp = new ApplicationSettingsGroup();
                AppGrp.ForceDeclaration(true);
                _myConfig.SectionGroups.Add("applicationSettings", AppGrp);
                //
                _appSettingsSection = new ClientSettingsSection();
                _appSettingsSection.SectionInformation.RequirePermission = false;
                _appSettingsSection.SectionInformation.ForceDeclaration(true);
                _appSettingsSection.SectionInformation.ForceSave = true;
                AppGrp.Sections.Add(_settingsType.FullName, _appSettingsSection);
            }
            else
            {
                _appSettingsSection = (ClientSettingsSection)AppSettingsGroup.Sections[_settingsType.FullName];
            }

            if (UsrSettingsGroup == null)
            {
                UserSettingsGroup UsrGrp = new UserSettingsGroup();
                UsrGrp.ForceDeclaration(true);
                _myConfig.SectionGroups.Add("userSettings", UsrGrp);
                //
                _usrSettingsSection = new ClientSettingsSection();
                _usrSettingsSection.SectionInformation.AllowExeDefinition = ConfigurationAllowExeDefinition.MachineToLocalUser;
                _usrSettingsSection.SectionInformation.RequirePermission = false;
                _usrSettingsSection.SectionInformation.ForceDeclaration(true);
                _usrSettingsSection.SectionInformation.ForceSave = true;
                UsrGrp.Sections.Add(_settingsType.FullName, _usrSettingsSection);
            }
            else
            {
                _usrSettingsSection = (ClientSettingsSection)UsrSettingsGroup.Sections[_settingsType.FullName];
            }

            if (!_userSettingsFileFound ||! _appSettingsFileFound)
            {
                _userSettingsFileFound = true; // must be before force save --> otherwis infinite loop
                _appSettingsFileFound = true; // must be before force save --> otherwis infinite loop
                ForceSave();
            }

        }

        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection collection)
        {

            _settingsType = (Type)context["SettingsClassType"];

            if (_myConfig == null)
                GetConfiguration(_settingsType.Assembly.Location);

            SettingsPropertyValueCollection values = new SettingsPropertyValueCollection();

            string settingValue;
            // Iterate through the settings to be retrieved
            foreach (SettingsProperty setting in collection)
            {
                settingValue = getConfigValue(setting);
                SettingsPropertyValue value = new SettingsPropertyValue(setting);
                value.IsDirty = false;
                value.SerializedValue = settingValue;
                values.Add(value);
                // Keep AppSettings in sync
                ConfigurationManager.AppSettings.Set(setting.Name, settingValue);
            }

            return values;
        }

        private string getConfigValue(SettingsProperty setting)
        {
            //if (appSettingsSection != null)
            if ((setting.Attributes.ContainsKey(typeof(ApplicationScopedSettingAttribute))) &&
                (!setting.Attributes.ContainsKey(typeof(SpecialSettingAttribute))))
            {
                SettingElement appSetting = _appSettingsSection.Settings.Get(setting.Name);
                if (appSetting != null)
                    return appSetting.Value.ValueXml.InnerText.TrimEnd();
                else
                {
                    if (setting.DefaultValue != null)
                        return setting.DefaultValue.ToString();
                    else
                        return null;
                }
            }
            //if (usrSettingsSection != null)
            else if (setting.Attributes.ContainsKey(typeof(UserScopedSettingAttribute)))
            {
                SettingElement usrSetting = _usrSettingsSection.Settings.Get(setting.Name);
                if (usrSetting != null)
                    return usrSetting.Value.ValueXml.InnerText.TrimEnd();
                else
                {
                    if (setting.DefaultValue != null)
                        return setting.DefaultValue.ToString();
                    else
                        return null;
                }
            }
            else
            {
                ConnectionStringSettings connStr = _myConfig.ConnectionStrings.ConnectionStrings[_settingsType.FullName + "." + setting.Name];
                if (connStr != null)
                    return connStr.ConnectionString;
                else
                    return null;
            }
        }

        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
        {
            foreach (SettingsPropertyValue SettingsPropertyValue in collection)
            {
                SetConfigValue(SettingsPropertyValue);
            }
        }

        public void Save()
        {
            _myConfig.Save(ConfigurationSaveMode.Full);
            GetConfiguration(_settingsType.Assembly.Location);
        }

        public void ForceSave()
        {
            if (_myConfig.SectionGroups["userSettings"] != null)
            {
                foreach (ConfigurationSection Section in _myConfig.SectionGroups["userSettings"].Sections)
                {
                    Section.SectionInformation.ForceSave = true;
                }
            }

            _myConfig.Save(ConfigurationSaveMode.Full);
            GetConfiguration(_settingsType.Assembly.Location);

            if (_myConfig.SectionGroups["applicationSettings"] != null)
            {
                foreach (ConfigurationSection Section in _myConfig.SectionGroups["applicationSettings"].Sections)
                {
                    Section.SectionInformation.ForceSave = true;
                }
            };
            _myConfig.Save(ConfigurationSaveMode.Full);
            GetConfiguration(_settingsType.Assembly.Location);
        }



        private void SetConfigValue(SettingsPropertyValue value)
        {
            SettingsAttributeDictionary SettingAttributes = value.Property.Attributes;
            if (SettingAttributes.ContainsKey(typeof(ApplicationScopedSettingAttribute)) && (!SettingAttributes.ContainsKey(typeof(SpecialSettingAttribute))))
            {
                SettingElement AppSetting = _appSettingsSection.Settings.Get(value.Name);
                if (AppSetting == null)
                {
                    AppSetting = new SettingElement(value.Name, SettingsSerializeAs.String);
                }
                if (!AppSetting.Value.ValueXml.InnerText.Equals(value.PropertyValue.ToString()))
                {
                    AppSetting.Value.ValueXml.InnerXml = value.SerializedValue.ToString();
                    _appSettingsSection.SectionInformation.ForceSave = true;
                }
                return;
            }
            else if (value.Property.Attributes.ContainsKey(typeof(UserScopedSettingAttribute)))
            {

                SettingElement UserSetting = _usrSettingsSection.Settings.Get(value.Name);
                if (UserSetting == null)
                {
                    UserSetting = new SettingElement(value.Name, SettingsSerializeAs.String);
                }
                if (!UserSetting.Value.ValueXml.InnerText.Equals(value.PropertyValue.ToString()))
                {
                    UserSetting.Value.ValueXml.InnerXml = value.SerializedValue.ToString();
                    _usrSettingsSection.SectionInformation.ForceSave = true;
                }
                return;
            }
            else
            {
                ConnectionStringSettings ConnStr = _myConfig.ConnectionStrings.ConnectionStrings[_settingsType.FullName + "." + value.Name];
                if (ConnStr == null)
                {
                    ConnStr = new ConnectionStringSettings(_settingsType.FullName + "." + value.Name, value.PropertyValue.ToString());
                    _myConfig.ConnectionStrings.ConnectionStrings.Add(ConnStr);
                    _myConfig.ConnectionStrings.SectionInformation.ForceSave = true;
                }
                else
                {
                    string StrVal = value.PropertyValue.ToString();
                    if (!ConnStr.ConnectionString.Equals(StrVal))
                    {
                        ConnStr.ConnectionString = StrVal;
                        _myConfig.ConnectionStrings.SectionInformation.ForceSave = true;
                    }

                }
                return;
            }
        }
    }
}
