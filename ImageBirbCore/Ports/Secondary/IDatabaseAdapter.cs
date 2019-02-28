namespace ImageBirb.Core.Ports.Secondary
{
    /// <summary>
    /// This adapter is used to persist any data in the
    /// application.
    /// Management of different data types is split into
    /// separate adapters.
    /// </summary>
    internal interface IDatabaseAdapter
    {
        /// <summary>
        /// The connection string used by the database.
        /// </summary>
        string ConnectionString { get; }
    }
}