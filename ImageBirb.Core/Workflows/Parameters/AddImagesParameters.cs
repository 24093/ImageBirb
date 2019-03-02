using ImageBirb.Core.BusinessObjects;
using System.Collections.Generic;

namespace ImageBirb.Core.Workflows.Parameters
{
    internal class AddImagesParameters : WorkflowParameters
    {
        public IList<string> FileNames { get; }

        public string Directory { get; }

        public bool AddFolder { get; }

        public ImageStorageType ImageStorageType { get; }

        public bool IgnoreSimilarImages { get; }

        public double SimilarityThreshold { get; }

        public AddImagesParameters(IList<string> fileNames, ImageStorageType imageStorageType, bool ignoreSimilarImages,
            double similarityThreshold)
            : this(imageStorageType, ignoreSimilarImages, similarityThreshold)
        {
            FileNames = fileNames;
            AddFolder = false;
        }

        public AddImagesParameters(string directory, ImageStorageType imageStorageType, bool ignoreSimilarImages,
            double similarityThreshold)
            : this(imageStorageType, ignoreSimilarImages, similarityThreshold)
        {
            Directory = directory;
            AddFolder = true;
        }

        private AddImagesParameters(ImageStorageType imageStorageType, bool ignoreSimilarImages,
            double similarityThreshold)
        {
            ImageStorageType = imageStorageType;
            IgnoreSimilarImages = ignoreSimilarImages;
            SimilarityThreshold = similarityThreshold;
        }
    }
}