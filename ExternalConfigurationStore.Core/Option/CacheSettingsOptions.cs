using System;
using ExternalConfigurationStore.Core.SettingProvider;

namespace ExternalConfigurationStore.Core.Option
{
    /// <summary>
    /// Represents the options to configure the <see cref="CacheSettings"/> provider.
    /// </summary>
    public class CacheSettingsOptions
    {
        /// <summary>
        /// Gets the refresh interval.
        /// </summary>
        public int RefreshInterval { get; set; }

        /// <summary>
        /// Gets a fallback function executed if a setting is not found.
        /// </summary>
        public Func<string, string> FallBackFunc { get; set; }
    }
}
