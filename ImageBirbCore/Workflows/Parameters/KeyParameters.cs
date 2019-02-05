namespace ImageBirb.Core.Workflows.Parameters
{
    public class KeyParameters : WorkflowParameters
    {
        public string Key { get; }

        public KeyParameters(string key)
        {
            Key = key;
        }
    }
}