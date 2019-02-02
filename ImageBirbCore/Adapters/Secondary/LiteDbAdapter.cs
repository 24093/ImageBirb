using ImageBirb.Core.Common;
using ImageBirb.Core.Ports.Secondary;
using LiteDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageBirb.Core.Adapters.Secondary
{
    public class LiteDbAdapter : IDatabaseAdapter, IDisposable
    {
        private const string FilePrefix = "$/images/";

        private const string ThumbnailPostfix = "_thumb";

        private readonly LiteDatabase _db;
        
        private readonly LiteCollection<Image> _imageCollection;

        public LiteDbAdapter(string databaseFilename)
        {
            _db = new LiteDatabase(databaseFilename);
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

        public async Task<IList<Image>> GetThumbnails(Predicate<Image> predicate = null)
        {
            return await Task.Run(() =>
            {
                IList<Image> allImages = _imageCollection.FindAll().ToList();
                IList<Image> images = new List<Image>();

                foreach (var image in allImages)
                {
                    if (predicate != null && !predicate(image))
                    {
                        continue;
                    }

                    using (var ms = new MemoryStream())
                    {
                        _db.FileStorage.Download(FilePrefix + image.ImageId + ThumbnailPostfix, ms);
                        image.ThumbnailData = ms.ToArray();
                        images.Add(image);
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

        public async Task AddTag(string imageId, string tagName)
        {
            await Task.Run(() =>
            {
                var image = _imageCollection.Find(doc => doc.ImageId == imageId).FirstOrDefault();

                if (image?.Tags.Contains(tagName) == false)
                {
                    image.Tags.Add(tagName);
                }

                _imageCollection.Update(image);
            });
        }

        public async Task RemoveTag(string imageId, string tagName)
        {
            await Task.Run(() =>
            {
                var image = _imageCollection.Find(doc => doc.ImageId == imageId).FirstOrDefault();

                if (image?.Tags.Contains(tagName) == true)
                {
                    image.Tags.Remove(tagName);
                }

                _imageCollection.Update(image);
            });
        }

        public async Task<IDictionary<string, int>> GetTags()
        {
            return await Task.Run(() =>
            {
                var dict = new Dictionary<string, int>();
                var images = _imageCollection.FindAll();

                foreach (var image in images)
                {
                    foreach (var tag in image.Tags)
                    {
                        if (!dict.ContainsKey(tag))
                        {
                            dict.Add(tag, 0);
                        }

                        dict[tag]++;
                    }
                }

                return dict;
            });
        }

        public void Dispose()
        {
            _db?.Dispose();
        }
    }
}
