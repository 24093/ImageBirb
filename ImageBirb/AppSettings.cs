using System.Configuration;

namespace ImageBirb
{
    internal static class AppSettings
    {
        public static string Get(string key, string defaultValue)
        {
            var appSettings = ConfigurationManager.AppSettings;
            return appSettings[key] ?? defaultValue;
        }

        public static void Set(string key, string value)
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;

            if (settings[key] == null)
            {
                settings.Add(key, value);
            }
            else
            {
                settings[key].Value = value;
            }

            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
        }
    }
}