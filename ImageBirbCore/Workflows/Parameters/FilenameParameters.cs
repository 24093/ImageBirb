namespace ImageBirb.Core.Workflows.Parameters
{
    internal class FilenameParameters : WorkflowParameters
    {
        public string Filename { get; }

        public FilenameParameters(string filename)
        {
            Filename = filename;
        }
    }
}