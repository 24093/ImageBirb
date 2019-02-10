﻿using ImageBirb.Core.Ports.Primary;
using ImageBirb.Core.Workflows;
using ImageBirb.Core.Workflows.Parameters;
using ImageBirb.Core.Workflows.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImageBirb.Core.Adapters.Primary
{
    internal class WorkflowAdapter : IWorkflowAdapter
    {
        private readonly WorkflowHost _workflows;

        public WorkflowAdapter(WorkflowHost workflows)
        {
            _workflows = workflows;
        }

        public async Task<WorkflowResult> AddImage(string filename)
        {
            return await _workflows.Run<AddImageWorkflow, FilenameParameters, WorkflowResult>(new FilenameParameters(filename));
        }

        public async Task<WorkflowResult> RemoveImage(string imageId)
        {
            return await _workflows.Run<RemoveImageWorkflow, ImageIdParameters, WorkflowResult>(new ImageIdParameters(imageId));
        }

        public async Task<ImageResult> LoadImage(string imageId)
        {
            return await _workflows.Run<LoadImageWorkflow, ImageIdParameters, ImageResult>(new ImageIdParameters(imageId));
        }

        public async Task<IsBitmapImageResult> VerifyImageFile(string filename)
        {
            return await _workflows.Run<VerifyImageFileWorkflow, FilenameParameters, IsBitmapImageResult>(new FilenameParameters(filename));
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
    }
}
