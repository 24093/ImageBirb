namespace ImageBirb.Core.Workflows.Parameters
{
    public class FilenameParameters : WorkflowParameters
    {
        public string Filename { get; }

        public FilenameParameters(string filename)
        {
            Filename = filename;
        }
    }
}