using ImageBirb.Core.Common;
using ImageBirb.Core.Ports.Secondary.DatabaseAdapter;
using ImageBirb.Core.Workflows;
using ImageBirb.Core.Workflows.Results;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ImageBirbCoreUnitTests.WorkflowTests
{
    public class LoadTagsWorkflowTests
    {
        private readonly DatabaseAdapterMock _databaseAdapterMock;

        private Mock<IDatabaseAdapter> _databaseAdapter => _databaseAdapterMock.DatabaseAdapter;

        private Mock<ITagManagement> _tagManagement => _databaseAdapterMock.TagManagement;

        public LoadTagsWorkflowTests()
        {
            _databaseAdapterMock = new DatabaseAdapterMock();
            _tagManagement.Setup(x => x.GetTags()).ReturnsAsync(new List<Tag>());
        }

        [Fact]
        public async Task SuccessfullyLoadsTags()
        {
            // arrange
            var workflow = new LoadTagsWorkflow(_databaseAdapter.Object);

            // act
            var result = await workflow.Run();

            // assert
            Assert.Equal(ResultState.Success, result.State);
            _tagManagement.Verify(x => x.GetTags(), Times.Once());
        }

        [Fact]
        public async Task TagsFailToBeLoaded()
        {
            // arrange
            var databaseAdapterMock = new DatabaseAdapterMock();
            databaseAdapterMock.TagManagement.Setup(x => x.GetTags()).ThrowsAsync(new WorkflowTestException());

            var workflow = new LoadTagsWorkflow(databaseAdapterMock.DatabaseAdapter.Object);

            // act
            var result = await workflow.Run();

            // assert
            Assert.Equal(ResultState.Error, result.State);
            Assert.Equal(ErrorCode.WorkflowInternalError, result.ErrorCode);
            Assert.IsType<WorkflowTestException>(result.Exception);
        }
    }
}
