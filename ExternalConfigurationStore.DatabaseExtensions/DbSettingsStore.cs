using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using ExternalConfigurationStore.Core.SettingsStore;

namespace ExternalConfigurationStore.DatabaseExtensions
{
    /// <summary>
    /// Retrieves the settings from a database.
    /// </summary>
    public class DbSettingsStore : SettingsStoreMapper<DbSettingsStore>, ISettingStore
    {
        private readonly IDbConnection _dbConnection;
        private readonly string _tableName;

        protected override string KeyColumnName => "[key]";

        /// <summary>
        /// Initialize a new instance of the <see cref="DbSettingsStore"/> class.
        /// </summary>
        /// <param name="dbConnection">The connection to the database.</param>
        /// <param name="tableName">The name of the table.</param>
        public DbSettingsStore(IDbConnection dbConnection, string tableName)
        {
            _dbConnection = dbConnection;
            _tableName = tableName;
        }

        /// <summary>
        /// Retrieves all the settings from the store.
        /// </summary>
        public async Task<IEnumerable<KeyValuePair<string, string>>> GetAllAsync()
        {
            return await ReadSettingsFromTableAsync();
        }

        private async Task<IEnumerable<KeyValuePair<string, string>>> ReadSettingsFromTableAsync()
        {
            using (_dbConnection)
            {
                if (_dbConnection.State != ConnectionState.Open)
                {
                    _dbConnection.Open();
                }

                return (await
                    _dbConnection.QueryAsync(
                        $"SELECT {KeyColumnName} AS [Key], {ValueColumnName} AS [Value] FROM {_tableName}"))
                    .ToDictionary(x => (string)x.Key, x => (string)x.Value);
            }
        }
    }
}
