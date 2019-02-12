using ImageBirb.Core.Ports.Secondary.DatabaseAdapter;
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
        private readonly DatabaseAdapterMock _databaseAdapterMock;

        private Mock<IDatabaseAdapter> _databaseAdapter => _databaseAdapterMock.DatabaseAdapter;

        private Mock<ITagManagement> _tagManagement => _databaseAdapterMock.TagManagement;

        private readonly string _imageId;

        private readonly string _tagName;
        
        public AddTagWorkflowTests()
        {
            _databaseAdapterMock = new DatabaseAdapterMock();

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
            var result = await workflow.Run(parameters);

            // assert
            Assert.Equal(ResultState.Success, result.State);
            _tagManagement.Verify(x => x.AddTag(_imageId, _tagName), Times.Once());
        }

        [Fact]
        public async Task InvalidTagName()
        {
            // arrange
            var workflow = new AddTagWorkflow(_databaseAdapter.Object);
            var parameters = new ImageIdTagParameters(_imageId, null);

            // act
            var result = await workflow.Run(parameters);

            // assert
            Assert.Equal(ResultState.Error, result.State);
            Assert.Equal(ErrorCode.InvalidParameter, result.ErrorCode);
            _tagManagement.Verify(x => x.AddTag(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }


        [Fact]
        public async Task InvalidImageId()
        {
            // arrange
            var workflow = new AddTagWorkflow(_databaseAdapter.Object);
            var parameters = new ImageIdTagParameters(null, _tagName);

            // act
            var result = await workflow.Run(parameters);

            // assert
            Assert.Equal(ResultState.Error, result.State);
            Assert.Equal(ErrorCode.InvalidParameter, result.ErrorCode);
            _tagManagement.Verify(x => x.AddTag(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task TagFailsToBeAdded()
        {
            // arrange
            var databaseAdapterMock = new DatabaseAdapterMock();
            databaseAdapterMock.TagManagement.Setup(x => x.AddTag(It.IsAny<string>(), It.IsAny<string>())).ThrowsAsync(new WorkflowTestException());

            var workflow = new AddTagWorkflow(databaseAdapterMock.DatabaseAdapter.Object);
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
