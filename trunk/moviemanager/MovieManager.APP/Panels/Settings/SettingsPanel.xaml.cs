using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System;
using System.Windows.Controls.Primitives;

namespace MovieManager.APP.Panels.Settings
{
    /// <summary>
    /// Interaction logic for SettingsPanel.xaml
    /// </summary>
    public partial class SettingsPanel
    {
        public SettingsPanel()
        {
            InitializeComponent();
            parameteredStringBuilder1.Parameters = new List<FrameworkElement> { new TextBox { Text = "Custom" }, new Button { Content = "Videoname" }, new Button { Content = "Extension" }, new Button { Content = "Season" }, new Button { Content = "EpisodeNumber" } };
            parameteredStringBuilder1.ParameteredString = "{{Extension}}Custom{{Videoname}}";
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Console.Write(parameteredStringBuilder1.ParameteredString);
        }

    }
}
