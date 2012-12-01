using System;
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
    /// Interaction logic for InternalMediaPlayerSettingsPanel.xaml
    /// </summary>
    public partial class InternalMediaPlayerSettingsPanel : SettingsPanelBase
    {

        public InternalMediaPlayerSettingsPanel()
        {
            InitializeComponent();

            //initialize base class variables;
            _panelName = "MMPlayer";
            _iconPath = "/MovieManager;component/Images/picto.png";
        }

        public override bool SaveSettings()
        {
            return true;
        }
    }
}
