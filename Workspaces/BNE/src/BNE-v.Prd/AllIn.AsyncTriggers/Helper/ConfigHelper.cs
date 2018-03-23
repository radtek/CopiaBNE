using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace AllInTriggers.Helper
{
    public class ConfigHelper
    {
        public static string GetConfig(string configNamePath, string defaultValue)
        {
            if (ConfigurationManager.AppSettings == null || ConfigurationManager.AppSettings.Count <= 0)
                return defaultValue;

            if (ConfigurationManager.AppSettings.AllKeys.All(a => !(a ?? string.Empty).Equals(configNamePath, StringComparison.OrdinalIgnoreCase)))
                return defaultValue;

            try
            {
                var value = ConfigurationManager.AppSettings[configNamePath];
                return value;
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}
