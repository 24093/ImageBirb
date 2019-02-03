using System.IO;
using System.Threading.Tasks;
using ImageBirb.Core.Common;
using ImageBirb.Core.Ports.Secondary;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
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

                var resizeOptions = new ResizeOptions
                {
                    Mode = ResizeMode.Pad,
                    Size = new Size(Constants.ThumbnailWidth, Constants.ThumbnailHeight)
                };

                image.Mutate(x => x.Resize(resizeOptions));

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