using System;
using ExternalConfigurationStore.Core.SettingProvider;

namespace ExternalConfigurationStore.Core
{
    /// <summary>
    /// A Configuration manager that retrieves settings from the spedified store.
    /// </summary>
    public static class ExternalConfigurationManager
    {
        private static Lazy<ISettingProvider> _configuredInstance;

        /// <summary>
        /// Initialize the configuration provider.
        /// </summary>
        public static void Initialize(Func<ISettingProvider> settingProviderFunc)
        {
            _configuredInstance = new Lazy<ISettingProvider>(settingProviderFunc);
        }

        /// <summary>
        /// Gets the application settings.
        /// </summary>
        public static ISettingProvider AppSettings
        {
            get
            {
                if (_configuredInstance == null)
                {
                    throw new InvalidOperationException(
                        "The SettingProvider has not been initialized.\r\n Consider calling the ExternalConfigurationManager.Initialize(Func<IConfigurationProvider> configurationProviderFunc) before retrieving setting.");
                }

                return _configuredInstance.Value;
            }
        }

        /// <summary>
        /// Release the setting provider.
        /// </summary>
        public static void Release()
        {
            if(_configuredInstance != null && _configuredInstance.IsValueCreated)
                _configuredInstance.Value.Dispose();
        }
    }
}
