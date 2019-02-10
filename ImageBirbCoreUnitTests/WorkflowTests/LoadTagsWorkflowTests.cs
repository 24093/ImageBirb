using System.Collections.Generic;
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
    public class LoadTagsWorkflowTests
    {
        private readonly IList<Tag> _tags;

        private readonly Mock<IDatabaseAdapter> _databaseAdapter;

        public LoadTagsWorkflowTests()
        {
            _tags = new List<Tag>
            {
                new Tag("tag1", 1),
                new Tag("tag2", 59),
                new Tag("tag3", 100),
                new Tag("tag4", 25),
                new Tag("tag5", 50)
            };

            _databaseAdapter = new Mock<IDatabaseAdapter>();
            _databaseAdapter.Setup(x => x.GetTags()).ReturnsAsync(_tags);
        }

        [Fact]
        public async Task SuccessfullyLoadsTags()
        {
            // arrange
            var workflow = new LoadTagsWorkflow(_databaseAdapter.Object);

            // act
            var result = await workflow.RunWorkflow();

            // assert
            Assert.Equal(ResultState.Success, result.State);
            _databaseAdapter.Verify(x => x.GetTags(), Times.Once());
        }

        [Fact]
        public async Task TagsAreSortedByCount()
        {
            // arrange
            var workflow = new LoadTagsWorkflow(_databaseAdapter.Object);

            // act
            var result = await workflow.RunWorkflow();

            // assert
            Assert.Equal(ResultState.Success, result.State);
            _databaseAdapter.Verify(x => x.GetTags(), Times.Once());

            var tags = result.Tags;
            Assert.Equal(5, tags.Count);

            Assert.Equal("tag3", tags[0].Name);
            Assert.Equal(100, tags[0].Count);

            Assert.Equal("tag5", tags[1].Name);
            Assert.Equal(50, tags[1].Count);

            Assert.Equal("tag4", tags[2].Name);
            Assert.Equal(25, tags[2].Count);

            Assert.Equal("tag2", tags[3].Name);
            Assert.Equal(5, tags[3].Count);

            Assert.Equal("tag1", tags[4].Name);
            Assert.Equal(1, tags[4].Count);
        }

        [Fact]
        public async Task TagsFailToBeLoaded()
        {
            // arrange
            var databaseAdapter = new Mock<IDatabaseAdapter>();
            databaseAdapter.Setup(x => x.GetTags()).ThrowsAsync(new WorkflowTestException());

            var workflow = new LoadTagsWorkflow(databaseAdapter.Object);

            // act
            var result = await workflow.RunWorkflow();

            // assert
            Assert.Equal(ResultState.Error, result.State);
            Assert.Equal(ErrorCode.WorkflowInternalError, result.ErrorCode);
            Assert.IsType<WorkflowTestException>(result.Exception);
        }
    }
}
