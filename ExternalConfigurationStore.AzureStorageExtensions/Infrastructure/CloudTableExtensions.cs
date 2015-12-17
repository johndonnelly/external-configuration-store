using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace ExternalConfigurationStore.AzureStorageExtensions.Infrastructure
{
    /// <summary>
    /// This class contains extensions methods for the <see cref="CloudTable"/> class.
    /// </summary>
    internal static class CloudTableExtensions
    {
        /// <summary>
        /// Execute the <paramref name="query"/> asynchronously.
        /// </summary>
        /// <param name="table">The <see cref="CloudTable"/> containing the data.</param>
        /// <param name="query">The query to execute.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A collection of <see cref="DynamicTableEntity"/>.</returns>
        public static async Task<IList<DynamicTableEntity>> ExecuteQueryAsync(this CloudTable table, TableQuery query, CancellationToken cancellationToken = default(CancellationToken))
        {
            var items = new List<DynamicTableEntity>();
            TableContinuationToken token = null;
            do
            {
                var seg = await table.ExecuteQuerySegmentedAsync(query, token, cancellationToken);
                token = seg.ContinuationToken;
                items.AddRange(seg);

            } while (token != null && !cancellationToken.IsCancellationRequested && (query.TakeCount == null || items.Count < query.TakeCount.Value));

            return items;
        }
    }
}
