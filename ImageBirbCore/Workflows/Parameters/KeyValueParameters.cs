namespace ImageBirb.Core.Workflows.Parameters
{
    internal class KeyValueParameters : WorkflowParameters
    {
        public string Key { get; }

        public object Value { get; }

        public KeyValueParameters(string key, object value)
        {
            Key = key;
            Value = value;
        }
    }
}