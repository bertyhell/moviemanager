using System.Configuration;
using MovieManager.Common.SettingsStorage;

namespace MovieManager.APP.Properties
{
    
    
    // This class allows you to handle specific events on the settings class:
    //  The SettingChanging event is raised before a setting's value is changed.
    //  The PropertyChanged event is raised after a setting's value is changed.
    //  The SettingsLoaded event is raised after the setting values are loaded.
    //  The SettingsSaving event is raised before the setting values are saved.
    //[SettingsManageability(System.Configuration.SettingsManageability.Roaming)]
    [SettingsProvider(typeof(CustomSettingsProvider))]
    public sealed partial class Settings
    {
        private CustomSettingsProvider _provider = null;

        public Settings() {
            //this.Providers.Add(new CustomSettingsProvider());
            // // To add event handlers for saving and changing settings, uncomment the lines below:
            //
            // this.SettingChanging += this.SettingChangingEventHandler;
            //
            //this.SettingsSaving += this.SettingsSavingEventHandler;
            
        }
        
        private void SettingChangingEventHandler(object sender, System.Configuration.SettingChangingEventArgs e) {
            // Add code to handle the SettingChangingEvent event here.
        }
        
        //private void SettingsSavingEventHandler(object sender, System.ComponentModel.CancelEventArgs e) {
        //    if(_provider == null)
        //    {
        //        foreach (var Provider in this.Providers)
        //        {
        //            if(Provider is CustomSettingsProvider)
        //            {
        //                _provider = (CustomSettingsProvider)Provider;
        //            }
        //        }
        //    }
        //    _provider.Save();
        //}

        protected override void OnSettingChanging(object sender, SettingChangingEventArgs e)
        {
            base.OnSettingChanging(sender, e);
        }
    }
}
