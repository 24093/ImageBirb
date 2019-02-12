using ImageBirb.Core.Common;
using ImageBirb.Core.Ports.Secondary;
using ImageBirb.Core.Workflows;
using ImageBirb.Core.Workflows.Parameters;
using ImageBirb.Core.Workflows.Results;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImageBirb.Core.Ports.Secondary.DatabaseAdapter;
using Xunit;

namespace ImageBirbCoreUnitTests.WorkflowTests
{
    public class LoadThumbnailsWorkflowTests
    {
        private readonly IList<Image> _thumbnails;

        private readonly DatabaseAdapterMock _databaseAdapterMock;

        private Mock<IDatabaseAdapter> _databaseAdapter => _databaseAdapterMock.DatabaseAdapter;

        private Mock<IImageManagement> _imageManagement => _databaseAdapterMock.ImageManagement;

        public LoadThumbnailsWorkflowTests()
        {
            _thumbnails = new List<Image>
            {
                new Image {ImageId = "1", ThumbnailData = new byte[] {1, 2}, Tags = new List<string> {"a", "b"}},
                new Image {ImageId = "2", ThumbnailData = new byte[] {3, 4}, Tags = new List<string> {"a", "c"}},
                new Image {ImageId = "3", ThumbnailData = new byte[] {5, 6}, Tags = new List<string> {"b", "c"}}
            };

            _databaseAdapterMock = new DatabaseAdapterMock();
            _imageManagement.Setup(x => x.GetThumbnails(It.IsAny<IList<string>>())).ReturnsAsync(_thumbnails);
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
            _imageManagement.Verify(x => x.GetThumbnails(null), Times.Once());
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
            _imageManagement.Verify(x => x.GetThumbnails(It.IsAny<IList<string>>()), Times.Once());
        }

        [Fact]
        public async Task SuccessfullyLoadsAllThumbnailsWithFilter()
        {
            // arrange
            var databaseAdapterMock = new DatabaseAdapterMock();
            databaseAdapterMock.ImageManagement.Setup(x => x.GetThumbnails(null)).ThrowsAsync(new WorkflowTestException());

            var workflow = new LoadThumbnailsWorkflow(databaseAdapterMock.DatabaseAdapter.Object);
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
            var databaseAdapterMock = new DatabaseAdapterMock();
            databaseAdapterMock.ImageManagement.Setup(x => x.GetThumbnails(It.IsAny<IList<string>>())).ThrowsAsync(new WorkflowTestException());

            var workflow = new LoadThumbnailsWorkflow(databaseAdapterMock.DatabaseAdapter.Object);
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
