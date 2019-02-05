namespace ImageBirb.Core.Workflows.Parameters
{
    public class KeyValueParameters : WorkflowParameters
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