using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using ExternalConfigurationStore.Core.Infrastructure;
using ExternalConfigurationStore.Core.Option;
using ExternalConfigurationStore.Core.SettingsStore;

namespace ExternalConfigurationStore.Core.SettingProvider
{
    /// <summary>
    /// Store the settings in a static cache and refresh the settings based on an interval.
    /// </summary>
    public sealed class CacheSettings : ISettingProvider
    {
        private readonly ISettingStore _settingStore;
        private readonly ConcurrentDictionary<string, string> _settings;
        private readonly ISubject<KeyValuePair<string, string>> _changed = new Subject<KeyValuePair<string, string>>();
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly Task _updateTask;
        private bool _disposed;

        /// <summary>
        /// Gets the provider for push-based notification.
        /// </summary>
        public IObservable<KeyValuePair<string, string>> Changed => _changed.AsObservable();

        /// <summary>
        /// Gets a setting with the given name.
        /// </summary>
        /// <param name="name">The setting name.</param>
        /// <returns>The setting value or null if not found.</returns>
        public string this[string name] => InternalGet(name);

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheSettings"/> class.
        /// </summary>
        /// <param name="settingStore">The setting store.</param>
        /// <param name="cacheSettingsOptions">The options to configure the <see cref="CacheSettings"/> provider.</param>
        public CacheSettings(ISettingStore settingStore, CacheSettingsOptions cacheSettingsOptions)
        {
            if (settingStore == null)
                throw new ArgumentNullException(nameof(settingStore));

            if (cacheSettingsOptions.RefreshInterval < 0)
                throw new IndexOutOfRangeException("refreshInterval cannot be negative.");
            
            // Get the settings from the store.
            _settingStore = settingStore;
            _settings = new ConcurrentDictionary<string, string>(_settingStore.GetAllAsync().GetResultSynchronously());

            // If zero, data will be persistent.
            if (cacheSettingsOptions.RefreshInterval == 0) return;

            // Initialize the repeated task.
            _cancellationTokenSource = new CancellationTokenSource();
            _updateTask = Repeat.Interval(
                TimeSpan.FromSeconds(cacheSettingsOptions.RefreshInterval),
                UpdateSettingsAsync, _cancellationTokenSource.Token);
        }

        /// <summary>
        /// Gets a setting with the given name.
        /// </summary>
        /// <param name="name">The setting name.</param>
        /// <returns>The setting value or null if not found.</returns>
        public string Get(string name)
        {
            return InternalGet(name);
        }

        /// <summary>
        /// Cancel the update task and wait for the task to complete.
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;

            // Cancel the task and wait for the task to complete.
            _cancellationTokenSource?.Cancel();
            _updateTask?.Wait();
            _cancellationTokenSource?.Dispose();

            _disposed = true;
            GC.SuppressFinalize((object)this);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the appsettings.
        /// </summary>
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _settings.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the appsettings.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private string InternalGet(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            string value;
            return _settings.TryGetValue(name, out value) ? value : null;
        }

        private async Task UpdateSettingsAsync()
        {
            // Get the latest settings from the settings store.
            var latestSettings = await _settingStore.GetAllAsync();

            // Publish changes.
            var keyValuePairs = latestSettings as List<KeyValuePair<string, string>> ?? latestSettings.ToList();
            foreach (var setting in keyValuePairs)
            {
                _settings[setting.Key] = setting.Value;
            }

            // Notify settings changed
            foreach (var kv in keyValuePairs.Except(_settings))
            {
                _changed.OnNext(kv);
            }
        }
    }
}
