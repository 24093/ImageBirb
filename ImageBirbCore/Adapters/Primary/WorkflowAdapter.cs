using ImageBirb.Core.Ports.Primary;
using ImageBirb.Core.Workflows;
using ImageBirb.Core.Workflows.Parameters;
using ImageBirb.Core.Workflows.Results;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// ReSharper disable SuggestBaseTypeForParameter

namespace ImageBirb.Core.Adapters.Primary
{
    public class WorkflowAdapter : IWorkflowAdapter
    {
        private readonly List<IWorkflow> _workflows;

        public async Task<WorkflowResult> AddImage(string filename)
        {
            return await Run<AddImageWorkflow, FilenameParameters, WorkflowResult>(new FilenameParameters(filename));
        }

        public async Task<WorkflowResult> RemoveImage(string imageId)
        {
            return await Run<RemoveImageWorkflow, ImageIdParameters, WorkflowResult>(new ImageIdParameters(imageId));
        }

        public async Task<ThumbnailsResult> LoadThumbnails()
        {
            return await Run<LoadThumbnailsWorkflow, ThumbnailsResult>();
        }

        public async Task<ImageResult> LoadImage(string imageId)
        {
            return await Run<LoadImageWorkflow, ImageIdParameters, ImageResult>(new ImageIdParameters(imageId));
        }

        public async Task<IsBitmapImageResult> VerifyImageFile(string filename)
        {
            return await Run<VerifyImageFileWorkflow, FilenameParameters, IsBitmapImageResult>(new FilenameParameters(filename));
        }

        public async Task<WorkflowResult> AddTag(string imageId, string tagName)
        {
            return await Run<AddTagWorkflow, ImageIdTagParameters, WorkflowResult>(new ImageIdTagParameters(imageId, tagName));
        }

        public async Task<WorkflowResult> RemoveTag(string imageId, string tagName)
        {
            return await Run<RemoveTagWorkflow, ImageIdTagParameters, WorkflowResult>(new ImageIdTagParameters(imageId, tagName));
        }

        public async Task<TagsResult> LoadTags()
        {
            return await Run<LoadTagsWorkflow, TagsResult>();
        }

        public async Task<ThumbnailsResult> LoadThumbnailsByTags(List<string> tagNames)
        {
            return await Run<LoadThumbnailsByTagsWorkflow, TagNamesParameters, ThumbnailsResult>(new TagNamesParameters(tagNames));
        }
        
        public async Task<SettingsResult> ReadSettings()
        {
            return await Run<ReadSettingsWorkflow, SettingsResult>();
        }

        public async Task<WorkflowResult> UpdateSetting(string key, object value)
        {
            return await Run<UpdateSettingWorkflow, KeyValueParameters, WorkflowResult>(new KeyValueParameters(key, value));
        }

        private async Task<TResult> Run<TWorkflow, TParameters, TResult>(TParameters parameters)
            where TWorkflow : Workflow<TParameters, TResult>
            where TParameters : WorkflowParameters
            where TResult : WorkflowResult
        {
            var workflow = _workflows.OfType<TWorkflow>().Single();
            var result = await workflow.RunWorkflow(parameters);
            return result;
        }

        private async Task<TResult> Run<TWorkflow, TResult>()
            where TWorkflow : Workflow<TResult>
            where TResult : WorkflowResult
        {
            var workflow = _workflows.OfType<TWorkflow>().Single();
            var result = await workflow.RunWorkflow();
            return result;
        }

        public WorkflowAdapter(
            AddImageWorkflow addImageWorkflow, 
            RemoveImageWorkflow removeImageWorkflow,
            LoadThumbnailsWorkflow loadThumbnailsWorkflow, 
            LoadImageWorkflow loadImageWorkflow, 
            VerifyImageFileWorkflow verifyImageFileWorkflow,
            AddTagWorkflow addTagWorkflow,
            RemoveTagWorkflow removeTagWorkflow,
            LoadTagsWorkflow loadTagsWorkflow,
            LoadThumbnailsByTagsWorkflow loadThumbnailsByTagsWorkflow,
            ReadSettingsWorkflow readSettingsWorkflow,
            UpdateSettingWorkflow updateSettingWorkflow)
        {
            _workflows = new List<IWorkflow>
            {
                addImageWorkflow,
                removeImageWorkflow,
                loadThumbnailsWorkflow,
                loadImageWorkflow,
                verifyImageFileWorkflow,
                addTagWorkflow,
                removeTagWorkflow,
                loadTagsWorkflow,
                loadThumbnailsByTagsWorkflow,
                readSettingsWorkflow,
                updateSettingWorkflow
            };
        }
    }
}
