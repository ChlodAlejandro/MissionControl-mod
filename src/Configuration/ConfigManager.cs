using KSP.IO;
using System;
using System.Collections.Generic;

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
        private readonly PluginConfiguration _pluginConfiguration;
        private static ConfigManager _instance;
        private readonly Dictionary<string, ConfigKey> _keys = new Dictionary<string, ConfigKey>()
        {
            { "networkPermitted", new ConfigKey("networkPermitted", "Network Permitted", false) },
            { "listenerHost", new ConfigKey("listenerHost", "Listener Host", "127.0.0.1") },
            { "listenerPort", new ConfigKey("listenerPort", "Listener Port", (ushort) 8417) }
        };

        public static ConfigManager Instance => _instance ?? (_instance = new ConfigManager());

        public ConfigManager()
        {
            _pluginConfiguration = PluginConfiguration.CreateForType<ConfigManager>();
            LoadConfiguration();
        }

        public void LoadConfiguration()
        {
            try
            {
                _pluginConfiguration.load();

                foreach (ConfigKey key in _keys.Values)
                {
                    object configValue = _pluginConfiguration[key.Key];
                    if (configValue != null)
                    {
                        key.Value = configValue;
                        // Log.i("Successfully loaded config key '" + key.Key + "': " + configValue);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.E(ex, "An exception occured!");
            }
        }

        public void SaveConfiguration()
        {
            try
            {
                foreach (string key in _keys.Keys)
                {
                    _pluginConfiguration.SetValue(key, _keys[key].Value);
                }

                _pluginConfiguration.save();
            }
            catch (Exception ex)
            {
                Log.E(ex, "An exception occured!");
            }
        }

        public void SetValue(string key, object value)
        {
            if (!_keys.ContainsKey(key))
                throw new Exception("Invalid key.");
            
            _keys[key].Value = value;
            SaveConfiguration();
        }

        public ConfigKey GetConfigKey(string key)
        {
            return _keys[key];
        }

        public T GetValue<T>(string key)
        {
            return (T) _keys[key].Value;
        }

    }
}
