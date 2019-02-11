using ImageBirb.Core.Common;
using ImageBirb.Core.Ports.Secondary;
using ImageBirb.Core.Workflows.Parameters;
using ImageBirb.Core.Workflows.Results;
using System.Threading.Tasks;

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
            var imageData = await _fileSystemAdapter.ReadBinaryFile(p.Filename);
            var imageFormat = await _imagingAdapter.GetImageFormat(imageData);
            
            return new IsBitmapImageResult(ResultState.Success, imageFormat != ImageFormat.None);
        }
    }
}
