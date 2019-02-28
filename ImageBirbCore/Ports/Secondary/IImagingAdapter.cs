using System.Threading.Tasks;

namespace ImageBirb.Core.Ports.Secondary
{
    /// <summary>
    /// This adapter handles all image inspection and
    /// manipulation tasks.
    /// </summary>
    internal interface IImagingAdapter
    {
        /// <summary>
        /// Create a thumbnail from an image.
        /// </summary>
        /// <param name="imageData">Full size image data.</param>
        /// <returns>Thumbnail image data.</returns>
        Task<byte[]> CreateThumbnail(byte[] imageData);

        /// <summary>
        /// Generates a fingerprint of the image which can
        /// be used to compare it to others.
        /// </summary>
        /// <param name="imageData">Full size image data.</param>
        /// <returns>Fingerprint string.</returns>
        Task<string> CreateFingerprint(byte[] imageData);

        /// <summary>
        /// Calculates a similarity score for two fingerprints.
        /// </summary>
        /// <param name="fingerprint1">First fingerprint.</param>
        /// <param name="fingerprint2">Second fingerprint.</param>
        /// <returns>Similarity score in [0,1]</returns>
        Task<double> GetSimilarityScore(string fingerprint1, string fingerprint2);
    }
}
