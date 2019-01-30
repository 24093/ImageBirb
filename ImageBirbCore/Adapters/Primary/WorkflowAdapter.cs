using ImageBirb.Core.Ports.Primary;
using ImageBirb.Core.Workflows;

namespace ImageBirb.Core.Adapters.Primary
{
    public class WorkflowAdapter : IWorkflowAdapter
    {
        public AddImageWorkflow AddImage { get; }

        public LoadThumbnailsWorkflow LoadThumbnails { get; }

        public LoadImageWorkflow LoadImage { get; }

        public VerifyImageFileWorkflow VerifyImageFile { get; }

        public AddTagWorkflow AddTag { get; }

        public RemoveTagWorkflow RemoveTag { get; }

        public LoadTagsWorkflow LoadTags { get; }

        public WorkflowAdapter(
            AddImageWorkflow addImageWorkflow, 
            LoadThumbnailsWorkflow loadThumbnailsWorkflow, 
            LoadImageWorkflow loadImageWorkflow, 
            VerifyImageFileWorkflow verifyImageFileWorkflow,
            AddTagWorkflow addTagWorkflow,
            RemoveTagWorkflow removeTagWorkflow,
            LoadTagsWorkflow loadTagsWorkflow)
        {
            AddImage = addImageWorkflow;
            LoadThumbnails = loadThumbnailsWorkflow;
            LoadImage = loadImageWorkflow;
            VerifyImageFile = verifyImageFileWorkflow;
            AddTag = addTagWorkflow;
            RemoveTag = removeTagWorkflow;
            LoadTags = loadTagsWorkflow;
        }
    }
}
