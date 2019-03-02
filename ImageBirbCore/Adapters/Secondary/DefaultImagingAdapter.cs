using ImageBirb.Core.BusinessObjects;
using ImageBirb.Core.Ports.Secondary;
using ImageMagick;
using Shipwreck.Phash;
using Shipwreck.Phash.Bitmaps;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ImageBirb.Core.Adapters.Secondary
{
    internal class DefaultImagingAdapter : IImagingAdapter
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

        public async Task<string> CreateFingerprint(byte[] imageData)
        {
            return await Task.Run(() =>
            {
                using (var ms = new MemoryStream(imageData))
                {
                    var bitmap = (Bitmap) System.Drawing.Image.FromStream(ms);
                    var digest = ImagePhash.ComputeDigest(bitmap.ToLuminanceImage());

                    return Encoding.UTF8.GetString(digest.Coefficents);
                }
            });
        }

        public Scoring.ScoreFunc GetSimilarityScore => async (fingerprint1, fingerprint2) =>
        {
            return await Task.Run(() => ImagePhash.GetCrossCorrelation(
                Encoding.UTF8.GetBytes(fingerprint1),
                Encoding.UTF8.GetBytes(fingerprint2)));
        };
    }
}