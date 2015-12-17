using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExternalConfigurationStore.Core.SettingsStore
{
    /// <summary>
    /// Provides methods to query the settings store.
    /// </summary>
    public interface ISettingStore
    {
        /// <summary>
        /// Retrieves all the settings from the store.
        /// </summary>
        Task<IEnumerable<KeyValuePair<string, string>>> GetAllAsync();
    }
}
