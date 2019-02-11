using System.Linq;
using System.Threading.Tasks;
using ImageBirb.Core.Common;
using ImageBirb.Core.Ports.Secondary;
using ImageBirb.Core.Workflows.Parameters;
using ImageBirb.Core.Workflows.Results;

namespace ImageBirb.Core.Workflows
{
    internal class VerifyImageFileWorkflow : Workflow<FilenameParameters, IsBitmapImageResult>
    {
        private readonly IFileSystemAdapter _fileSystemAdapter;
        private readonly IImagingAdapter _imagingAdapter;

        public VerifyImageFileWorkflow(IFileSystemAdapter fileSystemAdapter, IImagingAdapter imagingAdapter)
        {
            _fileSystemAdapter = fileSystemAdapter;
            _imagingAdapter = imagingAdapter;
        }

        protected override async Task<IsBitmapImageResult> RunImpl(FilenameParameters p)
        {
            var validFormats = new[]
            {
                ImageFormat.Jpeg,
                ImageFormat.Png,
                ImageFormat.Bmp
            };

            var imageData = await _fileSystemAdapter.ReadBinaryFile(p.Filename);
            var imageFormat = await _imagingAdapter.GetImageFormat(imageData);

            var isValidFormat = validFormats.Contains(imageFormat);

            return new IsBitmapImageResult(ResultState.Success, isValidFormat);
        }
    }
}
