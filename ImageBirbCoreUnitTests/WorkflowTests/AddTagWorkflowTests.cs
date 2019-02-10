using ImageBirb.Core.Ports.Secondary;
using ImageBirb.Core.Workflows;
using ImageBirb.Core.Workflows.Parameters;
using ImageBirb.Core.Workflows.Results;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace ImageBirbCoreUnitTests.WorkflowTests
{
    public class AddTagWorkflowTests
    {
        private readonly Mock<IDatabaseAdapter> _databaseAdapter;

        private readonly string _imageId;

        private readonly string _tagName;
        
        public AddTagWorkflowTests()
        {
            _databaseAdapter = new Mock<IDatabaseAdapter>();
            _imageId = "123";
            _tagName = "tag333";
        }

        [Fact]
        public async Task SuccessfullyAddsTag()
        {
            // arrange
            var workflow = new AddTagWorkflow(_databaseAdapter.Object);
            var parameters = new ImageIdTagParameters(_imageId, _tagName);

            // act
            var result = await workflow.RunWorkflow(parameters);

            // assert
            Assert.Equal(ResultState.Success, result.State);
            _databaseAdapter.Verify(x => x.AddTag(_imageId, _tagName), Times.Once());
        }

        [Fact]
        public async Task InvalidTagName()
        {
            // arrange
            var workflow = new AddTagWorkflow(_databaseAdapter.Object);
            var parameters = new ImageIdTagParameters(_imageId, null);

            // act
            var result = await workflow.RunWorkflow(parameters);

            // assert
            Assert.Equal(ResultState.Error, result.State);
            Assert.Equal(ErrorCode.InvalidParameter, result.ErrorCode);
            _databaseAdapter.Verify(x => x.AddTag(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }


        [Fact]
        public async Task InvalidImageId()
        {
            // arrange
            var workflow = new AddTagWorkflow(_databaseAdapter.Object);
            var parameters = new ImageIdTagParameters(null, _tagName);

            // act
            var result = await workflow.RunWorkflow(parameters);

            // assert
            Assert.Equal(ResultState.Error, result.State);
            Assert.Equal(ErrorCode.InvalidParameter, result.ErrorCode);
            _databaseAdapter.Verify(x => x.AddTag(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task TagFailsToBeAdded()
        {
            // arrange
            var databaseAdapter = new Mock<IDatabaseAdapter>();
            databaseAdapter.Setup(x => x.AddTag(It.IsAny<string>(), It.IsAny<string>())).ThrowsAsync(new WorkflowTestException());

            var workflow = new AddTagWorkflow(databaseAdapter.Object);
            var parameters = new ImageIdTagParameters(_imageId, _tagName);

            // act
            var result = await workflow.RunWorkflow(parameters);

            // assert
            Assert.Equal(ResultState.Error, result.State);
            Assert.Equal(ErrorCode.WorkflowInternalError, result.ErrorCode);
            Assert.IsType<WorkflowTestException>(result.Exception);
            _databaseAdapter.Verify(x => x.AddTag(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
    }
}
