using ImageBirb.Core.Ports.Secondary;
using ImageBirb.Core.Workflows;
using ImageBirb.Core.Workflows.Parameters;
using ImageBirb.Core.Workflows.Results;
using Moq;
using System.Threading.Tasks;
using ImageBirb.Core.Common;
using Xunit;

namespace ImageBirbCoreUnitTests.WorkflowTests
{
    public class AddImageWorkflowTests
    {
        private readonly Image _image;

        private readonly Mock<IImageManagementAdapter> _imageManagementAdapter;
        
        private readonly Mock<IFileSystemAdapter> _fileSystemAdapter;

        private readonly Mock<IImagingAdapter> _imagingAdapter;

        public AddImageWorkflowTests()
        {
            _image = new Image
            {
                ImageId = "123",
                ImageData = new byte[] {1, 2},
                ThumbnailData = new byte[] {3, 4}
            };

            _imageManagementAdapter = new Mock<IImageManagementAdapter>();
            _imageManagementAdapter.Setup(x => x.CreateImageId()).ReturnsAsync(_image.ImageId);
            _imageManagementAdapter.Setup(x => x.AddImage(It.IsAny<Image>())).Returns(Task.CompletedTask);

            _fileSystemAdapter = new Mock<IFileSystemAdapter>();
            _fileSystemAdapter.Setup(x => x.ReadBinaryFile(It.IsAny<string>())).ReturnsAsync(_image.ImageData);

            _imagingAdapter = new Mock<IImagingAdapter>();
            _imagingAdapter.Setup(x => x.CreateThumbnail(It.IsAny<byte[]>())).ReturnsAsync(_image.ThumbnailData);
        }

        [Fact]
        public async Task SuccessfullyAddsImage()
        {
            // arrange
            var workflow = new AddImageWorkflow(_imageManagementAdapter.Object, _fileSystemAdapter.Object, _imagingAdapter.Object);
            var parameters = new FilenameParameters("kuckuck.jpg");

            // act
            var result = await workflow.Run(parameters);

            // assert
            Assert.Equal(ResultState.Success, result.State);
            _imageManagementAdapter.Verify(x => x.AddImage(It.Is<Image>(i => i.Equals(_image))), Times.Once());
        }

        [Fact]
        public async Task ImageIdFailsToBeCreated()
        {
            // arrange
            var imageManagementAdapter = new Mock<IImageManagementAdapter>();
            imageManagementAdapter.Setup(x => x.CreateImageId()).ThrowsAsync(new WorkflowTestException());
            imageManagementAdapter.Setup(x => x.AddImage(It.IsAny<Image>())).Returns(Task.CompletedTask);

            var workflow = new AddImageWorkflow(imageManagementAdapter.Object, _fileSystemAdapter.Object, _imagingAdapter.Object);
            var parameters = new FilenameParameters("kuckuck.jpg");

            // act
            var result = await workflow.Run(parameters);

            // assert
            Assert.Equal(ResultState.Error, result.State);
            Assert.Equal(ErrorCode.WorkflowInternalError, result.ErrorCode);
            Assert.IsType<WorkflowTestException>(result.Exception);
            imageManagementAdapter.Verify(x => x.AddImage(It.IsAny<Image>()), Times.Never);
        }

        [Fact]
        public async Task FileFailsToBeRead()
        {
            // arrange
            var fileSystemAdapter = new Mock<IFileSystemAdapter>();
            fileSystemAdapter.Setup(x => x.ReadBinaryFile(It.IsAny<string>())).ThrowsAsync(new WorkflowTestException());

            var workflow = new AddImageWorkflow(_imageManagementAdapter.Object, fileSystemAdapter.Object, _imagingAdapter.Object);
            var parameters = new FilenameParameters("kuckuck.jpg");

            // act
            var result = await workflow.Run(parameters);

            // assert
            Assert.Equal(ResultState.Error, result.State);
            Assert.Equal(ErrorCode.WorkflowInternalError, result.ErrorCode);
            Assert.IsType<WorkflowTestException>(result.Exception);
            _imageManagementAdapter.Verify(x => x.AddImage(It.IsAny<Image>()), Times.Never);
        }

        [Fact]
        public async Task ThumbnailFailsToBeCreated()
        {
            // arrange
            var imagingAdapter = new Mock<IImagingAdapter>();
            imagingAdapter.Setup(x => x.CreateThumbnail(It.IsAny<byte[]>())).ThrowsAsync(new WorkflowTestException());

            var workflow = new AddImageWorkflow(_imageManagementAdapter.Object, _fileSystemAdapter.Object, imagingAdapter.Object);
            var parameters = new FilenameParameters("kuckuck.jpg");

            // act
            var result = await workflow.Run(parameters);

            // assert
            Assert.Equal(ResultState.Error, result.State);
            Assert.Equal(ErrorCode.WorkflowInternalError, result.ErrorCode);
            Assert.IsType<WorkflowTestException>(result.Exception);
            _imageManagementAdapter.Verify(x => x.AddImage(It.IsAny<Image>()), Times.Never);
        }
    }
}