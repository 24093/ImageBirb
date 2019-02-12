using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageBirb.Core.Common;
using ImageBirb.Core.Ports.Secondary.DatabaseAdapter;
using LiteDB;

namespace ImageBirb.Core.Adapters.Secondary.LiteDbAdapter
{
    internal class LiteDbImageManagement : IImageManagement
    {
        private const string FilePrefix = "$/images/";

        private const string ThumbnailPostfix = "_thumb";

        private readonly LiteDatabase _db;

        private readonly LiteCollection<Image> _imageCollection;

        public LiteDbImageManagement(LiteDatabase liteDatabase)
        {
            _db = liteDatabase;
            _imageCollection = _db.GetCollection<Image>();
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

                using (var ms = new MemoryStream(image.ImageData))
                {
                    _db.FileStorage.Upload(fileId, fileId, ms);
                }

                using (var ms = new MemoryStream(image.ThumbnailData))
                {
                    _db.FileStorage.Upload(fileId + ThumbnailPostfix, fileId + ThumbnailPostfix, ms);
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
                _db.FileStorage.Delete(fileId);
                _db.FileStorage.Delete(fileId + ThumbnailPostfix);
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
                        _db.FileStorage.Download(FilePrefix + image.ImageId + ThumbnailPostfix, ms);
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
                        _db.FileStorage.Download(FilePrefix + image.ImageId + ThumbnailPostfix, ms);
                        image.ThumbnailData = ms.ToArray();
                    }

                    using (var ms = new MemoryStream())
                    {
                        _db.FileStorage.Download(FilePrefix + imageId, ms);
                        image.ImageData = ms.ToArray();
                    }
                }

                return image;
            });
        }
    }
}