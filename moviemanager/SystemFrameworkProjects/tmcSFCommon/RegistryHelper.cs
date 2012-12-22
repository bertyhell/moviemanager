using Microsoft.Win32;

namespace Tmc.SystemFrameworks.Common
{
    public static class RegistryHelper
    {
        public static string GetInstallationPath(string programDisplayName)
        {
            string RetVal = null;
            const string REGISTRY_KEY = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            using (RegistryKey Key = Registry.LocalMachine.OpenSubKey(REGISTRY_KEY))
            {
                if (Key != null)
                    foreach (string SubkeyName in Key.GetSubKeyNames())
                    {
                        using (RegistryKey Subkey = Key.OpenSubKey(SubkeyName))
                        {
                            if (Subkey != null)
                            {
                                object Value = Subkey.GetValue("DisplayName");
                                if (Value != null && Value.ToString().Contains(programDisplayName))
                                {
                                    RetVal = Subkey.GetValue("InstallLocation").ToString();
                                }
                            }
                        }
                    }
            }
            return RetVal;
        }
    }
}
