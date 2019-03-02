using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageBirb.Core.BusinessObjects;
using ImageBirb.Core.Ports.Secondary;
using LiteDB;

namespace ImageBirb.Core.Adapters
{
    public class LiteDbTagManagementAdapter : ITagManagementAdapter
    {
        private readonly LiteCollection<Image> _imageCollection;

        public LiteDbTagManagementAdapter(LiteDbAdapter liteDbAdapter)
        {
            _imageCollection = liteDbAdapter.ImageCollection;
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
                // Fancy typed LINQ statement, that selects all tags
                // from all images and counts and sorts them.
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