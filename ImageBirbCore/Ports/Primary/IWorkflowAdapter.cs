using ImageBirb.Core.Workflows.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImageBirb.Core.Ports.Primary
{
    public interface IWorkflowAdapter
    {
        Task<WorkflowResult> AddImage(string filename);

        Task<WorkflowResult> RemoveImage(string imageId);

        Task<ThumbnailsResult> LoadThumbnails();

        Task<ImageResult> LoadImage(string imageId);

        Task<IsBitmapImageResult> VerifyImageFile(string filename);

        Task<WorkflowResult> AddTag(string imageId, string tagName);

        Task<WorkflowResult> RemoveTag(string imageId, string tagName);

        Task<TagsResult> LoadTags();

        Task<ThumbnailsResult> LoadThumbnailsByTags(List<string> tagNames);

        Task<SettingsResult> ReadSettings();

        Task<WorkflowResult> UpdateSetting(string key, object value);

        Task<ConnectionStringResult> ReadConnectionString();
    }
}
