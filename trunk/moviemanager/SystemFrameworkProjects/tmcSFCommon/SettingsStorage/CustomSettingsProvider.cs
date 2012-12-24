using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Tmc.SystemFrameworks.Common.SettingsStorage
{
    public sealed class CustomSettingsProvider : SettingsProvider
    {
        /*Paths to the different files*/
        private string _appSettingsPath;
        private string _userSettingsPath;
        private string _machineSettingsPath;

        /*
         * The configurationt is loaded two times, one time at application level and one time at user level
         * This is necessary because otherwise we can not save the application scoped settings
         */
        private Configuration _exeConfig;
        private Configuration _userConfig;
        private ClientSettingsSection _usrSettingsSection;
        private ClientSettingsSection _appSettingsSection;
        private string _fullSettingsName; // for example: MovieManager.APP.Properties.Settings

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

        /// <summary>
        /// Initializes the class
        /// </summary>
        public override void Initialize(string name, NameValueCollection config)
        {
            //call base method
            NameValueCollection SettingValues = new NameValueCollection();
            base.Initialize(ApplicationName, SettingValues);

            //Set new setting file paths in appdata and programdata
            _userSettingsPath = Path.Combine(DefaultValues.PATH_USER_APPDATA, DefaultValues.PATH_APPLICATION_SUBDIR, DefaultValues.PATH_SETTINGS_SUBDIR,
                                 ApplicationName + ".Settings.config");
            _appSettingsPath = Path.Combine(DefaultValues.PATH_PROGRAM_DATA, DefaultValues.PATH_APPLICATION_SUBDIR, DefaultValues.PATH_SETTINGS_SUBDIR,
                                 ApplicationName + ".exe.config");

        }

        /// <summary>
        /// Loads the complete configuration from the specified files
        /// </summary>
        private void GetConfiguration()
        {
            //
            // Get Application specific settings --> load application settings section
            //

            ExeConfigurationFileMap ExeConfigurationFileMap = new ExeConfigurationFileMap
                {
                    MachineConfigFilename = _machineSettingsPath,
                    ExeConfigFilename = _appSettingsPath
                };
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
                AppGrp.Sections.Add(_fullSettingsName, _appSettingsSection);
            }
            else
            {
                _appSettingsSection = (ClientSettingsSection)AppSettingsGroup.Sections[_fullSettingsName];
            }

            //
            //  Get user specific user settings --> load user settings section
            //

            ExeConfigurationFileMap UserSettingsFileMap = new ExeConfigurationFileMap
                {
                    MachineConfigFilename = _machineSettingsPath,
                    ExeConfigFilename = _appSettingsPath,
                    RoamingUserConfigFilename = _userSettingsPath,
                    LocalUserConfigFilename = _userSettingsPath
                };
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
                UsrGrp.Sections.Add(_fullSettingsName, _usrSettingsSection);
            }
            else
            {
                _usrSettingsSection = (ClientSettingsSection)UsrSettingsGroup.Sections[_fullSettingsName];
            }
        }

        /// <summary>
        /// Get the values for all settings
        /// </summary>
        /// <returns>The collection of Settings</returns>
        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection collection)
        {
            //Get machine config file location and full settings name --> e.g. MovieManager.APP.Properties.Settings
            Type SettingsType = (Type)context["SettingsClassType"];
            if (string.IsNullOrWhiteSpace(_machineSettingsPath))
                _machineSettingsPath = SettingsType.Assembly.Location + ".config";
            if (string.IsNullOrWhiteSpace(_fullSettingsName))
                _fullSettingsName = SettingsType.FullName;

            //Get configuration from files
            if (_exeConfig == null)
                GetConfiguration();

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

        /// <summary>
        /// Gets the value for a single setting
        /// </summary>
        private void GetConfigValue(SettingsProperty setting, out SettingsPropertyValue settingsPropertyValue)
        {
            settingsPropertyValue = new SettingsPropertyValue(setting);

            //application settings
            if ((setting.Attributes.ContainsKey(typeof(ApplicationScopedSettingAttribute))) &&
                (!setting.Attributes.ContainsKey(typeof(SpecialSettingAttribute))))
            {
                SettingElement AppSetting = _appSettingsSection.Settings.Get(setting.Name);
                if (AppSetting == null)
                {
                    AppSetting = new SettingElement(setting.Name, SettingsSerializeAs.String);
                    if (AppSetting.Value.ValueXml == null)
                    {
                        XmlDocument XmlDocument = new XmlDocument();
                        AppSetting.Value.ValueXml = XmlDocument.CreateNode(XmlNodeType.Element, "value", String.Empty);
                    }
                    _appSettingsSection.Settings.Add(AppSetting);
                    GetDefaultValue(setting, settingsPropertyValue);
                }
                GetValue(AppSetting, setting, settingsPropertyValue);
            }
            //user settings
            else if (setting.Attributes.ContainsKey(typeof(UserScopedSettingAttribute)))
            {
                SettingElement UsrSetting = _usrSettingsSection.Settings.Get(setting.Name);
                if (UsrSetting == null)
                {
                    UsrSetting = new SettingElement(setting.Name, SettingsSerializeAs.String);
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
            //connection strings
            else
            {
                ConnectionStringSettings ConnStr = _exeConfig.ConnectionStrings.ConnectionStrings[_fullSettingsName + "." + setting.Name];
                settingsPropertyValue.SerializedValue = ( ConnStr != null ? ConnStr.ConnectionString : null );
            }
        }

        /// <summary>
        /// Gets a value for a settings element
        /// </summary>
        private void GetValue(SettingElement settingElement, SettingsProperty setting, SettingsPropertyValue propertyValue)
        {
            try
            {
                XmlSerializer Serializer = new XmlSerializer(setting.PropertyType);
                if (setting.PropertyType != typeof (string) && !setting.PropertyType.IsValueType &&
                    !setting.PropertyType.IsEnum)
                {
                    //Deserialize value
                    string Value = settingElement.Value.ValueXml.InnerXml.Trim();
                    propertyValue.PropertyValue = !string.IsNullOrEmpty(Value)
                                                      ? Serializer.Deserialize(new StringReader(Value))
                                                      : null;
                }
                else
                {
                    //Simply set text
                    string Value = settingElement.Value.ValueXml.InnerText.Trim();
                    propertyValue.SerializedValue = Value.Trim();
                }
            }
            catch
            {
                GetDefaultValue(setting, propertyValue);
            }
        }

        /// <summary>
        /// Gets a default value for a setting
        /// </summary>
        private void GetDefaultValue(SettingsProperty setting, SettingsPropertyValue propertyValue)
        {
            if (setting.DefaultValue != null)
                propertyValue.PropertyValue = setting.DefaultValue;
            else if (!setting.GetType().IsPrimitive)
                propertyValue.PropertyValue = Activator.CreateInstance(setting.PropertyType);
            else
                propertyValue.PropertyValue = null;
        }

        /// <summary>
        /// Sets the values of all settings
        /// </summary>
        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
        {
            foreach (SettingsPropertyValue SettingsPropertyValue in collection)
            {
                SetConfigValue(SettingsPropertyValue);
            }
            Save();
        }

        /// <summary>
        /// Sets a value for a specific settings
        /// </summary>
        private void SetConfigValue(SettingsPropertyValue value)
        {
            SettingsAttributeDictionary SettingAttributes = value.Property.Attributes;
            // application setting
            if (SettingAttributes.ContainsKey(typeof(ApplicationScopedSettingAttribute)) && (!SettingAttributes.ContainsKey(typeof(SpecialSettingAttribute))))
            {
                SettingElement AppSetting = _appSettingsSection.Settings.Get(value.Name);
                SetValue(AppSetting, value);
                _appSettingsSection.SectionInformation.ForceSave = true;
            }
            //user setting
            else if (value.Property.Attributes.ContainsKey(typeof(UserScopedSettingAttribute)))
            {

                SettingElement UserSetting = _usrSettingsSection.Settings.Get(value.Name);
                SetValue(UserSetting, value);
                _usrSettingsSection.SectionInformation.ForceSave = true;
            }
            //connectionstring
            else
            {
                ConnectionStringSettings ConnStr = _exeConfig.ConnectionStrings.ConnectionStrings[_fullSettingsName + "." + value.Name];
                if (ConnStr == null)
                {
                    ConnStr = new ConnectionStringSettings(_fullSettingsName + "." + value.Name, value.PropertyValue.ToString());
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
            }
        }

        /// <summary>
        /// serializes a value and sets the value
        /// </summary>
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
                String SerializedValue;
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

        /// <summary>
        /// Saves the configuration to the specified files
        /// </summary>
        public void Save()
        {
            _userConfig.Save(ConfigurationSaveMode.Full);
            _exeConfig.Save(ConfigurationSaveMode.Full);
        }
    }
}
