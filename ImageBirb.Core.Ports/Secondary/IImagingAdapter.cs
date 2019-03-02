using System.Threading.Tasks;

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
        /// Generates a fingerprint of the image which can
        /// be used to compare it to others.
        /// </summary>
        /// <param name="imageData">Full size image data.</param>
        /// <returns>Fingerprint string.</returns>
        Task<string> CreateFingerprint(byte[] imageData);

        /// <summary>
        /// Function to calculate a similarity score for two fingerprints.
        /// </summary>
        Scoring.ScoreFunc GetSimilarityScore { get; }
    }
}
