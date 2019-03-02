namespace ImageBirb.Core.BusinessObjects
{
    public class ImageSimilarity
    {
        public Image Image { get; }

        public double SimilarityScore { get; }

        public ImageSimilarity(Image image, double similarityScore)
        {
            Image = image;
            SimilarityScore = similarityScore;
        }
    }
}