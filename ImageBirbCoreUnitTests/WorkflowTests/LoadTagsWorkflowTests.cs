using ImageBirb.Core.Common;
using ImageBirb.Core.Workflows;
using ImageBirb.Core.Workflows.Results;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImageBirb.Core.Ports.Secondary;
using Xunit;

namespace ImageBirbCoreUnitTests.WorkflowTests
{
    public class LoadTagsWorkflowTests
    {
        private readonly Mock<ITagManagementAdapter> _tagManagementAdapter;

        public LoadTagsWorkflowTests()
        {
            _tagManagementAdapter = new Mock<ITagManagementAdapter>();
            _tagManagementAdapter.Setup(x => x.GetTags()).ReturnsAsync(new List<Tag>());
        }

        [Fact]
        public async Task SuccessfullyLoadsTags()
        {
            // arrange
            var workflow = new LoadTagsWorkflow(_tagManagementAdapter.Object);

            // act
            var result = await workflow.Run();

            // assert
            Assert.Equal(ResultState.Success, result.State);
            _tagManagementAdapter.Verify(x => x.GetTags(), Times.Once());
        }

        [Fact]
        public async Task TagsFailToBeLoaded()
        {
            // arrange
            var tagManagementAdapter = new Mock<ITagManagementAdapter>();
            tagManagementAdapter.Setup(x => x.GetTags()).ThrowsAsync(new WorkflowTestException());

            var workflow = new LoadTagsWorkflow(tagManagementAdapter.Object);

            // act
            var result = await workflow.Run();

            // assert
            Assert.Equal(ResultState.Failure, result.State);
            Assert.Equal(ErrorCode.WorkflowInternalError, result.ErrorCode);
            Assert.IsType<WorkflowTestException>(result.Exception);
        }
    }
}
