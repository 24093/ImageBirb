using System;
using System.Collections.Generic;
using System.Linq;
using ImageBirb.Core.Ports.Secondary;
using ImageBirb.Core.Workflows;
using ImageBirb.Core.Workflows.Parameters;
using ImageBirb.Core.Workflows.Results;
using Moq;
using System.Threading.Tasks;
using ImageBirb.Core.Common;
using Xunit;

namespace ImageBirbCoreUnitTests.WorkflowTests
{
    public class AddImagesWorkflowTests
    {
        private readonly Image _image;

        private readonly Mock<IImageManagementAdapter> _imageManagementAdapter;
        
        private readonly Mock<IFileSystemAdapter> _fileSystemAdapter;

        private readonly Mock<IImagingAdapter> _imagingAdapter;

        private readonly Mock<ISettingsManagementAdapter> _settingsManagementAdapter;

        public AddImagesWorkflowTests()
        {
            _image = new Image
            {
                ImageId = "123",
                ImageData = null,
                ThumbnailData = new byte[] {3, 4},
                Filename = "kuckuck.jpg",
                ImageStorageType = ImageStorageType.LinkToSource
            };

            _imageManagementAdapter = new Mock<IImageManagementAdapter>();
            _imageManagementAdapter.Setup(x => x.CreateImageId()).ReturnsAsync(_image.ImageId);
            _imageManagementAdapter.Setup(x => x.AddImage(It.IsAny<Image>())).Returns(Task.CompletedTask);

            _fileSystemAdapter = new Mock<IFileSystemAdapter>();

            _imagingAdapter = new Mock<IImagingAdapter>();
            _imagingAdapter.Setup(x => x.CreateThumbnail(It.IsAny<byte[]>())).ReturnsAsync(_image.ThumbnailData);

            _settingsManagementAdapter = new Mock<ISettingsManagementAdapter>();
            _settingsManagementAdapter.Setup(x => x.GetSetting(SettingType.ImageStorage))
                .ReturnsAsync(new Setting {Key = SettingType.ImageStorage.ToString(), Value = ImageStorageType.LinkToSource.ToString()});
        }

        [Fact]
        public async Task SuccessfullyAddsImage()
        {
            // arrange
            var workflow = new AddImagesWorkflow(_imageManagementAdapter.Object, _fileSystemAdapter.Object, _imagingAdapter.Object, _settingsManagementAdapter.Object);
            var parameters = new AddImagesParameters(new List<string> {_image.Filename});

            // act
            var result = await workflow.Run(parameters);

            // assert
            Assert.Equal(ResultState.Success, result.State);
            _imageManagementAdapter.Verify(x => x.AddImage(It.Is<Image>(i => i.Equals(_image))), Times.Once());
        }

        [Fact]
        public async Task ImageIdFailsToBeCreated()
        {
            // arrange
            var imageManagementAdapter = new Mock<IImageManagementAdapter>();
            imageManagementAdapter.Setup(x => x.CreateImageId()).ThrowsAsync(new WorkflowTestException());
            imageManagementAdapter.Setup(x => x.AddImage(It.IsAny<Image>())).Returns(Task.CompletedTask);

            var workflow = new AddImagesWorkflow(imageManagementAdapter.Object, _fileSystemAdapter.Object,
                _imagingAdapter.Object, _settingsManagementAdapter.Object);
            var parameters = new AddImagesParameters(new List<string> {_image.Filename});

            // act
            var result = await workflow.Run(parameters);

            // assert
            Assert.Equal(ResultState.Error, result.State);
            Assert.Equal(ErrorCode.WorkflowInternalError, result.ErrorCode);
            Assert.IsType<WorkflowTestException>(result.Exception);
            imageManagementAdapter.Verify(x => x.AddImage(It.IsAny<Image>()), Times.Never);
        }

        [Fact]
        public async Task FileFailsToBeRead()
        {
            // arrange
            var fileSystemAdapter = new Mock<IFileSystemAdapter>();
            fileSystemAdapter.Setup(x => x.ReadBinaryFile(It.IsAny<string>())).ThrowsAsync(new WorkflowTestException());

            var workflow = new AddImagesWorkflow(_imageManagementAdapter.Object, fileSystemAdapter.Object,
                _imagingAdapter.Object, _settingsManagementAdapter.Object);
            var parameters = new AddImagesParameters(new List<string> {_image.Filename});

            // act
            var result = await workflow.Run(parameters);

            // assert
            Assert.Equal(ResultState.Error, result.State);
            Assert.Equal(ErrorCode.WorkflowInternalError, result.ErrorCode);
            Assert.IsType<WorkflowTestException>(result.Exception);
            _imageManagementAdapter.Verify(x => x.AddImage(It.IsAny<Image>()), Times.Never);
        }

        [Fact]
        public async Task ThumbnailFailsToBeCreated()
        {
            // arrange
            var imagingAdapter = new Mock<IImagingAdapter>();
            imagingAdapter.Setup(x => x.CreateThumbnail(It.IsAny<byte[]>())).ThrowsAsync(new WorkflowTestException());

            var workflow = new AddImagesWorkflow(_imageManagementAdapter.Object, _fileSystemAdapter.Object,
                imagingAdapter.Object, _settingsManagementAdapter.Object);
            var parameters = new AddImagesParameters(new List<string> {_image.Filename});

            // act
            var result = await workflow.Run(parameters);

            // assert
            Assert.Equal(ResultState.Error, result.State);
            Assert.Equal(ErrorCode.WorkflowInternalError, result.ErrorCode);
            Assert.IsType<WorkflowTestException>(result.Exception);
            _imageManagementAdapter.Verify(x => x.AddImage(It.IsAny<Image>()), Times.Never);
        }
    }
}