using System.Threading.Tasks;
using ImageBirb.Core.Common;

namespace ImageBirb.Core.Ports.Secondary
{
    /// <summary>
    /// This adapter handles all image inspection and
    /// manipulation tasks.
    /// </summary>
    public interface IImagingAdapter
    {
        /// <summary>
        /// Create a thumbnail from an image.
        /// </summary>
        /// <param name="imageData">Full size image data.</param>
        /// <returns>Thumbnail image data.</returns>
        Task<byte[]> CreateThumbnail(byte[] imageData);

        /// <summary>
        /// Reads the image format from the image.
        /// </summary>
        /// <param name="imageData">Image data.</param>
        /// <returns>The image format or None if the format was not recognized.</returns>
        Task<ImageFormat> GetImageFormat(byte[] imageData);
    }
}
