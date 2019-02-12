using ImageBirb.Core.Ports.Secondary.DatabaseAdapter;
using Moq;

namespace ImageBirbCoreUnitTests.WorkflowTests
{
    public class DatabaseAdapterMock
    {
        public Mock<IDatabaseAdapter> DatabaseAdapter;

        public Mock<IImageManagement> ImageManagement;

        public Mock<ITagManagement> TagManagement;

        public DatabaseAdapterMock()
        {
            DatabaseAdapter = new Mock<IDatabaseAdapter>();
            ImageManagement = new Mock<IImageManagement>();
            TagManagement = new Mock<ITagManagement>();

            DatabaseAdapter.SetupGet(x => x.ImageManagement).Returns(ImageManagement.Object);
            DatabaseAdapter.SetupGet(x => x.TagManagement).Returns(TagManagement.Object);
        }
    }
}