namespace ImageBirb.Core.Workflows.Parameters
{
    internal class ImageIdTagParameters : WorkflowParameters
    {
        public string ImageId { get; }

        public string TagName { get; }

        public ImageIdTagParameters(string imageId,string tagName)
        {
            ImageId = imageId;
            TagName = tagName;
        }
    }
}