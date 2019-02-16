using ImageBirb.Core.Common;
using ImageBirb.Core.Ports.Secondary;
using ImageBirb.Core.Workflows.Parameters;
using ImageBirb.Core.Workflows.Results;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImageBirb.Core.Workflows
{
    internal class AddImagesWorkflow : Workflow<AddImagesParameters, WorkflowResult>
    {
        private readonly IImageManagementAdapter _imageManagementAdapter;
        private readonly IFileSystemAdapter _fileSystemAdapter;
        private readonly IImagingAdapter _imagingAdapter;
        private readonly ISettingsManagementAdapter _settingsManagementAdapter;

        public AddImagesWorkflow(IImageManagementAdapter imageManagementAdapter, IFileSystemAdapter fileSystemAdapter,
            IImagingAdapter imagingAdapter, ISettingsManagementAdapter settingsManagementAdapter)
        {
            _imageManagementAdapter = imageManagementAdapter;
            _fileSystemAdapter = fileSystemAdapter;
            _imagingAdapter = imagingAdapter;
            _settingsManagementAdapter = settingsManagementAdapter;
        }

        protected override async Task<WorkflowResult> RunImpl(AddImagesParameters p)
        {
            return await Task.Run(async () =>
            {
                var fileNames = p.FileNames;

                // Read image storage type setting.
                var imageStorageType = await GetImageStorageType();
                
                // If a directory instead of file names was supplied, scan it for image files.
                if (p.AddFolder)
                {
                    fileNames = await _fileSystemAdapter.GetFilenamesFromDirectory(p.Directory, new List<string>
                    {
                        "bmp", "jpg", ".jpeg", "png"
                    });
                }

                // Add all files.
                // If the number of images is below a threshold, 
                // go without parallel working to save the overhead.
                if (fileNames.Count > 10)
                {
                    AddImagesParallel(imageStorageType, fileNames);
                }
                else
                {
                    foreach (var filename in fileNames)
                    {
                        await AddImage(imageStorageType, filename);
                    }
                }

                return new WorkflowResult(ResultState.Success);
            });
        }

        private async Task<ImageStorageType> GetImageStorageType()
        {
            return (await _settingsManagementAdapter.GetSetting(SettingType.ImageStorage))
                .AsEnum<ImageStorageType>();
        }

        /// <summary>
        /// Add images to the DB.
        /// This is done in parallel to speed up adding of
        /// many images.
        /// </summary>
        /// <param name="imageStorageType">Specified image storage type.</param>
        /// <param name="filenames">List of image file names.</param>
        private void AddImagesParallel(ImageStorageType imageStorageType, IList<string> filenames)
        {
            var exceptions = new ConcurrentQueue<Exception>();
            var i = 0;

            Parallel.ForEach(filenames, async filename =>
            {
                try
                {
                    await AddImage(imageStorageType, filename);
                    RaiseProgressChanged(ProgressType.ImageAdded, ++i, filenames.Count);
                }
                catch (Exception ex)
                {
                    exceptions.Enqueue(ex);
                }
            });
            
            if (exceptions.Count > 0)
            {
                throw new AggregateException(exceptions);
            }
        }

        /// <summary>
        /// Add a single image to the DB.
        /// </summary>
        /// <param name="imageStorageType">Specified image storage type.</param>
        /// <param name="filename">Filename of the image.</param>
        private async Task AddImage(ImageStorageType imageStorageType, string filename)
        {
            var imageId = await _imageManagementAdapter.CreateImageId();
            var imageData = await _fileSystemAdapter.ReadBinaryFile(filename);
            var thumbnailData = await _imagingAdapter.CreateThumbnail(imageData);

            var image = new Image
            {
                ImageId = imageId,
                ImageData = imageStorageType == ImageStorageType.CopyToDatabase ? imageData : null,
                ThumbnailData = thumbnailData,
                Filename = imageStorageType == ImageStorageType.LinkToSource ? filename : null,
                ImageStorageType = imageStorageType
            };

            await _imageManagementAdapter.AddImage(image);
        }
    }
}