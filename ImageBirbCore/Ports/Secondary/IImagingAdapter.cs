using System.Threading.Tasks;
using ImageBirb.Core.Common;

namespace ImageBirb.Core.Ports.Secondary
{
    public interface IImagingAdapter
    {
        Task<byte[]> CreateThumbnail(byte[] imageData);

        Task<ImageFormat> GetImageFormat(byte[] imageData);
    }
}
