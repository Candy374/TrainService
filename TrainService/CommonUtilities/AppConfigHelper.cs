using System.Configuration;

namespace CommonUtilities
{
    public static class AppConfigHelper
    {
        public static string LoadAppSetting(string key, string defaultValue)
        {
            var value = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrEmpty(value))
            {
                value = defaultValue;
            }
            return value;
        }
    }
}
