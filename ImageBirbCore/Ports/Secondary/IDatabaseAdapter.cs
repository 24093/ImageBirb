using ImageBirb.Core.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImageBirb.Core.Ports.Secondary
{
    public interface IDatabaseAdapter
    {
        string ConnectionString { get; }

        Task<string> CreateImageId();

        Task AddImage(Image image);

        Task RemoveImage(string imageId);

        Task<IList<Image>> GetThumbnails(IList<string> tagNames);

        Task<Image> GetImage(string imageId);
        
        Task AddTag(string imageId, string tagName);

        Task RemoveTag(string imageId, string tagName);

        Task<IList<Tag>> GetTags();
    }
}