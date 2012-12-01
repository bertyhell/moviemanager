using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace MovieManager.Common.SettingsStorage
{
    public sealed class CustomSettingsProvider : SettingsProvider
    {
        private string _appSettingsPath;
        private string _userSettingsPath;
        private bool _userSettingsFileFound;
        private bool _appSettingsFileFound;

        private NameValueCollection settingValues = new NameValueCollection();
        private Configuration _exeConfig;
        private Configuration _userConfig;
        private ClientSettingsSection _usrSettingsSection;
        private ClientSettingsSection _appSettingsSection;
        private System.Type _settingsType;

        public CustomSettingsProvider()
        { }

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
                                 this.ApplicationName + ".exe.config");

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
            if (!_userSettingsFileFound) CreateConfigFile(_userSettingsPath);
            if (!_appSettingsFileFound) CreateConfigFile(_appSettingsPath);

            string fileName = _settingsType.Assembly.Location;


            //
            // Get Application specific settings
            //

            ExeConfigurationFileMap ExeConfigurationFileMap = new ExeConfigurationFileMap();
            ExeConfigurationFileMap.MachineConfigFilename = fileName + ".config";
            ExeConfigurationFileMap.ExeConfigFilename = "C:\\ProgramData\\TheMovieCollector\\Settings\\moviemanager.exe.config";
            _exeConfig = ConfigurationManager.OpenMappedExeConfiguration(ExeConfigurationFileMap, ConfigurationUserLevel.None);

            ApplicationSettingsGroup AppSettingsGroup = (ApplicationSettingsGroup)_exeConfig.SectionGroups["applicationSettings"];

            if (AppSettingsGroup == null)
            {
                ApplicationSettingsGroup AppGrp = new ApplicationSettingsGroup();
                AppGrp.ForceDeclaration(true);
                _exeConfig.SectionGroups.Add("applicationSettings", AppGrp);
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

            //
            //  Get user specific user settings
            //

            ExeConfigurationFileMap UserSettingsFileMap = new ExeConfigurationFileMap();
            UserSettingsFileMap.MachineConfigFilename = fileName + ".config";
            UserSettingsFileMap.ExeConfigFilename = "C:\\ProgramData\\TheMovieCollector\\Settings\\moviemanager.exe.config";
            UserSettingsFileMap.RoamingUserConfigFilename = _userSettingsPath;
            UserSettingsFileMap.LocalUserConfigFilename = _userSettingsPath;
            _userConfig = ConfigurationManager.OpenMappedExeConfiguration(UserSettingsFileMap, ConfigurationUserLevel.PerUserRoamingAndLocal);

            UserSettingsGroup UsrSettingsGroup = (UserSettingsGroup)_userConfig.SectionGroups["userSettings"];
            if (UsrSettingsGroup == null)
            {
                UserSettingsGroup UsrGrp = new UserSettingsGroup();
                UsrGrp.ForceDeclaration(true);
                _userConfig.SectionGroups.Add("userSettings", UsrGrp);
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
        }
        
        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection collection)
        {

            _settingsType = (Type)context["SettingsClassType"];

            if (_exeConfig == null)
                GetConfiguration(_settingsType.Assembly.Location);

            SettingsPropertyValueCollection Values = new SettingsPropertyValueCollection();

            // Iterate through the settings to be retrieved
            foreach (SettingsProperty Setting in collection)
            {
                SettingsPropertyValue Value;
                GetConfigValue(Setting, out Value);

                Value.IsDirty = false;
                Values.Add(Value);
                // Keep AppSettings in sync
                ConfigurationManager.AppSettings.Set(Setting.Name, (string)Value.SerializedValue);
            }

            return Values;
        }

        private void GetConfigValue(SettingsProperty setting, out SettingsPropertyValue settingsPropertyValue)
        {
            settingsPropertyValue = new SettingsPropertyValue(setting);

            //if (appSettingsSection != null)
            if ((setting.Attributes.ContainsKey(typeof(ApplicationScopedSettingAttribute))) &&
                (!setting.Attributes.ContainsKey(typeof(SpecialSettingAttribute))))
            {
                SettingElement AppSetting = _appSettingsSection.Settings.Get(setting.Name);
                if (AppSetting == null)
                {
                    AppSetting = new SettingElement(setting.Name, SettingsSerializeAs.String);
                    _appSettingsSection.Settings.Add(AppSetting);
                }
                GetValue(AppSetting, setting, settingsPropertyValue);
            }
            //if (usrSettingsSection != null)
            else if (setting.Attributes.ContainsKey(typeof(UserScopedSettingAttribute)))
            {
                SettingElement UsrSetting = _usrSettingsSection.Settings.Get(setting.Name);
                if (UsrSetting == null)
                {
                    UsrSetting = new SettingElement(setting.Name, SettingsSerializeAs.String);
                    //UsrSetting.CurrentConfiguration = _usrSettingsSection.CurrentConfiguration;
                    if (UsrSetting.Value.ValueXml == null)
                    {
                        XmlDocument XmlDocument = new XmlDocument();
                        UsrSetting.Value.ValueXml = XmlDocument.CreateNode(XmlNodeType.Element, "value", String.Empty);
                    }
                    _usrSettingsSection.Settings.Add(UsrSetting);
                    GetDefaultValue(setting, settingsPropertyValue);
                }
                else
                {
                    GetValue(UsrSetting, setting, settingsPropertyValue);
                }
            }
            else
            {
                ConnectionStringSettings ConnStr = _exeConfig.ConnectionStrings.ConnectionStrings[_settingsType.FullName + "." + setting.Name];
                if (ConnStr != null)
                    settingsPropertyValue.SerializedValue = ConnStr.ConnectionString;
                else
                    settingsPropertyValue.SerializedValue = null;
            }
        }

        private void GetValue(SettingElement settingElement, SettingsProperty setting, SettingsPropertyValue propertyValue)
        {
            object RetVal = null;
            XmlSerializer Serializer = new XmlSerializer(setting.PropertyType);
            if (setting.PropertyType != typeof(string) && !setting.PropertyType.IsValueType && !setting.PropertyType.IsEnum)
            {
                string Value = settingElement.Value.ValueXml.InnerXml.Trim();
                if (!string.IsNullOrEmpty(Value))
                    propertyValue.PropertyValue = Serializer.Deserialize(new StringReader(Value));
                else propertyValue.PropertyValue = null;
            }
            else
            {
                string Value = settingElement.Value.ValueXml.InnerText.Trim();
                propertyValue.SerializedValue = Value.Trim();
            }
        }

        private void GetDefaultValue(SettingsProperty setting, SettingsPropertyValue propertyValue)
        {
            if (setting.DefaultValue != null)
                propertyValue.PropertyValue = setting.DefaultValue;
            else if (!setting.GetType().IsPrimitive)
                propertyValue.PropertyValue = Activator.CreateInstance(setting.PropertyType);
            else
                propertyValue.PropertyValue = null;
        }

        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
        {
            foreach (SettingsPropertyValue SettingsPropertyValue in collection)
            {
                SetConfigValue(SettingsPropertyValue);
            }
            Save();
        }

        public void Save()
        {
            _userConfig.Save(ConfigurationSaveMode.Full);
            _exeConfig.Save(ConfigurationSaveMode.Full);
            GetConfiguration(_settingsType.Assembly.Location);
        }
        
        private void SetConfigValue(SettingsPropertyValue value)
        {
            SettingsAttributeDictionary SettingAttributes = value.Property.Attributes;
            if (SettingAttributes.ContainsKey(typeof(ApplicationScopedSettingAttribute)) && (!SettingAttributes.ContainsKey(typeof(SpecialSettingAttribute))))
            {
                SettingElement AppSetting = _appSettingsSection.Settings.Get(value.Name);
                SetValue(AppSetting, value);
                _appSettingsSection.SectionInformation.ForceSave = true;
            }
            else if (value.Property.Attributes.ContainsKey(typeof(UserScopedSettingAttribute)))
            {

                SettingElement UserSetting = _usrSettingsSection.Settings.Get(value.Name);
                SetValue(UserSetting, value);
                _usrSettingsSection.SectionInformation.ForceSave = true;
            }
            else
            {
                ConnectionStringSettings ConnStr = _exeConfig.ConnectionStrings.ConnectionStrings[_settingsType.FullName + "." + value.Name];
                if (ConnStr == null)
                {
                    ConnStr = new ConnectionStringSettings(_settingsType.FullName + "." + value.Name, value.PropertyValue.ToString());
                    _exeConfig.ConnectionStrings.ConnectionStrings.Add(ConnStr);
                    _exeConfig.ConnectionStrings.SectionInformation.ForceSave = true;
                }
                else
                {
                    string StrVal = value.PropertyValue.ToString();
                    if (!ConnStr.ConnectionString.Equals(StrVal))
                    {
                        ConnStr.ConnectionString = StrVal;
                        _exeConfig.ConnectionStrings.SectionInformation.ForceSave = true;
                    }

                }
                return;
            }
        }

        private void SetValue(SettingElement settingElement, SettingsPropertyValue value)
        {
            if (settingElement == null)
            {
                settingElement = new SettingElement(value.Name, SettingsSerializeAs.String);
            }

            Object PropValue = value.PropertyValue;
            if (PropValue != null)
            {
                XmlSerializer Serializer = new XmlSerializer(PropValue.GetType());
                String SerializedValue = null;
                using (StringWriter StringWriter = new StringWriter())
                {
                    XmlWriterSettings Settings = new XmlWriterSettings
                                                     {
                                                         OmitXmlDeclaration = true,
                                                         Indent = false,
                                                         NewLineHandling = NewLineHandling.None
                                                     };
                    using (XmlWriter XmlWriter = XmlTextWriter.Create(StringWriter, Settings))
                    {
                        Serializer.Serialize(XmlWriter, PropValue);
                        SerializedValue = StringWriter.ToString();
                    }
                }
                if (!settingElement.Value.ValueXml.InnerText.Equals(SerializedValue))
                {
                    settingElement.Value.ValueXml.InnerXml = SerializedValue;
                }
            }
        }
    }
}
