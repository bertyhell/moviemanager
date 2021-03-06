﻿using System;
using System.Globalization;

namespace Tmc.WinUI.Application.Panels.Settings
{
    /// <summary>
    /// Interaction logic for FileRenameSettingsPanelBase.xaml
    /// </summary>
    public partial class VideoSearchPanel
    {


        public VideoSearchPanel()
        {
            InitializeComponent();

            //initialize base class variables;
            _panelName = "Video file search";
            _iconPath = "/Tmc.WinUI.Application;component/Images/search.png";

            LoadSettings();
        }

        private void LoadSettings()
        {
            long Bytes = Properties.Settings.Default.MinimumVideoFileSize;
            int SelectedMagnitudeIndex = 0;
            while(Bytes / 1000 >= 1)
            {
                Bytes /= 1000;
                SelectedMagnitudeIndex++;
            }
            cbbMagnitude.SelectedIndex = SelectedMagnitudeIndex;
            txtVideoFileSize.Text = Bytes.ToString(CultureInfo.InvariantCulture);
        }

        public override bool SaveSettings()
        {
            long Size;
            if (cbbMagnitude.SelectedIndex*3 + txtVideoFileSize.Text.Length > 13)
            {
                Size = 999999999999;
            }else
            {
                Size = Int64.Parse(txtVideoFileSize.Text) * (long)Math.Pow(1000, cbbMagnitude.SelectedIndex);
            }
            Properties.Settings.Default.MinimumVideoFileSize = Size;

            return true;
        }
    }
}
