using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace MovieManager.APP.Panels.Settings
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
            _iconPath = "/MovieManager.APP;component/Images/disk.png";

            

            LoadSettings();
        }

        private void LoadSettings()
        {
            txtVideoFileSize.Text = Properties.Settings.Default.MinimumVideoFileSize.ToString();
        }

        public override bool SaveSettings()
        {
            txtVideoFileSize.Text = Properties.Settings.Default.MinimumVideoFileSize.ToString();
            Properties.Settings.Default.MinimumVideoFileSize = Int32.Parse(txtVideoFileSize.Text);
            Properties.Settings.Default.Save();

            return true;
        }
    }
}
