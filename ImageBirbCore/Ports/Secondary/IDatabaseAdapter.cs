using ImageBirb.Core.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImageBirb.Core.Ports.Secondary
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
        /// Creates an image id for a new image.
        /// </summary>
        Task<string> CreateImageId();

        /// <summary>
        /// Adds an image to the database.
        /// </summary>
        /// <param name="image">Image to be added.</param>
        Task AddImage(Image image);

        /// <summary>
        /// Removes an image from the database.
        /// </summary>
        /// <param name="imageId">ID of the image to be removed.</param>
        Task RemoveImage(string imageId);

        /// <summary>
        /// Reads all thumbnails that contain at least one tag from the
        /// supplied list.
        /// </summary>
        /// <param name="tagNames">List of tags, leave empty or null to retrieve all thumbnails.</param>
        /// <returns>List of thumbnails matching the tags.</returns>
        Task<IList<Image>> GetThumbnails(IList<string> tagNames);

        /// <summary>
        /// Reads an image from the database.
        /// </summary>
        /// <param name="imageId">ID of the image.</param>
        /// <returns>Image object containing image and metadata.</returns>
        Task<Image> GetImage(string imageId);
        
        /// <summary>
        /// Add a tag to an image.
        /// </summary>
        /// <param name="imageId">ID of the image.</param>
        /// <param name="tagName">Name of the tag.</param>
        Task AddTag(string imageId, string tagName);

        /// <summary>
        /// Remove a tag from an image.
        /// </summary>
        /// <param name="imageId">ID of the image.</param>
        /// <param name="tagName">Name of the tag.</param>
        Task RemoveTag(string imageId, string tagName);

        /// <summary>
        /// Get all tags for all images. The list includes
        /// the count of each tag.
        /// </summary>
        /// <returns>List of tags.</returns>
        Task<IList<Tag>> GetTags();
    }
}