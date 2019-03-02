using ImageBirb.Core.Adapters;
using ImageBirb.Core.BusinessObjects;
using ImageBirb.Core.Workflows;
using ImageBirb.Core.Workflows.Parameters;
using ImageBirb.Core.Workflows.Results;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ImageBirbCoreUnitTests.AdapterTests.Primary
{
    public class WorkflowAdapterTests
    {
        [Fact]
        public async Task AddImagesRunsAddImagesWorkflow()
        {
            // arrange
            var workflowHost = new Mock<IWorkflowHost>();
            var workflowAdapter = new WorkflowAdapter(workflowHost.Object);
            
            // act
            await workflowAdapter.AddImages(new List<string> {"123.bmp"}, ImageStorageType.LinkToSource, true, 0.9);

            // assert
            workflowHost.Verify(
                x => x.Run<AddImagesWorkflow, AddImagesParameters, AddImagesResult>(
                    It.Is<AddImagesParameters>(y =>
                        y.FileNames[0] == "123.bmp" && y.AddFolder == false &&
                        y.ImageStorageType == ImageStorageType.LinkToSource && y.IgnoreSimilarImages == true &&
                        y.SimilarityThreshold == 0.9)),
                Times.Once);
        }

        [Fact]
        public async Task AddImagesWithDirectoryParamterRunsAddImagesWorkflow()
        {
            // arrange
            var workflowHost = new Mock<IWorkflowHost>();
            var workflowAdapter = new WorkflowAdapter(workflowHost.Object);

            // act
            await workflowAdapter.AddImages(@"C:\Images", ImageStorageType.LinkToSource, true, 0.9);

            // assert
            workflowHost.Verify(
                x => x.Run<AddImagesWorkflow, AddImagesParameters, AddImagesResult>(
                    It.Is<AddImagesParameters>(y => 
                        y.Directory == @"C:\Images" && y.AddFolder == true &&
                        y.ImageStorageType == ImageStorageType.LinkToSource && y.IgnoreSimilarImages == true &&
                        y.SimilarityThreshold == 0.9)),
                Times.Once);
        }

        [Fact]
        public async Task RemoveImageRunsRemoveImageWorkflow()
        {
            // arrange
            var workflowHost = new Mock<IWorkflowHost>();
            var workflowAdapter = new WorkflowAdapter(workflowHost.Object);

            // act
            await workflowAdapter.RemoveImage("555");

            // assert
            workflowHost.Verify(
                x => x.Run<RemoveImageWorkflow, ImageIdParameters, WorkflowResult>(
                    It.Is<ImageIdParameters>(y => y.ImageId == "555")), 
                Times.Once);
        }

        [Fact]
        public async Task LoadImageRunsLoadImageWorkflow()
        {
            // arrange
            var workflowHost = new Mock<IWorkflowHost>();
            var workflowAdapter = new WorkflowAdapter(workflowHost.Object);

            // act
            await workflowAdapter.LoadImage("567");

            // assert
            workflowHost.Verify(
                x => x.Run<LoadImageWorkflow, ImageIdParameters, ImageResult>(
                    It.Is<ImageIdParameters>(y => y.ImageId == "567")), 
                Times.Once);
        }
        
        [Fact]
        public async Task AddTagRunsAddTagWorkflow()
        {
            // arrange
            var workflowHost = new Mock<IWorkflowHost>();
            var workflowAdapter = new WorkflowAdapter(workflowHost.Object);

            // act
            await workflowAdapter.AddTag("12345", "taggg");

            // assert
            workflowHost.Verify(
                x => x.Run<AddTagWorkflow, ImageIdTagParameters, WorkflowResult>(
                    It.Is<ImageIdTagParameters>(y => y.ImageId == "12345" && y.TagName == "taggg")), 
                Times.Once);
        }

        [Fact]
        public async Task RemoveTagRunsRemoveTagWorkflow()
        {
            // arrange
            var workflowHost = new Mock<IWorkflowHost>();
            var workflowAdapter = new WorkflowAdapter(workflowHost.Object);

            // act
            await workflowAdapter.RemoveTag("55555", "tag123");

            // assert
            workflowHost.Verify(
                x => x.Run<RemoveTagWorkflow, ImageIdTagParameters, WorkflowResult>(
                    It.Is<ImageIdTagParameters>(y => y.ImageId == "55555" && y.TagName == "tag123")), 
                Times.Once);
        }

        [Fact]
        public async Task LoadTagsRunsLoadTagsWorkflow()
        {
            // arrange
            var workflowHost = new Mock<IWorkflowHost>();
            var workflowAdapter = new WorkflowAdapter(workflowHost.Object);

            // act
            await workflowAdapter.LoadTags();

            // assert
            workflowHost.Verify(x => x.Run<LoadTagsWorkflow, TagsResult>(), Times.Once);
        }

        [Fact]
        public async Task LoadThumbnailsRunsLoadThumbnailsWorkflow()
        {
            // arrange
            var workflowHost = new Mock<IWorkflowHost>();
            var workflowAdapter = new WorkflowAdapter(workflowHost.Object);

            // act
            await workflowAdapter.LoadThumbnails(new List<string> {"a", "b", "c"});

            // assert
            workflowHost.Verify(
                x => x.Run<LoadThumbnailsWorkflow, TagNamesParameters, ThumbnailsResult>(
                    It.Is<TagNamesParameters>(y =>
                        y.TagNames[0] == "a" && y.TagNames[1] == "b" && y.TagNames[2] == "c")), 
                Times.Once);
        }

        [Fact]
        public async Task ReadConnectionStringRunsReadConnectionStringWorkflow()
        {
            // arrange
            var workflowHost = new Mock<IWorkflowHost>();
            var workflowAdapter = new WorkflowAdapter(workflowHost.Object);

            // act
            await workflowAdapter.ReadConnectionString();

            // assert
            workflowHost.Verify(x => x.Run<ReadConnectionStringWorkflow, ConnectionStringResult>(), Times.Once);
        }

        [Fact]
        public async Task UpdateSettingsRunsUpdateSettingWorkflow()
        {
            // arrange
            var workflowHost = new Mock<IWorkflowHost>();
            var workflowAdapter = new WorkflowAdapter(workflowHost.Object);

            // act
            await workflowAdapter.UpdateSetting("xy", "123");

            // assert
            workflowHost.Verify(x => x.Run<UpdateSettingWorkflow, SettingParameters, WorkflowResult>(
                It.Is<SettingParameters>(y => y.Setting.Key == "xy" && y.Setting.Value == "123")), Times.Once);
        }

        [Fact]
        public async Task ReadSettingRunsReadSettingWorkflow()
        {
            // arrange
            var workflowHost = new Mock<IWorkflowHost>();
            var workflowAdapter = new WorkflowAdapter(workflowHost.Object);

            // act
            await workflowAdapter.ReadSetting("xy", "1.0");

            // assert
            workflowHost.Verify(x => x.Run<ReadSettingWorkflow, SettingParameters, SettingResult>(
                It.Is<SettingParameters>(y => y.Setting.Key == "xy" && y.Setting.Value == "1.0")), Times.Once);
        }
    }
}
