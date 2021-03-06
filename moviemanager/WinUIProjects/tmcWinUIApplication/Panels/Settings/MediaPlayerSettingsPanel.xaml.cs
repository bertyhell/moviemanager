﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MovieManager.APP.Panels.Settings
{
    /// <summary>
    /// Interaction logic for MediaPlayerSettingsPanel.xaml
    /// </summary>
    public partial class MediaPlayerSettingsPanel : SettingsPanelBase
    {
        private MediaPlayerSettingsViewModel _model;
        public MediaPlayerSettingsPanel()
        {
            InitializeComponent();

            //initialize base class variables;
            _panelName = "Media player";
            _iconPath = "/MovieManager;component/Images/MediaPlayer_32.png";
            _model = new MediaPlayerSettingsViewModel
                                                     {
                                                         PlayOnDoubleCLick = APP.Properties.Settings.Default.MediaPlayerPlayOnDoubleClick
                                                     };
            this.DataContext = _model;
            

            _settingsPanels.Add(new InternalMediaPlayerSettingsPanel());
        }


        public override bool SaveSettings()
        {
            APP.Properties.Settings.Default.MediaPlayerPlayOnDoubleClick = _model.PlayOnDoubleCLick;
            return true;
        }
    }
}
