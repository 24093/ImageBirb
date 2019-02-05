using GalaSoft.MvvmLight;
using ImageBirb.Core.Ports.Primary;
using ImageBirb.Core.Workflows;
using ImageBirb.Core.Workflows.Parameters;
using ImageBirb.Core.Workflows.Results;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

namespace ImageBirb.ViewModels
{
    /// <summary>
    /// Base view model for anything that needs access to the backend.
    /// </summary>
    internal abstract class WorkflowViewModel : ViewModelBase
    {
        private readonly IWorkflowAdapter _workflowAdapter;

        protected WorkflowViewModel(IWorkflowAdapter workflowAdapter)
        {
            _workflowAdapter = workflowAdapter;
        }

        protected async Task<WorkflowResult> AddImage(string filename)
        {
            return await Run(_workflowAdapter.AddImage, new FilenameParameters(filename));
        }

        protected async Task<WorkflowResult> RemoveImage(string imageId)
        {
            return await Run(_workflowAdapter.RemoveImage, new ImageIdParameters(imageId));
        }

        protected async Task<ThumbnailsResult> LoadThumbnails()
        {
            return await Run(_workflowAdapter.LoadThumbnails);
        }

        protected async Task<ImageResult> LoadImage(string imageId)
        {
            return await Run(_workflowAdapter.LoadImage, new ImageIdParameters(imageId));
        }

        protected async Task<IsBitmapImageResult> VerifyImageFile(string filename)
        {
            return await Run(_workflowAdapter.VerifyImageFile, new FilenameParameters(filename));
        }

        protected async Task<WorkflowResult> AddTag(string imageId, string tagName)
        {
            return await Run(_workflowAdapter.AddTag, new ImageIdTagParameters(imageId, tagName));
        }

        protected async Task<WorkflowResult> RemoveTag(string imageId, string tagName)
        {
            return await Run(_workflowAdapter.RemoveTag, new ImageIdTagParameters(imageId, tagName));
        }

        protected async Task<TagsResult> LoadTags()
        {
            return await Run(_workflowAdapter.LoadTags);
        }

        protected async Task<ThumbnailsResult> LoadThumbnailsByTags(List<string> tagNames)
        {
            return await Run(_workflowAdapter.LoadThumbnailsByTags, new TagNamesParameters(tagNames));
        }

        protected async Task<SettingsResult> ReadSettings()
        {
            return await Run(_workflowAdapter.ReadSettings);
        }

        protected async Task<WorkflowResult> UpdateSetting(string key, object value)
        {
            return await Run(_workflowAdapter.UpdateSetting, new KeyValueParameters(key, value)));
        }

        private static async Task<TResult> Run<TParameters, TResult>(Workflow<TParameters, TResult> workflow, TParameters parameters)
            where TParameters : WorkflowParameters
            where TResult : WorkflowResult
        {
            var result = await workflow.RunWorkflow(parameters);
            VerifyWorkflowResult(result);
            return result;
        }

        private static async Task<TResult> Run<TResult>(Workflow<TResult> workflow)
            where TResult : WorkflowResult
        {
            var result = await workflow.RunWorkflow();
            VerifyWorkflowResult(result);
            return result;
        }

        private static void VerifyWorkflowResult(WorkflowResult result)
        {
            if (!result.IsSuccess)
            {
                var message = result.Exception?.Message;

                if (Debugger.IsAttached)
                {
                    message += Environment.NewLine + Environment.NewLine + result.Exception?.StackTrace;
                }

                message = message?.Trim() ?? string.Empty;

                MessageBox.Show(message, result.ErrorCode.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}