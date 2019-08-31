using KSP.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MissionControl.Configuration
{
    /// <summary>
    /// Mission Control's Configuration Manager
    /// 
    /// <para>This class facilitates the use of the 
    /// plugin's configuration files.</para>
    /// </summary>
    public class ConfigManager
    {
        private PluginConfiguration pluginConfiguration;
        private static ConfigManager instance;
        private readonly Dictionary<string, ConfigKey> keys = new Dictionary<string, ConfigKey>()
        {
            { "networkPermitted", new ConfigKey("networkPermitted", "Network Permitted", false) }
        };

        public static ConfigManager Instance
        {
            get
            {
                return instance ?? (instance = new ConfigManager());
            }
        }

        public ConfigManager()
        {
            pluginConfiguration = PluginConfiguration.CreateForType<ConfigManager>();
            LoadConfiguration();
        }

        public void LoadConfiguration()
        {
            try
            {
                pluginConfiguration.load();

                foreach (ConfigKey key in keys.Values)
                {
                    object configValue = pluginConfiguration[key.Key];
                    if (configValue != null)
                    {
                        key.Value = configValue;
                        // Log.i("Successfully loaded config key '" + key.Key + "': " + configValue);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log.e(ex, "An exception occured!");
            }
        }

        public void SaveConfiguration()
        {
            try
            {
                foreach (string key in keys.Keys)
                {
                    pluginConfiguration.SetValue(key, keys[key].Value);
                }

                pluginConfiguration.save();
            }
            catch (Exception ex)
            {
                // Log.e(ex, "An exception occured!");
            }
        }

        public void SetValue(string key, object value)
        {
            if (!keys.ContainsKey(key))
                throw new Exception("Invalid key.");
            
            keys[key].Value = value;
            SaveConfiguration();
        }

        public ConfigKey GetConfigKey(string key)
        {
            return keys[key];
        }

        public T GetValue<T>(string key)
        {
            return (T) keys[key].Value;
        }

    }
}
