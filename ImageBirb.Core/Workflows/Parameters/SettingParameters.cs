using ImageBirb.Core.BusinessObjects;

namespace ImageBirb.Core.Workflows.Parameters
{
    internal class SettingParameters : WorkflowParameters
    {
        public Setting Setting { get; }

        public SettingParameters(string key, string value)
        {
            Setting = new Setting
            {
                Key = key,
                Value = value
            };
        }
    }
}