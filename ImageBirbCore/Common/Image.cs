using LiteDB;
using System.Collections.Generic;

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
    }
}
