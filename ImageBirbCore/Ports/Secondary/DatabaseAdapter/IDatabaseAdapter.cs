namespace ImageBirb.Core.Ports.Secondary.DatabaseAdapter
{
    /// <summary>
    /// This adapter is used to persist images and their metadata.
    /// </summary>
    public interface IDatabaseAdapter
    {
        /// <summary>
        /// The connection string used by the database.
        /// </summary>
        string ConnectionString { get; }

        /// <summary>
        /// Database module that handles the image data.
        /// </summary>
        IImageManagement ImageManagement { get; }

        /// <summary>
        /// Database module that handles the tag data.
        /// </summary>
        ITagManagement TagManagement { get; }
    }
}