namespace ImageBirb.Core.Workflows.Parameters
{
    public class ImageIdParameters : WorkflowParameters
    {
        public string ImageId { get; }

        public ImageIdParameters(string imageId)
        {
            ImageId = imageId;
        }
    }
}