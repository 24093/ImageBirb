using ImageBirb.Core.Common;
using ImageBirb.Core.Workflows.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImageBirb.Core.Ports.Primary
{
    /// <summary>
    /// This adapter hides the actual workflows from the user and
    /// only exposes the results.
    /// </summary>
    public interface IWorkflowAdapter
    {
        /// <summary>
        /// Event raised if a workflow's progress was changed.
        /// </summary>
        event EventHandler<ProgressChangedEventArgs> ProgressChanged;

        /// <summary>
        /// Add images from files on the local file system.
        /// </summary>
        /// <param name="filenames">Full local file names.</param>
        /// <returns>Standard WorkflowResult.</returns>
        Task<WorkflowResult> AddImages(IList<string> filenames);

        /// <summary>
        /// Add images from files on the local file system.
        /// </summary>
        /// <param name="directory">Directory to scan for files.</param>
        /// <returns>Standard WorkflowResult.</returns>
        Task<WorkflowResult> AddImages(string directory);

        /// <summary>
        /// Removes an image from the catalog.
        /// </summary>
        /// <param name="imageId">ID of the image to be removed.</param>
        /// <returns>Standard WorkflowResult.</returns>
        Task<WorkflowResult> RemoveImage(string imageId);

        /// <summary>
        /// Retrieve an image from the catalog.
        /// </summary>
        /// <param name="imageId">ID of the image to be received.</param>
        /// <returns>Result containing the image.</returns>
        Task<ImageResult> LoadImage(string imageId);

        /// <summary>
        /// Add a tag to an image.
        /// </summary>
        /// <param name="imageId">ID of the image.</param>
        /// <param name="tagName">Name of the tag.</param>
        /// <returns>Standard WorkflowResult.</returns>
        Task<WorkflowResult> AddTag(string imageId, string tagName);

        /// <summary>
        /// Remove a tag from an image.
        /// </summary>
        /// <param name="imageId">ID of the image.</param>
        /// <param name="tagName">Name of the tag.</param>
        /// <returns>Standard WorkflowResult.</returns>
        Task<WorkflowResult> RemoveTag(string imageId, string tagName);

        /// <summary>
        /// Load all tags combined into a single list including the count
        /// of each tag.
        /// </summary>
        /// <returns>Result containing the tag list.</returns>
        Task<TagsResult> LoadTags();

        /// <summary>
        /// Load thumbnails from the catalog. A supplied list of tags
        /// can be used to filter the thumbnails for those tags.
        /// </summary>
        /// <param name="tagNames">List of tag names to be used as a filter. Set to null or empty to disable filtering.</param>
        /// <returns>Result containing the thumbnails.</returns>
        Task<ThumbnailsResult> LoadThumbnails(List<string> tagNames);

        /// <summary>
        /// Read the connection string from the database in use.
        /// </summary>
        /// <returns>Result containing the connection string.</returns>
        Task<ConnectionStringResult> ReadConnectionString();

        /// <summary>
        /// Update a setting in the database.
        /// </summary>
        /// <param name="key">Setting identifier.</param>
        /// <param name="value">Setting value.</param>
        Task UpdateSetting(string key, string value);

        /// <summary>
        /// Read a setting from the database.
        /// </summary>
        /// <param name="key">Setting identifier.</param>
        /// <returns>Setting containing value.</returns>
        Task<SettingResult> ReadSetting(string key);
    }
}
