using ImageBirb.Core.Ports.Primary;
using ImageBirb.Core.Workflows;

namespace ImageBirb.Core.Adapters.Primary
{
    public class WorkflowAdapter : IWorkflowAdapter
    {
        public AddImageWorkflow AddImage { get; }

        public RemoveImageWorkflow RemoveImage { get; }

        public LoadThumbnailsWorkflow LoadThumbnails { get; }

        public LoadImageWorkflow LoadImage { get; }

        public VerifyImageFileWorkflow VerifyImageFile { get; }

        public AddTagWorkflow AddTag { get; }

        public RemoveTagWorkflow RemoveTag { get; }

        public LoadTagsWorkflow LoadTags { get; }

        public LoadThumbnailsByTagsWorkflow LoadThumbnailsByTags { get; }

        public WorkflowAdapter(
            AddImageWorkflow addImageWorkflow, 
            RemoveImageWorkflow removeImageWorkflow,
            LoadThumbnailsWorkflow loadThumbnailsWorkflow, 
            LoadImageWorkflow loadImageWorkflow, 
            VerifyImageFileWorkflow verifyImageFileWorkflow,
            AddTagWorkflow addTagWorkflow,
            RemoveTagWorkflow removeTagWorkflow,
            LoadTagsWorkflow loadTagsWorkflow,
            LoadThumbnailsByTagsWorkflow loadThumbnailsByTagsWorkflow)
        {
            AddImage = addImageWorkflow;
            RemoveImage = removeImageWorkflow;
            LoadThumbnails = loadThumbnailsWorkflow;
            LoadImage = loadImageWorkflow;
            VerifyImageFile = verifyImageFileWorkflow;
            AddTag = addTagWorkflow;
            RemoveTag = removeTagWorkflow;
            LoadTags = loadTagsWorkflow;
            LoadThumbnailsByTags = loadThumbnailsByTagsWorkflow;
        }
    }
}
