namespace ImageBirb.Core.Workflows.Parameters
{
    internal class KeyParameters : WorkflowParameters
    {
        public string Key { get; }


        public KeyParameters(string key)
        {
            Key = key;
        }
    }
}