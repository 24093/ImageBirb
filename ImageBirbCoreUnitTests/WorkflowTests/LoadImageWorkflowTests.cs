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

        public LoadImageWorkflowTests()
        {
            _image = new Image
            {
                ImageId = "123"
            };

            _imageManagementAdapter = new Mock<IImageManagementAdapter>();
            _imageManagementAdapter.Setup(x => x.GetImage(_image.ImageId)).ReturnsAsync(_image);
        }

        [Fact]
        public async Task SuccessfullyLoadsImage()
        {
            // arrange
            var workflow = new LoadImageWorkflow(_imageManagementAdapter.Object);
            var parameters = new ImageIdParameters(_image.ImageId);

            // act
            var result = await workflow.Run(parameters);

            // assert
            Assert.Equal(ResultState.Success, result.State);
            _imageManagementAdapter.Verify(x => x.GetImage(_image.ImageId), Times.Once());
        }

        [Fact]
        public async Task ImageFailsToBeLoaded()
        {
            // arrange
            var imageManagementAdapter = new Mock<IImageManagementAdapter>();
            imageManagementAdapter.Setup(x => x.GetImage(It.IsAny<string>())).ThrowsAsync(new WorkflowTestException());

            var workflow = new LoadImageWorkflow(imageManagementAdapter.Object);
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
