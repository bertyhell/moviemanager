using System;
using System.Collections.Generic;
using System.Windows.Controls;
using MovieManager.LOG;

namespace MovieManager.APP.Panels.Settings
{
    public class SettingsPanelBase : UserControl
    {
        protected string _panelName;
        protected string _iconPath;
        protected List<SettingsPanelBase> _settingsPanels = new List<SettingsPanelBase>();

        public List<SettingsPanelBase> ChildPanels
        {
            get { return _settingsPanels; }
            set { _settingsPanels = value; }
        }

        public string PanelName
        {
            get { return _panelName; }
            set { _panelName = value; }
        }

        public string IconPath
        {
            get { return _iconPath; }
            set { _iconPath = value; }
        }

        public bool IsSelected { get; set; }
        public bool IsExpanded { get; set; }

        /// <summary>
        /// Save the settings of the current panel and its childpanels
        /// </summary>
        /// <returns>returns true if succeeded</returns>
        public bool SaveAllSettings()
        {
            bool RetVal = false;

            try
            {
                bool Succeeded = this.SaveSettings();
                foreach (SettingsPanelBase SettingsPanel in _settingsPanels)
                {
                    Succeeded &= SettingsPanel.SaveAllSettings();
                }
                RetVal = Succeeded;
            }
            catch(Exception Ex)
            {
                GlobalLogger.Instance.MovieManagerLogger.Error(GlobalLogger.FormatExceptionForLog("SettingsPanelBase", "SaveAllSettings", Ex.Message));
            }
            return RetVal;
        }

        /// <summary>
        /// Save settings of the current panel
        /// </summary>
        /// <returns></returns>
        public virtual bool SaveSettings()
        {
            return true;
        }
    }
}
