using ImageBirb.Core.Common;
using ImageBirb.Core.Ports.Primary;
using ImageBirb.Core.Workflows;
using ImageBirb.Core.Workflows.Parameters;
using ImageBirb.Core.Workflows.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImageBirb.Core.Adapters.Primary
{
    internal class WorkflowAdapter : IWorkflowAdapter, IDisposable
    {
        public event EventHandler<ProgressChangedEventArgs> ProgressChanged;

        private readonly IWorkflowHost _workflows;

        public WorkflowAdapter(IWorkflowHost workflows)
        {
            _workflows = workflows;
            _workflows.ProgressChanged += WorkflowsOnProgressChanged;
        }
        
        public async Task<WorkflowResult> AddImages(IList<string> filenames)
        {
            return await _workflows.Run<AddImagesWorkflow, FilenamesParameters, WorkflowResult>(new FilenamesParameters(filenames));
        }

        public async Task<WorkflowResult> AddImages(string directory)
        {
            return await _workflows.Run<AddImagesWorkflow, FilenamesParameters, WorkflowResult>(new FilenamesParameters(directory));
        }

        public async Task<WorkflowResult> RemoveImage(string imageId)
        {
            return await _workflows.Run<RemoveImageWorkflow, ImageIdParameters, WorkflowResult>(new ImageIdParameters(imageId));
        }

        public async Task<ImageResult> LoadImage(string imageId)
        {
            return await _workflows.Run<LoadImageWorkflow, ImageIdParameters, ImageResult>(new ImageIdParameters(imageId));
        }
        
        public async Task<WorkflowResult> AddTag(string imageId, string tagName)
        {
            return await _workflows.Run<AddTagWorkflow, ImageIdTagParameters, WorkflowResult>(new ImageIdTagParameters(imageId, tagName));
        }

        public async Task<WorkflowResult> RemoveTag(string imageId, string tagName)
        {
            return await _workflows.Run<RemoveTagWorkflow, ImageIdTagParameters, WorkflowResult>(new ImageIdTagParameters(imageId, tagName));
        }

        public async Task<TagsResult> LoadTags()
        {
            return await _workflows.Run<LoadTagsWorkflow, TagsResult>();
        }

        public async Task<ThumbnailsResult> LoadThumbnails(List<string> tagNames)
        {
            return await _workflows.Run<LoadThumbnailsWorkflow, TagNamesParameters, ThumbnailsResult>(new TagNamesParameters(tagNames));
        }
        
        public async Task<ConnectionStringResult> ReadConnectionString()
        {
            return await _workflows.Run<ReadConnectionStringWorkflow, ConnectionStringResult>();
        }

        public async Task UpdateSettings(string key, string value)
        {
            await _workflows.Run<UpdateSettingsWorkflow, SettingParameters, WorkflowResult>(new SettingParameters(key, value));
        }

        public async Task<SettingResult> ReadSetting(string key)
        {
            return await _workflows.Run<ReadSettingsWorkflow, KeyParameters, SettingResult>(new KeyParameters(key));
        }

        public void Dispose()
        {
            _workflows.ProgressChanged -= WorkflowsOnProgressChanged;
        }

        private void WorkflowsOnProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressChanged?.Invoke(sender, e);
        }
    }
}
