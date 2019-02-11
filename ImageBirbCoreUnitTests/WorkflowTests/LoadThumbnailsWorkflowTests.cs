using ImageBirb.Core.Common;
using ImageBirb.Core.Ports.Secondary;
using ImageBirb.Core.Workflows;
using ImageBirb.Core.Workflows.Parameters;
using ImageBirb.Core.Workflows.Results;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ImageBirbCoreUnitTests.WorkflowTests
{
    public class LoadThumbnailsWorkflowTests
    {
        private readonly IList<Image> _thumbnails;

        private readonly Mock<IDatabaseAdapter> _databaseAdapter;

        public LoadThumbnailsWorkflowTests()
        {
            _thumbnails = new List<Image>
            {
                new Image {ImageId = "1", ThumbnailData = new byte[] {1, 2}, Tags = new List<string> {"a", "b"}},
                new Image {ImageId = "2", ThumbnailData = new byte[] {3, 4}, Tags = new List<string> {"a", "c"}},
                new Image {ImageId = "3", ThumbnailData = new byte[] {5, 6}, Tags = new List<string> {"b", "c"}}
            };

            _databaseAdapter = new Mock<IDatabaseAdapter>();
            _databaseAdapter.Setup(x => x.GetThumbnails(It.IsAny<IList<string>>())).ReturnsAsync(_thumbnails);
        }

        [Fact]
        public async Task SuccessfullyLoadsAllThumbnailsWithNullParameter()
        {
            // arrange
            var workflow = new LoadThumbnailsWorkflow(_databaseAdapter.Object);
            var parameters = new TagNamesParameters(null);

            // act
            var result = await workflow.Run(parameters);

            // assert
            Assert.Equal(ResultState.Success, result.State);
            Assert.Equal(3, result.Thumbnails.Count);
            _databaseAdapter.Verify(x => x.GetThumbnails(null), Times.Once());
        }

        [Fact]
        public async Task SuccessfullyLoadsAllThumbnailsWithEmptyParameter()
        {
            // arrange
            var workflow = new LoadThumbnailsWorkflow(_databaseAdapter.Object);
            var parameters = new TagNamesParameters(new List<string>());

            // act
            var result = await workflow.Run(parameters);

            // assert
            Assert.Equal(ResultState.Success, result.State);
            Assert.Equal(3, result.Thumbnails.Count);
            _databaseAdapter.Verify(x => x.GetThumbnails(It.IsAny<IList<string>>()), Times.Once());
        }

        [Fact]
        public async Task SuccessfullyLoadsAllThumbnailsWithFilter()
        {
            // arrange
            var databaseAdapter = new Mock<IDatabaseAdapter>();
            databaseAdapter.Setup(x => x.GetThumbnails(null)).ThrowsAsync(new WorkflowTestException());

            var workflow = new LoadThumbnailsWorkflow(databaseAdapter.Object);
            var parameters = new TagNamesParameters(new List<string> {"a", "b"});

            // act
            var result = await workflow.Run(parameters);

            // assert
            Assert.Equal(ResultState.Success, result.State);
        }

        [Fact]
        public async Task ThumbnailsFailToLoad()
        {
            // arrange
            var databaseAdapter = new Mock<IDatabaseAdapter>();
            databaseAdapter.Setup(x => x.GetThumbnails(It.IsAny<IList<string>>())).ThrowsAsync(new WorkflowTestException());

            var workflow = new LoadThumbnailsWorkflow(databaseAdapter.Object);
            var parameters = new TagNamesParameters(new List<string>());

            // act
            var result = await workflow.Run(parameters);

            // assert
            Assert.Equal(ResultState.Error, result.State);
            Assert.Equal(ErrorCode.WorkflowInternalError, result.ErrorCode);
            Assert.IsType<WorkflowTestException>(result.Exception);
        }
    }
}
