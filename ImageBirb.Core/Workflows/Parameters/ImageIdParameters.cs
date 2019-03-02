namespace ImageBirb.Core.Workflows.Parameters
{
    internal class ImageIdParameters : WorkflowParameters
    {
        public string ImageId { get; }

        public ImageIdParameters(string imageId)
        {
            ImageId = imageId;
        }
    }
}