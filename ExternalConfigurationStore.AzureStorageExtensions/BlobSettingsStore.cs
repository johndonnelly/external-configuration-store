using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using ExternalConfigurationStore.Core.SettingsStore;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ExternalConfigurationStore.AzureStorageExtensions
{
    /// <summary>
    /// Retrieves the settings from a blob stored into an <see cref="CloudBlobContainer"/>.
    /// </summary>
    public class BlobSettingsStore : SettingsStoreMapper<BlobSettingsStore>, ISettingStore
    {
        private readonly CloudBlockBlob _configBlob;

        /// <summary>
        /// Initialize a new instance of the <see cref="BlobSettingsStore"/> class.
        /// </summary>
        /// <param name="storageAccount">The connection string to connect to the storage account.</param>
        /// <param name="configContainer">The azure storage container</param>
        /// <param name="configBlobName">The blob name.</param>
        public BlobSettingsStore(string storageAccount, string configContainer, string configBlobName)
        {
            var account = CloudStorageAccount.Parse(storageAccount);
            var client = account.CreateCloudBlobClient();
            var container = client.GetContainerReference(configContainer);

            if (!container.Exists())
                throw new InvalidOperationException($"Cannot find container with name: {configContainer}.");

            _configBlob = container.GetBlockBlobReference(configBlobName);
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
            XElement configFile;

            // Read the configuration blob and return the settings as a Dictionary.
            using (var stream = new MemoryStream())
            {
                await _configBlob.DownloadToStreamAsync(stream);

                stream.Position = 0;
                using (var reader = new StreamReader(stream))
                {
                    configFile = XElement.Parse(reader.ReadToEnd());
                }
            }

            return
                configFile.Descendants()//.Descendants("add")
                    .Select(x => new KeyValuePair<string, string>(x.Attribute(KeyColumnName).Value, x.Attribute(ValueColumnName).Value))
                    .ToList();
        }
    }
}
