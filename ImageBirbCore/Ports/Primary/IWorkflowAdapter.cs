using ImageBirb.Core.Workflows;

namespace ImageBirb.Core.Ports.Primary
{
    public interface IWorkflowAdapter
    {
        AddImageWorkflow AddImage { get; }

        RemoveImageWorkflow RemoveImage { get; }

        LoadThumbnailsWorkflow LoadThumbnails { get; }

        LoadImageWorkflow LoadImage { get; }

        VerifyImageFileWorkflow VerifyImageFile { get; }

        AddTagWorkflow AddTag { get; }

        RemoveTagWorkflow RemoveTag { get; }

        LoadTagsWorkflow LoadTags { get; }

        LoadThumbnailsByTagsWorkflow LoadThumbnailsByTags { get; }
    }
}
