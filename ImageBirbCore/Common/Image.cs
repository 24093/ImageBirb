using LiteDB;
using System.Collections.Generic;
using System.Linq;

namespace ImageBirb.Core.Common
{
    public class Image
    {
        [BsonId]
        public string ImageId { get; set; }

        [BsonIgnore]
        public byte[] ImageData { get; set; }

        [BsonIgnore]
        public byte[] ThumbnailData { get; set; }

        public string Filename { get; set; }

        public List<string> Tags { get; set; } = new List<string>();

        public ImageStorageType ImageStorageType { get; set; }

        public string Fingerprint { get; set; }

        public bool Equals(Image image)
        {
            return image.ImageId == this.ImageId &&
                   image.ImageData.UnsafeCompare(this.ImageData) &&
                   image.ThumbnailData.UnsafeCompare(this.ThumbnailData) &&
                   image.Filename == this.Filename &&
                   image.Tags.SequenceEqual(this.Tags) &&
                   image.ImageStorageType == this.ImageStorageType &&
                   image.Fingerprint == this.Fingerprint;
        }
    }
}
