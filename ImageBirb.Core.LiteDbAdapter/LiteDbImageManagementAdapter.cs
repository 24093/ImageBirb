using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageBirb.Core.BusinessObjects;
using ImageBirb.Core.Ports.Secondary;
using LiteDB;

namespace ImageBirb.Core.Adapters
{
    public class LiteDbImageManagementAdapter : IImageManagementAdapter
    {
        private const string FilePrefix = "$/images/";

        private const string ThumbnailPostfix = "_thumb";

        private readonly LiteStorage _fileStorage;

        private readonly LiteCollection<Image> _imageCollection;

        public LiteDbImageManagementAdapter(LiteDbAdapter liteDbAdapter)
        {
            _fileStorage = liteDbAdapter.FileStorage;
            _imageCollection = liteDbAdapter.ImageCollection;
        }

        public async Task<string> CreateImageId()
        {
            return await Task.Run(() => Guid.NewGuid().ToString());
        }

        public async Task AddImage(Image image)
        {
            await Task.Run(() =>
            {
                var fileId = FilePrefix + image.ImageId;

                if (image.ImageStorageType == ImageStorageType.CopyToDatabase)
                {
                    using (var ms = new MemoryStream(image.ImageData))
                    {
                        _fileStorage.Upload(fileId, fileId, ms);
                    }
                }

                using (var ms = new MemoryStream(image.ThumbnailData))
                {
                    _fileStorage.Upload(fileId + ThumbnailPostfix, fileId + ThumbnailPostfix, ms);
                }

                _imageCollection.Insert(image);
            });
        }

        public async Task RemoveImage(string imageId)
        {
            await Task.Run(() =>
            {
                _imageCollection.Delete(doc => doc.ImageId == imageId);

                var fileId = FilePrefix + imageId;
                _fileStorage.Delete(fileId);
                _fileStorage.Delete(fileId + ThumbnailPostfix);
            });
        }

        public async Task<IList<Image>> GetThumbnails(IList<string> tagNames)
        {
            return await Task.Run(() =>
            {
                IList<Image> images;

                var doFiltering = tagNames != null && tagNames.Any();

                // Retrieve the image objects.
                if (doFiltering)
                {
                    images = _imageCollection.Find(doc => doc.Tags.Intersect(tagNames).Any()).ToList();
                }
                else
                {
                    images = _imageCollection.FindAll().ToList();
                }

                // Add thumbnail data to images.
                foreach (var image in images)
                {
                    using (var ms = new MemoryStream())
                    {
                        _fileStorage.Download(FilePrefix + image.ImageId + ThumbnailPostfix, ms);
                        image.ThumbnailData = ms.ToArray();
                    }
                }

                return images;
            });
        }

        public async Task<Image> GetImage(string imageId)
        {
            return await Task.Run(() =>
            {
                var image = _imageCollection.Find(i => i.ImageId == imageId).FirstOrDefault();

                if (image != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        _fileStorage.Download(FilePrefix + image.ImageId + ThumbnailPostfix, ms);
                        image.ThumbnailData = ms.ToArray();
                    }

                    if (image.ImageStorageType == ImageStorageType.CopyToDatabase)
                    {
                        using (var ms = new MemoryStream())
                        {
                            _fileStorage.Download(FilePrefix + imageId, ms);
                            image.ImageData = ms.ToArray();
                        }
                    }
                }

                return image;
            });
        }

        public async Task<IList<ImageSimilarity>> GetSimilarImages(string fingerprint, Scoring.ScoreFunc scoreFunc, double threshold)
        {
            return await Task.Run(async () =>
            {
                var images = _imageCollection.FindAll();
                var imageSimilarities = new List<ImageSimilarity>();

                foreach (var image in images)
                {
                    var score = await scoreFunc(fingerprint, image.Fingerprint);

                    if (score > threshold)
                    {
                        imageSimilarities.Add(new ImageSimilarity(image, score));
                    }
                }

                return imageSimilarities;
            });
        }
    }
}