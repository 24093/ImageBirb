using ImageBirb.Core.Workflows.Parameters;
using ImageBirb.Core.Workflows.Results;
using System;
using System.Reflection;
using System.Threading.Tasks;
using ImageBirb.Core.Common;
using ImageBirb.Core.Ports.Secondary;

namespace ImageBirb.Core.Workflows
{
    public class UpdateSettingWorkflow : Workflow<KeyValueParameters, WorkflowResult>
    {
        private readonly IFileSystemAdapter _fileSystemAdapter;

        public UpdateSettingWorkflow(IFileSystemAdapter fileSystemAdapter)
        {
            _fileSystemAdapter = fileSystemAdapter;
        }

        protected override async Task<WorkflowResult> Run(KeyValueParameters p)
        {
            const string configFilename = "ImageBirb.json";

            var settings = await _fileSystemAdapter.ReadJsonFile<Settings>(configFilename);
            var property = typeof(Settings).GetProperty(p.Key, BindingFlags.Public);
            
            if (property == null)
            {
                return new WorkflowResult(ResultState.Error, ErrorCode.InvalidParameter);
            }

            var propertyType = property.PropertyType;

            if (propertyType != p.Value.GetType())
            {
                return new WorkflowResult(ResultState.Error, ErrorCode.InvalidParameter);
            }

            property.SetValue(settings, p.Value);
            await _fileSystemAdapter.WriteJsonFile(configFilename, settings);

            return new WorkflowResult(ResultState.Success);
        }
    }
}
