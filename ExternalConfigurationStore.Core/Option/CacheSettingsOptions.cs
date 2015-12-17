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
    }
}
