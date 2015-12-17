using System;
using System.Threading;
using System.Threading.Tasks;

namespace ExternalConfigurationStore.Core.Infrastructure
{
    /// <summary>
    /// Provides methods to execute repeated action.
    /// </summary>
    internal static class Repeat
    {
        /// <summary>
        /// Repeat an action based on a time interval.
        /// </summary>
        /// <param name="pollInterval">The time interval</param>
        /// <param name="actionAsync">The async action to execute.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns></returns>
        public static Task Interval(
            TimeSpan pollInterval,
            Func<Task> actionAsync,
            CancellationToken token)
        {
            // We don't use Observable.Interval:
            // If we block, the values start bunching up behind each other.
            return Task.Run(
                () =>
                {
                    for (;;)
                    {
                        if (token.WaitCancellationRequested(pollInterval))
                            break;

                        actionAsync();
                    }
                }, token);
        }
    }

    /// <summary>
    /// Provides extensiosn methods for the <see cref="Task"/> class.
    /// </summary>
    internal static class TaskExtensions
    {
        /// <summary>
        /// Get the result of an asynchronous task synchronously...
        /// </summary>
        /// <typeparam name="T">The result type.</typeparam>
        /// <param name="task">The task.</param>
        /// <returns>The task result.</returns>
        public static T GetResultSynchronously<T>(this Task<T> task)
        {
            task.Wait();
            return task.Result;
        }
    }
}
