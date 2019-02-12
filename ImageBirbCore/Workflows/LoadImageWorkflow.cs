using ImageBirb.Core.Workflows.Parameters;
using ImageBirb.Core.Workflows.Results;
using System.Threading.Tasks;
using ImageBirb.Core.Ports.Secondary;

namespace ImageBirb.Core.Workflows
{
    internal class LoadImageWorkflow : Workflow<ImageIdParameters, ImageResult>
    {
        private readonly IImageManagementAdapter _imageManagementAdapter;

        public LoadImageWorkflow(IImageManagementAdapter imageManagementAdapter)
        {
            _imageManagementAdapter = imageManagementAdapter;
        }

        protected override async Task<ImageResult> RunImpl(ImageIdParameters p)
        {
            var image = await _imageManagementAdapter.GetImage(p.ImageId);
            return new ImageResult(ResultState.Success, image);
        }
    }
}
