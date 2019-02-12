using System.Collections.Generic;
using System.Threading.Tasks;
using ImageBirb.Core.Common;

namespace ImageBirb.Core.Ports.Secondary
{
    /// <summary>
    /// Database module that handles images.
    /// </summary>
    public interface IImageManagementAdapter
    {
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
    }
}