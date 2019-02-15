using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using ImageBirb.Core.Common;
using ImageBirb.Core.Ports.Secondary;
using ImageBirb.Core.Workflows.Parameters;
using ImageBirb.Core.Workflows.Results;
using System.Threading.Tasks;

namespace ImageBirb.Core.Workflows
{
    internal class AddImagesWorkflow : Workflow<FilenamesParameters, WorkflowResult>
    {
        private readonly IImageManagementAdapter _imageManagementAdapter;
        private readonly IFileSystemAdapter _fileSystemAdapter;
        private readonly IImagingAdapter _imagingAdapter;

        public AddImagesWorkflow(IImageManagementAdapter imageManagementAdapter, IFileSystemAdapter fileSystemAdapter,
            IImagingAdapter imagingAdapter)
        {
            _imageManagementAdapter = imageManagementAdapter;
            _fileSystemAdapter = fileSystemAdapter;
            _imagingAdapter = imagingAdapter;
        }

        protected override async Task<WorkflowResult> RunImpl(FilenamesParameters p)
        {
            return await Task.Run(async () =>
            {
                var filenames = p.Filenames;

                // If a directory instead of filenames was supplied, scan it for image files.
                if (p.Filenames == null && !string.IsNullOrEmpty(p.Directory))
                {
                    filenames = await _fileSystemAdapter.GetFilenamesFromDirectory(p.Directory, new List<string>
                    {
                        "bmp", "jpg", ".jpeg", "png"
                    });
                }

                // Add all files.
                // If the number of images is below a threshold, 
                // go without parallel working to save the overhead.
                if (filenames.Count > 10)
                {
                    AddImagesParallel(filenames);
                }
                else
                {
                    foreach (var filename in filenames)
                    {
                        await AddImage(filename);
                    }
                }

                return new WorkflowResult(ResultState.Success);
            });
        }

        /// <summary>
        /// Add images to the DB.
        /// This is done in parallel to speed up adding of
        /// many images.
        /// </summary>
        /// <param name="filenames">List of image file names.</param>
        private void AddImagesParallel(IList<string> filenames)
        {
            var exceptions = new ConcurrentQueue<Exception>();
            var i = 0;

            Parallel.ForEach(filenames, async filename =>
            {
                try
                {
                    await AddImage(filename);
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
        /// <param name="filename">Filename of the image.</param>
        private async Task AddImage(string filename)
        {
            var imageId = await _imageManagementAdapter.CreateImageId();
            var imageData = await _fileSystemAdapter.ReadBinaryFile(filename);
            var thumbnailData = await _imagingAdapter.CreateThumbnail(imageData);

            var image = new Image
            {
                ImageId = imageId,
                ImageData = imageData,
                ThumbnailData = thumbnailData,
                Filename = filename
            };

            await _imageManagementAdapter.AddImage(image);
        }
    }
}