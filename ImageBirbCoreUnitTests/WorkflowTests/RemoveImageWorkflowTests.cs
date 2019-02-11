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
    public class RemoveImageWorkflowTests
    {
        private readonly string _imageId;

        private readonly Mock<IDatabaseAdapter> _databaseAdapter;
        
        public RemoveImageWorkflowTests()
        {
            _imageId = "123";

            _databaseAdapter = new Mock<IDatabaseAdapter>();
            _databaseAdapter.Setup(x => x.RemoveImage(It.IsAny<string>())).Returns(Task.CompletedTask);
        }

        [Fact]
        public async Task SuccessfullyRemovesImage()
        {
            // arrange
            var workflow = new RemoveImageWorkflow(_databaseAdapter.Object);
            var parameters = new ImageIdParameters(_imageId);

            // act
            var result = await workflow.Run(parameters);

            // assert
            Assert.Equal(ResultState.Success, result.State);
            _databaseAdapter.Verify(x => x.RemoveImage(_imageId), Times.Once());
        }

        [Fact]
        public async Task ImageFailsToBeRemoved()
        {
            // arrange
            var databaseAdapter = new Mock<IDatabaseAdapter>();
            databaseAdapter.Setup(x => x.RemoveImage(It.IsAny<string>())).ThrowsAsync(new WorkflowTestException());

            var workflow = new RemoveImageWorkflow(databaseAdapter.Object);
            var parameters = new ImageIdParameters(_imageId);

            // act
            var result = await workflow.Run(parameters);

            // assert
            Assert.Equal(ResultState.Error, result.State);
            Assert.Equal(ErrorCode.WorkflowInternalError, result.ErrorCode);
            Assert.IsType<WorkflowTestException>(result.Exception);
        }
    }
}
