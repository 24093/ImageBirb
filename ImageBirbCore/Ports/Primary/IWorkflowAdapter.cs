using ImageBirb.Core.Workflows.Results;
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
        Task<WorkflowResult> AddImage(string filename);

        Task<WorkflowResult> RemoveImage(string imageId);

        Task<ImageResult> LoadImage(string imageId);

        Task<IsBitmapImageResult> VerifyImageFile(string filename);

        Task<WorkflowResult> AddTag(string imageId, string tagName);

        Task<WorkflowResult> RemoveTag(string imageId, string tagName);

        Task<TagsResult> LoadTags();

        Task<ThumbnailsResult> LoadThumbnails(List<string> tagNames);

        Task<ConnectionStringResult> ReadConnectionString();
    }
}
