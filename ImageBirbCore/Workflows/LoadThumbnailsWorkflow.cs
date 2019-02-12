using ImageBirb.Core.Workflows.Parameters;
using ImageBirb.Core.Workflows.Results;
using System.Threading.Tasks;
using ImageBirb.Core.Ports.Secondary;

namespace ImageBirb.Core.Workflows
{
    internal class LoadThumbnailsWorkflow : Workflow<TagNamesParameters, ThumbnailsResult>
    {
        private readonly IImageManagementAdapter _imageManagementAdapter;

        public LoadThumbnailsWorkflow(IImageManagementAdapter imageManagementAdapter)
        {
            _imageManagementAdapter = imageManagementAdapter;
        }

        protected override async Task<ThumbnailsResult> RunImpl(TagNamesParameters p)
        {
            var thumbnails = await _imageManagementAdapter.GetThumbnails(p.TagNames);
            return new ThumbnailsResult(ResultState.Success, thumbnails);
        }
    }
}