using ImageBirb.Core.Workflows;
using ImageBirb.Core.Workflows.Parameters;
using ImageBirb.Core.Workflows.Results;
using Moq;
using System.Threading.Tasks;
using ImageBirb.Core.Ports.Secondary;
using Xunit;

namespace ImageBirbCoreUnitTests.WorkflowTests
{
    public class AddTagWorkflowTests
    {
        private readonly Mock<ITagManagementAdapter> _tagManagementAdapter;

        private readonly string _imageId;

        private readonly string _tagName;
        
        public AddTagWorkflowTests()
        {
            _tagManagementAdapter = new Mock<ITagManagementAdapter>();

            _imageId = "123";
            _tagName = "tag333";
        }

        [Fact]
        public async Task SuccessfullyAddsTag()
        {
            // arrange
            var workflow = new AddTagWorkflow(_tagManagementAdapter.Object);
            var parameters = new ImageIdTagParameters(_imageId, _tagName);

            // act
            var result = await workflow.Run(parameters);

            // assert
            Assert.Equal(ResultState.Success, result.State);
            _tagManagementAdapter.Verify(x => x.AddTag(_imageId, _tagName), Times.Once());
        }

        [Fact]
        public async Task InvalidTagName()
        {
            // arrange
            var workflow = new AddTagWorkflow(_tagManagementAdapter.Object);
            var parameters = new ImageIdTagParameters(_imageId, null);

            // act
            var result = await workflow.Run(parameters);

            // assert
            Assert.Equal(ResultState.Error, result.State);
            Assert.Equal(ErrorCode.InvalidParameter, result.ErrorCode);
            _tagManagementAdapter.Verify(x => x.AddTag(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }


        [Fact]
        public async Task InvalidImageId()
        {
            // arrange
            var workflow = new AddTagWorkflow(_tagManagementAdapter.Object);
            var parameters = new ImageIdTagParameters(null, _tagName);

            // act
            var result = await workflow.Run(parameters);

            // assert
            Assert.Equal(ResultState.Error, result.State);
            Assert.Equal(ErrorCode.InvalidParameter, result.ErrorCode);
            _tagManagementAdapter.Verify(x => x.AddTag(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task TagFailsToBeAdded()
        {
            // arrange
            var tagManagementAdapter = new Mock<ITagManagementAdapter>();
            tagManagementAdapter.Setup(x => x.AddTag(It.IsAny<string>(), It.IsAny<string>())).ThrowsAsync(new WorkflowTestException());

            var workflow = new AddTagWorkflow(tagManagementAdapter.Object);
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
