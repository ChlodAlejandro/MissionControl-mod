using System.Collections.Generic;

namespace MissionControl.Configuration
{
    public class ConfigKey
    {

        /// <summary>
        /// The internal name of the configuration key.
        /// </summary>
        public string Key { get; private set; }
        /// <summary>
        /// The human-readable name of the configuration key.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// The value of the configuration key.
        /// </summary>
        public object Value;
        /// <summary>
        /// The default value of the configuration key.
        /// </summary>
        public object Default { get; private set; }

        /// <summary>
        /// A configuration key object.
        /// 
        /// The value must be assigned to manually.
        /// </summary>
        /// <param name="key">The internal name of the key.</param>
        /// <param name="name">The human-readable name of the key.</param>
        /// <param name="_default">The default value of the key.</param>
        public ConfigKey(string key, string name, object _default)
        {
            Key = key;
            Name = name;
            Default = _default;

            Value = _default;
        }

    }
}
