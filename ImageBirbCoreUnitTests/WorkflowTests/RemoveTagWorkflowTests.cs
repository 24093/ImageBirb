using ImageBirb.Core.Workflows;
using ImageBirb.Core.Workflows.Parameters;
using ImageBirb.Core.Workflows.Results;
using Moq;
using System.Threading.Tasks;
using ImageBirb.Core.Ports.Secondary;
using Xunit;

namespace ImageBirbCoreUnitTests.WorkflowTests
{
    public class RemoveTagWorkflowTests
    {
        private readonly string _imageId;
        private readonly string _tagName;

        private readonly Mock<ITagManagementAdapter> _tagManagementAdapter;
        
        public RemoveTagWorkflowTests()
        {
            _imageId = "123";
            _tagName = "taggg";

            _tagManagementAdapter = new Mock<ITagManagementAdapter>();
            _tagManagementAdapter.Setup(x => x.RemoveTag(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);
        }

        [Fact]
        public async Task SuccessfullyRemovesImage()
        {
            // arrange
            var workflow = new RemoveTagWorkflow(_tagManagementAdapter.Object);
            var parameters = new ImageIdTagParameters(_imageId, _tagName);

            // act
            var result = await workflow.Run(parameters);

            // assert
            Assert.True(result.IsSuccess);
            _tagManagementAdapter.Verify(x => x.RemoveTag(_imageId, _tagName), Times.Once());
        }

        [Fact]
        public async Task TageFailsToBeRemoved()
        {
            // arrange
            var tagMangementAdapter = new Mock<ITagManagementAdapter>();
            tagMangementAdapter.Setup(x => x.RemoveTag(It.IsAny<string>(), It.IsAny<string>())).ThrowsAsync(new WorkflowTestException());

            var workflow = new RemoveTagWorkflow(tagMangementAdapter.Object);
            var parameters = new ImageIdTagParameters(_imageId, _tagName);

            // act
            var result = await workflow.Run(parameters);

            // assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorCode.WorkflowInternalError, result.ErrorCode);
            Assert.IsType<WorkflowTestException>(result.Exception);
        }
    }
}
