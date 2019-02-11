using ImageBirb.Core.Common;
using ImageBirb.Core.Ports.Secondary;
using ImageBirb.Core.Workflows;
using ImageBirb.Core.Workflows.Parameters;
using ImageBirb.Core.Workflows.Results;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace ImageBirbCoreUnitTests.WorkflowTests
{
    public class VerifyImageWorkflowTests
    {
        private readonly string _fileName;

        private readonly byte[] _imageData;

        private readonly Mock<IFileSystemAdapter> _fileSystemAdapter;

        private readonly Mock<IImagingAdapter> _imagingAdapter;

        public VerifyImageWorkflowTests()
        {
            _fileName = "img.bmp";
            _imageData = new byte[] { 1, 2 };

            _fileSystemAdapter = new Mock<IFileSystemAdapter>();
            _fileSystemAdapter.Setup(x => x.ReadBinaryFile(It.IsAny<string>())).ReturnsAsync(_imageData);

            _imagingAdapter = new Mock<IImagingAdapter>();
            _imagingAdapter.Setup(x => x.GetImageFormat(It.IsAny<byte[]>())).ReturnsAsync(ImageFormat.Bmp);
        }

        [Fact]
        public async Task SuccessfullyVerifiesImage()
        {
            // arrange
            var workflow = new VerifyImageFileWorkflow(_fileSystemAdapter.Object, _imagingAdapter.Object);
            var parameters = new FilenameParameters(_fileName);

            // act
            var result = await workflow.Run(parameters);

            // assert
            Assert.Equal(ResultState.Success, result.State);
            _fileSystemAdapter.Verify(x => x.ReadBinaryFile(_fileName), Times.Once());
            _imagingAdapter.Verify(x => x.GetImageFormat(_imageData), Times.Once);
        }

        [Fact]
        public async Task FileFailsToBeRead()
        {
            // arrange
            var fileSystemAdapter = new Mock<IFileSystemAdapter>();
            fileSystemAdapter.Setup(x => x.ReadBinaryFile(It.IsAny<string>())).ThrowsAsync(new WorkflowTestException());
            
            var workflow = new VerifyImageFileWorkflow(fileSystemAdapter.Object, _imagingAdapter.Object);
            var parameters = new FilenameParameters(_fileName);

            // act
            var result = await workflow.Run(parameters);

            // assert
            Assert.Equal(ResultState.Error, result.State);
            Assert.Equal(ErrorCode.WorkflowInternalError, result.ErrorCode);
            Assert.IsType<WorkflowTestException>(result.Exception);
            _imagingAdapter.Verify(x => x.GetImageFormat(_imageData), Times.Never);
        }

        [Fact]
        public async Task ImageFormatFailsToBeResolved()
        {
            // arrange
            var imagingAdapter = new Mock<IImagingAdapter>();
            imagingAdapter.Setup(x => x.GetImageFormat(It.IsAny<byte[]>())).ThrowsAsync(new WorkflowTestException());

            var workflow = new VerifyImageFileWorkflow(_fileSystemAdapter.Object, imagingAdapter.Object);
            var parameters = new FilenameParameters(_fileName);

            // act
            var result = await workflow.Run(parameters);

            // assert
            Assert.Equal(ResultState.Error, result.State);
            Assert.Equal(ErrorCode.WorkflowInternalError, result.ErrorCode);
            Assert.IsType<WorkflowTestException>(result.Exception);
        }
    }
}
