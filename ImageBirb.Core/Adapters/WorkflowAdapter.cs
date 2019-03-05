using ImageBirb.Core.BusinessObjects;
using ImageBirb.Core.Ports.Primary;
using ImageBirb.Core.Workflows;
using ImageBirb.Core.Workflows.Parameters;
using ImageBirb.Core.Workflows.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImageBirb.Core.Adapters
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

        public async Task AddImages(IList<string> filenames, ImageStorageType imageStorageType,
            bool ignoreSimilarImages, double similarityThreshold)
        {
            await _workflows.Run<AddImagesWorkflow, AddImagesParameters, AddImagesResult>(
                new AddImagesParameters(filenames, imageStorageType, ignoreSimilarImages, similarityThreshold));
        }

        public async Task AddImages(string directory, ImageStorageType imageStorageType, bool ignoreSimilarImages,
            double similarityThreshold)
        {
            await _workflows.Run<AddImagesWorkflow, AddImagesParameters, AddImagesResult>(
                new AddImagesParameters(directory, imageStorageType, ignoreSimilarImages, similarityThreshold));
        }

        public async Task RemoveImage(string imageId)
        {
            await _workflows.Run<RemoveImageWorkflow, ImageIdParameters, WorkflowResult>(new ImageIdParameters(imageId));
        }

        public async Task<Image> LoadImage(string imageId)
        {
            return (await _workflows.Run<LoadImageWorkflow, ImageIdParameters, ImageResult>(new ImageIdParameters(imageId)))?.Image;
        }
        
        public async Task AddTag(string imageId, string tagName)
        {
            await _workflows.Run<AddTagWorkflow, ImageIdTagParameters, WorkflowResult>(new ImageIdTagParameters(imageId, tagName));
        }

        public async Task RemoveTag(string imageId, string tagName)
        {
            await _workflows.Run<RemoveTagWorkflow, ImageIdTagParameters, WorkflowResult>(new ImageIdTagParameters(imageId, tagName));
        }

        public async Task<IList<Tag>> LoadTags()
        {
            return (await _workflows.Run<LoadTagsWorkflow, TagsResult>())?.Tags;
        }

        public async Task<IList<Image>> LoadThumbnails(List<string> tagNames)
        {
            return (await _workflows.Run<LoadThumbnailsWorkflow, TagNamesParameters, ThumbnailsResult>(new TagNamesParameters(tagNames)))?.Thumbnails;
        }
        
        public async Task<string> ReadConnectionString()
        {
            return (await _workflows.Run<ReadConnectionStringWorkflow, ConnectionStringResult>())?.ConnectionString;
        }

        public async Task UpdateSetting(string key, string value)
        {
            await _workflows.Run<UpdateSettingWorkflow, SettingParameters, WorkflowResult>(new SettingParameters(key, value));
        }

        public async Task<Setting> ReadSetting(string key, string defaultValue)
        {
            return (await _workflows.Run<ReadSettingWorkflow, SettingParameters, SettingResult>(new SettingParameters(key, defaultValue)))?.Setting;
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
