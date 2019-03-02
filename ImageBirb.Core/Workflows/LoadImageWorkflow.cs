using ImageBirb.Core.Workflows.Parameters;
using ImageBirb.Core.Workflows.Results;
using System.Threading.Tasks;
using ImageBirb.Core.BusinessObjects;
using ImageBirb.Core.Ports.Secondary;

namespace ImageBirb.Core.Workflows
{
    internal class LoadImageWorkflow : Workflow<ImageIdParameters, ImageResult>
    {
        private readonly IImageManagementAdapter _imageManagementAdapter;

        private readonly IFileSystemAdapter _fileSystemAdapter;

        public LoadImageWorkflow(IImageManagementAdapter imageManagementAdapter, IFileSystemAdapter fileSystemAdapter)
        {
            _imageManagementAdapter = imageManagementAdapter;
            _fileSystemAdapter = fileSystemAdapter;
        }

        protected override async Task<ImageResult> RunImpl(ImageIdParameters p)
        {
            var image = await _imageManagementAdapter.GetImage(p.ImageId);

            if (image.ImageStorageType == ImageStorageType.LinkToSource)
            {
                var imageData = await _fileSystemAdapter.ReadBinaryFile(image.Filename);
                image.ImageData = imageData;
            }

            return new ImageResult(image);
        }
    }
}
