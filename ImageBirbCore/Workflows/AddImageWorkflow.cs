using ImageBirb.Core.Common;
using ImageBirb.Core.Ports.Secondary;
using ImageBirb.Core.Workflows.Parameters;
using ImageBirb.Core.Workflows.Results;
using System.Threading.Tasks;

namespace ImageBirb.Core.Workflows
{
    internal class AddImageWorkflow : Workflow<FilenameParameters, WorkflowResult>
    {
        private readonly IImageManagementAdapter _imageManagementAdapter;
        private readonly IFileSystemAdapter _fileSystemAdapter;
        private readonly IImagingAdapter _imagingAdapter;

        public AddImageWorkflow(IImageManagementAdapter imageManagementAdapter, IFileSystemAdapter fileSystemAdapter, IImagingAdapter imagingAdapter)
        {
            _imageManagementAdapter = imageManagementAdapter;
            _fileSystemAdapter = fileSystemAdapter;
            _imagingAdapter = imagingAdapter;
        }

        protected override async Task<WorkflowResult> RunImpl(FilenameParameters p)
        {
            var imageId = await _imageManagementAdapter.CreateImageId();
            var imageData = await _fileSystemAdapter.ReadBinaryFile(p.Filename);
            var thumbnailData = await _imagingAdapter.CreateThumbnail(imageData);

            var image = new Image
            {
                ImageId = imageId,
                ImageData = imageData,
                ThumbnailData = thumbnailData
            };

            await _imageManagementAdapter.AddImage(image);

            return new WorkflowResult(ResultState.Success);
        }
    }
}
