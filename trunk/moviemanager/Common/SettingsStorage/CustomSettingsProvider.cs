using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace Common.SettingsStorage
{
    public sealed class CustomSettingsProvider : SettingsProvider
    {
        private NameValueCollection settingValues = new NameValueCollection();
        private Configuration myConfig;
        private ClientSettingsSection usrSettingsSection;
        private ClientSettingsSection appSettingsSection;
        private System.Type settingsType;

        public override string ApplicationName
        {
            get
            {
                return System.Reflection.Assembly.GetCallingAssembly().GetName().Name;
            }
            set
            {

            }
        }

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            base.Initialize(this.ApplicationName, settingValues);
        }

        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection collection)
        {
            //ConfigurationManager.OpenExeConfiguration()
            string appDataPath = "moviemanager";
            string commonApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string commonAppDataPath = Path.Combine(commonApplicationData, appDataPath);

            settingsType = (Type)context["SettingsClassType"];
            string fileName = settingsType.Assembly.Location;

            myConfig = ConfigurationManager.OpenExeConfiguration(fileName);

            if (myConfig.HasFile)
            {

            }
            else
            {
            }


            // Get the collection of the section groups.
            ApplicationSettingsGroup appSettingsGroup = (ApplicationSettingsGroup)myConfig.SectionGroups["applicationSettings"];
            UserSettingsGroup usrSettingsGroup = (UserSettingsGroup)myConfig.SectionGroups["userSettings"];

            if (usrSettingsGroup == null)
            {
                UserSettingsGroup usrGrp = new UserSettingsGroup();
                usrGrp.ForceDeclaration(true);
                myConfig.SectionGroups.Add("userSettings", usrGrp);
                //
                usrSettingsSection = new ClientSettingsSection();
                usrSettingsSection.SectionInformation.AllowExeDefinition = ConfigurationAllowExeDefinition.MachineToLocalUser;
                usrSettingsSection.SectionInformation.RequirePermission = false;
                usrSettingsSection.SectionInformation.ForceDeclaration(true);
                usrSettingsSection.SectionInformation.ForceSave = true;
                usrGrp.Sections.Add(settingsType.FullName, usrSettingsSection);
            }
            else
            {
                usrSettingsSection = (ClientSettingsSection)usrSettingsGroup.Sections[settingsType.FullName];
            }

            UserSettingsGroup UserSettingsGrp = new UserSettingsGroup();
            UserSettingsGrp.ForceDeclaration(true);



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

            myConfig.SaveAs("C:\\Users\\alexander\\AppData\\Roaming\\Taxrebel\\moviemanager\\settings\\settings.config", ConfigurationSaveMode.Full);//TODO 090 continue alex
            return values;
        }

        private string getConfigValue(SettingsProperty setting)
        {
            //if (appSettingsSection != null)
            if ((setting.Attributes.ContainsKey(typeof(ApplicationScopedSettingAttribute))) &&
                (!setting.Attributes.ContainsKey(typeof(SpecialSettingAttribute))))
            {
                SettingElement appSetting = appSettingsSection.Settings.Get(setting.Name);
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
                SettingElement usrSetting = usrSettingsSection.Settings.Get(setting.Name);
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
                ConnectionStringSettings connStr = myConfig.ConnectionStrings.ConnectionStrings[settingsType.FullName + "." + setting.Name];
                if (connStr != null)
                    return connStr.ConnectionString;
                else
                    return null;
            }
        }

        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
        {

        }
    }
}
