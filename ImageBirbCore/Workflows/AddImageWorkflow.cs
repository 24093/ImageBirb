using ImageBirb.Core.Common;
using ImageBirb.Core.Ports.Secondary;
using ImageBirb.Core.Workflows.Parameters;
using ImageBirb.Core.Workflows.Results;
using System.Threading.Tasks;

namespace ImageBirb.Core.Workflows
{
    internal class AddImageWorkflow : Workflow<FilenameParameters, WorkflowResult>
    {
        private readonly IDatabaseAdapter _databaseAdapter;
        private readonly IFileSystemAdapter _fileSystemAdapter;
        private readonly IImagingAdapter _imagingAdapter;

        public AddImageWorkflow(IDatabaseAdapter databaseAdapter, IFileSystemAdapter fileSystemAdapter, IImagingAdapter imagingAdapter)
        {
            _databaseAdapter = databaseAdapter;
            _fileSystemAdapter = fileSystemAdapter;
            _imagingAdapter = imagingAdapter;
        }

        protected override async Task<WorkflowResult> Run(FilenameParameters p)
        {
            var imageId = await _databaseAdapter.CreateImageId();
            var imageData = await _fileSystemAdapter.ReadBinaryFile(p.Filename);
            var thumbnailData = await _imagingAdapter.CreateThumbnail(imageData);

            var image = new Image
            {
                ImageId = imageId,
                ImageData = imageData,
                ThumbnailData = thumbnailData
            };

            await _databaseAdapter.AddImage(image);

            return new WorkflowResult(ResultState.Success);
        }
    }
}
