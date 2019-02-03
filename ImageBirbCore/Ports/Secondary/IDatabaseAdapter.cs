using ImageBirb.Core.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImageBirb.Core.Ports.Secondary
{
    public interface IDatabaseAdapter
    {
        Task<string> CreateImageId();

        Task AddImage(Image image);

        Task RemoveImage(string imageId);

        Task<IList<Image>> GetThumbnails(Predicate<Image> predicate = null);

        Task<Image> GetImage(string imageId);
        
        Task AddTag(string imageId, string tagName);

        Task RemoveTag(string imageId, string tagName);

        Task<IDictionary<string, int>> GetTags();
    }
}