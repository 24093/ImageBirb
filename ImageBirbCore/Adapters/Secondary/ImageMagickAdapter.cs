using ImageBirb.Core.Common;
using ImageBirb.Core.Ports.Secondary;
using ImageMagick;
using System.IO;
using System.Threading.Tasks;

namespace ImageBirb.Core.Adapters.Secondary
{
    internal class ImageMagickAdapter : IImagingAdapter
    {
        public async Task<byte[]> CreateThumbnail(byte[] imageData)
        {
            return await Task.Run(() =>
            {
                using (var image = new MagickImage(imageData))
                {
                    image.Resize(new MagickGeometry(Constants.ThumbnailWidth, Constants.ThumbnailHeight));

                    using (var ms = new MemoryStream())
                    {
                        image.Write(ms);
                        return ms.ToArray();
                    }
                }
            });
        }
    }

}