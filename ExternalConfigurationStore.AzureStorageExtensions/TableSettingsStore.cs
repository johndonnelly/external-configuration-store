using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExternalConfigurationStore.AzureStorageExtensions.Infrastructure;
using ExternalConfigurationStore.Core.SettingsStore;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace ExternalConfigurationStore.AzureStorageExtensions
{
    /// <summary>
    /// Retrieves the settings from an <see cref="CloudTable"/>.
    /// </summary>
    public class TableSettingsStore : SettingsStoreMapper<TableSettingsStore>, ISettingStore
    {
        private readonly CloudTable _configTable;

        /// <summary>
        /// Initialize a new instance of the <see cref="TableSettingsStore"/> class.
        /// </summary>
        /// <param name="storageAccount">The connection string to connect to the storage account.</param>
        /// <param name="configTableName">The azure storage table name.</param>
        public TableSettingsStore(string storageAccount, string configTableName)
        {
            var account = CloudStorageAccount.Parse(storageAccount);
            _configTable = account.CreateCloudTableClient().GetTableReference(configTableName);
        }

        /// <summary>
        /// Retrieves all the settings from the store.
        /// </summary>
        public async Task<IEnumerable<KeyValuePair<string, string>>> GetAllAsync()
        {
            return await ReadSettingsFromStorageAsync();
        }

        private async Task<IEnumerable<KeyValuePair<string, string>>> ReadSettingsFromStorageAsync()
        {
            var settings = await _configTable.ExecuteQueryAsync(new TableQuery());
            return settings.Select(s =>
                new KeyValuePair<string, string>(
                    s.Properties[KeyColumnName].StringValue,
                    s.Properties[ValueColumnName].StringValue))
                .ToList();
        }
    }
}
