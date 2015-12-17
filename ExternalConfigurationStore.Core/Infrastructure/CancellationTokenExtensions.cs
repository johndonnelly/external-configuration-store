using System;
using System.Threading;

namespace ExternalConfigurationStore.Core.Infrastructure
{
    /// <summary>
    /// Provides extension methods for the <see cref="CancellationToken"/> class.
    /// </summary>
    internal static class CancellationTokenExtensions
    {
        /// <summary>
        /// Wait until the token is cancelled or timed out.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        /// <param name="timeout">The time to wait.</param>
        public static bool WaitCancellationRequested(
            this CancellationToken token,
            TimeSpan timeout)
        {
            return token.WaitHandle.WaitOne(timeout);
        }
    }
}