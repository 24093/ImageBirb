using ImageBirb.Core.Common;
using ImageBirb.Core.Ports.Secondary;
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
        private readonly Mock<IDatabaseAdapter> _databaseAdapter;

        public LoadTagsWorkflowTests()
        {
            _databaseAdapter = new Mock<IDatabaseAdapter>();
            _databaseAdapter.Setup(x => x.GetTags()).ReturnsAsync(new List<Tag>());
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
            _databaseAdapter.Verify(x => x.GetTags(), Times.Once());
        }

        [Fact]
        public async Task TagsFailToBeLoaded()
        {
            // arrange
            var databaseAdapter = new Mock<IDatabaseAdapter>();
            databaseAdapter.Setup(x => x.GetTags()).ThrowsAsync(new WorkflowTestException());

            var workflow = new LoadTagsWorkflow(databaseAdapter.Object);

            // act
            var result = await workflow.Run();

            // assert
            Assert.Equal(ResultState.Error, result.State);
            Assert.Equal(ErrorCode.WorkflowInternalError, result.ErrorCode);
            Assert.IsType<WorkflowTestException>(result.Exception);
        }
    }
}
