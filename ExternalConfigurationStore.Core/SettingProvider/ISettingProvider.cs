using System;
using System.Collections.Generic;

namespace ExternalConfigurationStore.Core.SettingProvider
{
    /// <summary>
    /// Provides methods to retrieve settings and being notify of any changes.
    /// </summary>
    public interface ISettingProvider :IEnumerable<KeyValuePair<string, string>>, IDisposable
    {
        /// <summary>
        /// Gets a setting with the given name.
        /// </summary>
        /// <param name="name">The setting name.</param>
        /// <returns>The setting value or null if not found.</returns>
        string this[string name] { get; }

        /// <summary>
        /// Gets a setting with the given name.
        /// </summary>
        /// <param name="name">The setting name.</param>
        /// <returns>The setting value or null if not found.</returns>
        string Get(string name);

        /// <summary>
        /// Gets the provider for push-based notification.
        /// </summary>
        IObservable<KeyValuePair<string, string>> Changed { get; }
    }
}
