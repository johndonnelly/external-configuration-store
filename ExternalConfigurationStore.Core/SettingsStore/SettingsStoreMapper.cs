namespace ExternalConfigurationStore.Core.SettingsStore
{
    /// <summary>
    /// Base class to map settings key/value column in the store.
    /// </summary>
    /// <typeparam name="TDerived">The type of the derived class.</typeparam>
    public abstract class SettingsStoreMapper<TDerived> where TDerived : SettingsStoreMapper<TDerived>
    {
        /// <summary>
        /// Gets the key column name.
        /// </summary>
        protected virtual string KeyColumnName { get; private set; } = "key";

        /// <summary>
        /// Gets the value column name.
        /// </summary>
        protected virtual string ValueColumnName { get; private set; } = "value";

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsStoreMapper{T}"/> class.
        /// </summary>
        protected SettingsStoreMapper()
        {
        }

        /// <summary>
        /// Specifies the mapping for the key and value columns in the store.
        /// </summary>
        /// <param name="keyColumnName">The key column name.</param>
        /// <param name="valueColumnName">The value column name.</param>
        /// <returns></returns>
        public TDerived WithKeyValue(string keyColumnName, string valueColumnName)
        {
            KeyColumnName = keyColumnName;
            ValueColumnName = valueColumnName;
            return (TDerived)this;
        }
    }
}
