﻿using System.Collections.Generic;
using System.Linq;

namespace ImageBirb.Core.BusinessObjects
{
    public class Image
    {
        public string ImageId { get; set; }

        public byte[] ImageData { get; set; }

        public byte[] ThumbnailData { get; set; }

        public string Filename { get; set; }

        public List<string> Tags { get; set; } = new List<string>();

        public ImageStorageType ImageStorageType { get; set; }

        public string Fingerprint { get; set; }

        public bool Equals(Image image)
        {
            return image.ImageId == this.ImageId &&
                   UnsafeCompare(image.ImageData, this.ImageData) &&
                   UnsafeCompare(image.ThumbnailData, this.ThumbnailData) &&
                   image.Filename == this.Filename &&
                   image.Tags.SequenceEqual(this.Tags) &&
                   image.ImageStorageType == this.ImageStorageType &&
                   image.Fingerprint == this.Fingerprint;
        }

        /// <summary>
        /// Compare two byte arrays.
        /// Claimed to be super fast.
        /// <see cref="https://stackoverflow.com/a/8808245"/>
        /// </summary>
        /// <param name="a1">First byte array.</param>
        /// <param name="a2">Second byte array.</param>
        /// <returns>True if both arrays are equal.</returns>
        private static unsafe bool UnsafeCompare(byte[] a1, byte[] a2)
        {
            if (a1 == a2)
            {
                return true;
            }

            if (a1 == null || a2 == null || a1.Length != a2.Length)
            {
                return false;
            }

            fixed (byte* p1 = a1, p2 = a2)
            {
                byte* x1 = p1, x2 = p2;
                int l = a1.Length;

                for (int i = 0; i < l / 8; i++, x1 += 8, x2 += 8)
                {
                    if (*((long*)x1) != *((long*)x2))
                    {
                        return false;
                    }
                }

                if ((l & 4) != 0)
                {
                    if (*((int*)x1) != *((int*)x2))
                    {
                        return false;
                    }

                    x1 += 4; x2 += 4;
                }

                if ((l & 2) != 0)
                {
                    if (*((short*)x1) != *((short*)x2))
                    {
                        return false;
                    }

                    x1 += 2; x2 += 2;
                }

                if ((l & 1) != 0)
                {
                    if (*((byte*)x1) != *((byte*)x2))
                    {
                        return false;
                    }
                }

                return true;
            }
        }
    }
}
