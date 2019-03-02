using System.Collections.Generic;
using System.Threading.Tasks;
using ImageBirb.Core.BusinessObjects;

namespace ImageBirb.Core.Ports.Secondary
{
    /// <summary>
    /// Database module that handles tags.
    /// </summary>
    internal interface ITagManagementAdapter
    {
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