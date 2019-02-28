using ImageBirb.Core.Common;
using ImageBirb.Core.Ports.Secondary;
using ImageBirb.Core.Workflows.Parameters;
using ImageBirb.Core.Workflows.Results;
using NLog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageBirb.Core.Workflows
{
    internal class AddImagesWorkflow : Workflow<AddImagesParameters, AddImagesResult>
    {
        private new static readonly Logger Logger = LogManager.GetCurrentClassLogger();

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

        protected override async Task<AddImagesResult> RunImpl(AddImagesParameters p)
        {
            return await Task.Run(async () =>
            {
                var fileNames = p.FileNames;

                // Read needed settings.
                var imageStorageType = (await _settingsManagementAdapter.GetSetting(SettingType.ImageStorage)).AsEnum<ImageStorageType>();
                var ignoreSimilarImages = (await _settingsManagementAdapter.GetSetting(SettingType.IgnoreSimilarImages)).AsBool();
                var similarityThreshold = (await _settingsManagementAdapter.GetSetting(SettingType.SimilarityThreshold)).AsDouble();

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
                IList<string> ignoredImageFileNames;

                if (fileNames.Count > 10)
                {
                    ignoredImageFileNames = AddImagesParallel(imageStorageType, ignoreSimilarImages, similarityThreshold, fileNames);
                }
                else
                {
                    ignoredImageFileNames = await AddImages(imageStorageType, ignoreSimilarImages, similarityThreshold, fileNames);
                }

                return new AddImagesResult(ResultState.Success, ignoredImageFileNames);
            });
        }

        /// <summary>
        /// Add images to the DB.
        /// </summary>
        /// <param name="imageStorageType">Specified image storage type.</param>
        /// <param name="ignoreSimilarImages">States if images that are similar to existing ones are ignored.</param>
        /// <param name="similarityThreshold">Threshold for similar images to be considered [0,1].</param>
        /// <param name="fileNames">List of image file names.</param>
        /// <returns>List of ignored files.</returns>
        private async Task<IList<string>> AddImages(ImageStorageType imageStorageType, bool ignoreSimilarImages, double similarityThreshold, IList<string> fileNames)
        {
            var i = 0;

            var ignoredImageFileNames = new List<string>();

            foreach (var fileName in fileNames)
            {
                var added = await AddImage(imageStorageType, ignoreSimilarImages, similarityThreshold, fileName);

                if (!added)
                {
                    ignoredImageFileNames.Add(fileName);
                }

                RaiseProgressChanged(ProgressType.ImageAdded, ++i, fileNames.Count);
            }

            return ignoredImageFileNames;
        }

        /// <summary>
        /// Add images to the DB.
        /// This is done in parallel to speed up adding of
        /// many images.
        /// </summary>
        /// <param name="imageStorageType">Specified image storage type.</param>
        /// <param name="ignoreSimilarImages">States if images that are similar to existing ones are ignored.</param>
        /// <param name="similarityThreshold">Threshold for similar images to be considered [0,1].</param>
        /// <param name="fileNames">List of image file names.</param>
        /// <returns>List of ignored files.</returns>
        private IList<string> AddImagesParallel(ImageStorageType imageStorageType, bool ignoreSimilarImages, double similarityThreshold, IList<string> fileNames)
        {
            var exceptions = new ConcurrentQueue<Exception>();
            var i = 0;
            var ignoredImageFileNames = new List<string>();

            Parallel.ForEach(fileNames, async fileName =>
            {
                try
                {
                    var added = await AddImage(imageStorageType, ignoreSimilarImages, similarityThreshold, fileName);

                    if (!added)
                    {
                        ignoredImageFileNames.Add(fileName);
                    }

                    RaiseProgressChanged(ProgressType.ImageAdded, ++i, fileNames.Count);
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

            return ignoredImageFileNames;
        }

        /// <summary>
        /// Add a single image to the DB.
        /// </summary>
        /// <param name="imageStorageType">Specified image storage type.</param>
        /// <param name="ignoreSimilarImages">States if images that are similar to existing ones are ignored.</param>
        /// <param name="similarityThreshold">Threshold for similar images to be considered [0,1].</param>
        /// <param name="filename">Filename of the image.</param>
        /// <returns>True if image was added, false if image was ignored.</returns>
        private async Task<bool> AddImage(ImageStorageType imageStorageType, bool ignoreSimilarImages, double similarityThreshold, string filename)
        {
            var imageId = await _imageManagementAdapter.CreateImageId();
            var imageData = await _fileSystemAdapter.ReadBinaryFile(filename);
            var thumbnailData = await _imagingAdapter.CreateThumbnail(imageData);

            var fingerprint = await _imagingAdapter.CreateFingerprint(imageData);

            // Check for similar images.
            if (ignoreSimilarImages)
            {
                var similarImages = await _imageManagementAdapter.GetSimilarImages(fingerprint,
                    _imagingAdapter.GetSimilarityScore, similarityThreshold);

                // If any similar images were found, ignore this image.
                if (similarImages.Any())
                {
                    Logger.Info("ignored image {0} from \"{1}\" because it is similar ({2}) to existing image (original source: \"{3}\")", 
                        imageId, filename, similarImages.First().SimilarityScore, similarImages.First().Image.Filename);
                    return false;
                }
            }

            // Add the image.
            var image = new Image
            {
                ImageId = imageId,
                ImageData = imageStorageType == ImageStorageType.CopyToDatabase ? imageData : null,
                ThumbnailData = thumbnailData,
                Filename = imageStorageType == ImageStorageType.LinkToSource ? filename : null,
                ImageStorageType = imageStorageType,
                Fingerprint = fingerprint
            };

            await _imageManagementAdapter.AddImage(image);
            return true;
        }
    }
}