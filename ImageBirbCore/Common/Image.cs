using LiteDB;
using System.Collections.Generic;
using System.Linq;

namespace ImageBirb.Core.Common
{
    public class Image
    {
        [BsonId]
        public int Id { get; set; }

        public string ImageId { get; set; }

        [BsonIgnore]
        public byte[] ImageData { get; set; }

        [BsonIgnore]
        public byte[] ThumbnailData { get; set; }

        public List<string> Tags { get; set; } = new List<string>();

        public bool Equals(Image image)
        {
            return image.ImageId == this.ImageId &&
                   image.ImageData.UnsafeCompare(this.ImageData) &&
                   image.ThumbnailData.UnsafeCompare(this.ThumbnailData) &&
                   image.Tags.SequenceEqual(this.Tags);
        }
    }
}
