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
    public class AddImageWorkflowTests
    {
        [Fact]
        public async Task AddImage()
        {
            // arrange
            var image = new Image
            {
                ImageId = "03c1d6a6-02ef-4b36-a64f-19b890134522",
                ImageData = new byte[] { 1, 2 },
                ThumbnailData = new byte[] { 3, 4 }
            };

            var databaseAdapter = new Mock<IDatabaseAdapter>();
            databaseAdapter.Setup(x => x.CreateImageId()).ReturnsAsync(image.ImageId);
            databaseAdapter.Setup(x => x.AddImage(It.IsAny<Image>())).Returns(Task.CompletedTask);

            var fileSystemAdapter = new Mock<IFileSystemAdapter>();
            fileSystemAdapter.Setup(x => x.ReadBinaryFile(It.IsAny<string>())).ReturnsAsync(image.ImageData);

            var imagingAdapter = new Mock<IImagingAdapter>();
            imagingAdapter.Setup(x => x.CreateThumbnail(It.IsAny<byte[]>())).ReturnsAsync(image.ThumbnailData);

            var workflow = new AddImageWorkflow(databaseAdapter.Object, fileSystemAdapter.Object, imagingAdapter.Object);

            var parameters = new FilenameParameters("kuckuck.jpg");

            // act
            var result = await workflow.RunWorkflow(parameters);

            // assert
            Assert.Equal(ResultState.Success, result.State);
            databaseAdapter.Verify(x => x.AddImage(It.Is<Image>(i => i.Equals(image))), Times.Once());
        }
    }
}
