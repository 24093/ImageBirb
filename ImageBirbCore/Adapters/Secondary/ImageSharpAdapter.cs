using System.IO;
using System.Threading.Tasks;
using ImageBirb.Core.Common;
using ImageBirb.Core.Ports.Secondary;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Image = SixLabors.ImageSharp.Image;

namespace ImageBirb.Core.Adapters.Secondary
{
    internal class ImageSharpAdapter : IImagingAdapter
    {
        public async Task<byte[]> CreateThumbnail(byte[] imageData)
        {
            return await Task.Run(() =>
            {
                var format = Image.DetectFormat(imageData);
                var image = Image.Load(imageData);

                image.Mutate(x => x.Resize(Constants.ThumbnailWidth, Constants.ThumbnailHeight));

                using (var ms = new MemoryStream())
                {
                    image.Save(ms, format);
                    return ms.ToArray();
                }
            });
        }

        public async Task<ImageFormat> GetImageFormat(byte[] imageData)
        {
            return await Task.Run(() =>
            {
                var format = Image.DetectFormat(imageData);

                switch (format.Name)
                {
                    case "JPEG":
                        return ImageFormat.Jpeg;
                    case "PNG":
                        return ImageFormat.Png;
                    case "BMP":
                        return ImageFormat.Bmp;
                    default:
                        return ImageFormat.None;
                }
            });
        }
    }
}