﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

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
            catch
            {
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