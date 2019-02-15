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
    public class LoadImageWorkflowTests
    {
        private readonly Image _image;

        private readonly Mock<IImageManagementAdapter> _imageManagementAdapter;

        private readonly Mock<IFileSystemAdapter> _fileSystemAdapter;

        public LoadImageWorkflowTests()
        {
            _image = new Image
            {
                ImageId = "123",
                ImageData = new byte[] { 1, 2 },
                Filename = "123.bmp"
            };

            _imageManagementAdapter = new Mock<IImageManagementAdapter>();
            _imageManagementAdapter.Setup(x => x.GetImage(_image.ImageId)).ReturnsAsync(_image);

            _fileSystemAdapter = new Mock<IFileSystemAdapter>();
            _fileSystemAdapter.Setup(x => x.ReadBinaryFile(It.IsAny<string>())).ReturnsAsync(_image.ImageData);
        }

        [Fact]
        public async Task SuccessfullyLoadsImage()
        {
            // arrange
            var workflow = new LoadImageWorkflow(_imageManagementAdapter.Object, _fileSystemAdapter.Object);
            var parameters = new ImageIdParameters(_image.ImageId);

            // act
            var result = await workflow.Run(parameters);

            // assert
            Assert.Equal(ResultState.Success, result.State);
            _imageManagementAdapter.Verify(x => x.GetImage(_image.ImageId), Times.Once());
            _fileSystemAdapter.Verify(x => x.ReadBinaryFile(_image.Filename), Times.Once);
        }

        [Fact]
        public async Task ImageFailsToBeLoaded()
        {
            // arrange
            var imageManagementAdapter = new Mock<IImageManagementAdapter>();
            imageManagementAdapter.Setup(x => x.GetImage(It.IsAny<string>())).ThrowsAsync(new WorkflowTestException());

            var workflow = new LoadImageWorkflow(imageManagementAdapter.Object, _fileSystemAdapter.Object);
            var parameters = new ImageIdParameters(_image.ImageId);

            // act
            var result = await workflow.Run(parameters);

            // assert
            Assert.Equal(ResultState.Error, result.State);
            Assert.Equal(ErrorCode.WorkflowInternalError, result.ErrorCode);
            Assert.IsType<WorkflowTestException>(result.Exception);
        }
    }
}
