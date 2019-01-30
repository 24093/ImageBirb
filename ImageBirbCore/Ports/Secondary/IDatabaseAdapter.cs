using System.Collections.Generic;
using System.Threading.Tasks;
using ImageBirb.Core.Common;

namespace ImageBirb.Core.Ports.Secondary
{
    public interface IDatabaseAdapter
    {
        Task<string> CreateImageId();

        Task AddImage(Image image);

        Task<IList<Image>> GetThumbnails();

        Task<Image> GetImage(string imageId);
        
        Task AddTag(string imageId, string tagName);

        Task RemoveTag(string imageId, string tagName);

        Task<IDictionary<string, int>> GetTags();
    }
}