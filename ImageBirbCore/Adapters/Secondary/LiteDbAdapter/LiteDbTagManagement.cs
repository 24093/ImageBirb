using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageBirb.Core.Common;
using ImageBirb.Core.Ports.Secondary.DatabaseAdapter;
using LiteDB;

namespace ImageBirb.Core.Adapters.Secondary.LiteDbAdapter
{
    internal class LiteDbTagManagement : ITagManagement
    {
        private readonly LiteCollection<Image> _imageCollection;

        public LiteDbTagManagement(LiteDatabase liteDatabase)
        {
            _imageCollection = liteDatabase.GetCollection<Image>();
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

        public async Task<IList<Tag>> GetTags()
        {
            return await Task.Run(() =>
            {
                return _imageCollection.FindAll()
                    .SelectMany(x => x.Tags)
                    .GroupBy(x => x)
                    .Select(x => new Tag(x.Key, x.Count()))
                    .OrderByDescending(x => x.Count)
                    .ToList();
            });
        }
    }
}