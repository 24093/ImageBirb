using ImageBirb.Core.Ports.Secondary;
using ImageBirb.Core.Workflows;
using ImageBirb.Core.Workflows.Parameters;
using ImageBirb.Core.Workflows.Results;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace ImageBirbCoreUnitTests.WorkflowTests
{
    public class RemoveTagWorkflowTests
    {
        private readonly string _imageId;
        private readonly string _tagName;

        private readonly Mock<IDatabaseAdapter> _databaseAdapter;
        
        public RemoveTagWorkflowTests()
        {
            _imageId = "123";
            _tagName = "taggg";

            _databaseAdapter = new Mock<IDatabaseAdapter>();
            _databaseAdapter.Setup(x => x.RemoveTag(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);
        }

        [Fact]
        public async Task SuccessfullyRemovesImage()
        {
            // arrange
            var workflow = new RemoveTagWorkflow(_databaseAdapter.Object);
            var parameters = new ImageIdTagParameters(_imageId, _tagName);

            // act
            var result = await workflow.Run(parameters);

            // assert
            Assert.Equal(ResultState.Success, result.State);
            _databaseAdapter.Verify(x => x.RemoveTag(_imageId, _tagName), Times.Once());
        }

        [Fact]
        public async Task TageFailsToBeRemoved()
        {
            // arrange
            var databaseAdapter = new Mock<IDatabaseAdapter>();
            databaseAdapter.Setup(x => x.RemoveTag(It.IsAny<string>(), It.IsAny<string>())).ThrowsAsync(new WorkflowTestException());

            var workflow = new RemoveTagWorkflow(databaseAdapter.Object);
            var parameters = new ImageIdTagParameters(_imageId, _tagName);

            // act
            var result = await workflow.Run(parameters);

            // assert
            Assert.Equal(ResultState.Error, result.State);
            Assert.Equal(ErrorCode.WorkflowInternalError, result.ErrorCode);
            Assert.IsType<WorkflowTestException>(result.Exception);
        }
    }
}
