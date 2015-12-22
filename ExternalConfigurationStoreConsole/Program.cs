using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using ExternalConfigurationStore.AzureStorageExtensions;
using ExternalConfigurationStore.Core;
using ExternalConfigurationStore.Core.Option;
using ExternalConfigurationStore.Core.SettingProvider;
using ExternalConfigurationStore.DatabaseExtensions;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json.Linq;

namespace ExternalConfigurationStoreConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            ExternalConfigurationManager.Initialize(
                () => new CacheSettings(
                    new DbSettingsStore(
                        new SqlConnection(
                            ConfigurationManager.ConnectionStrings["ExternalConfigrationConnectionString"]
                                .ConnectionString),
                        ConfigurationManager.AppSettings.Get("AppSettingTableName")),
                    new CacheSettingsOptions {RefreshInterval = 1}));


            var setting = ExternalConfigurationManager.AppSettings.Get("key54");
            setting = ExternalConfigurationManager.AppSettings.Get("key58");
            Task.Delay(5000).Wait();
            //ExternalConfigurationManager.Initialize(
            //    () => new CacheSettings(
            //        new TableSettingsStore(
            //            ConfigurationManager.AppSettings.Get("ExternalConfigrationConnectionString"),
            //            ConfigurationManager.AppSettings.Get("AppSettingTableName")).WithKeyValue("Key", "Value"),
            //        new CacheSettingsOptions {RefreshInterval = 20*60}));

            setting = ExternalConfigurationManager.AppSettings.Get("key54");
            setting = ExternalConfigurationManager.AppSettings.Get("key58");

            ExternalConfigurationManager.Release();
        }

        private static CloudTable GetCloudTable()
        {
            var storageAccount =
                CloudStorageAccount.Parse(ConfigurationManager.AppSettings.Get("ExternalConfigrationConnectionString"));
            var tableClient = storageAccount.CreateCloudTableClient();
            var cloudTable = tableClient.GetTableReference(ConfigurationManager.AppSettings.Get("AppSettingTableName"));
            //cloudTable.CreateIfNotExists();
            return cloudTable;
        }
    }
}
