using System.Threading.Tasks;
using ImageBirb.Core.Common;
using ImageBirb.Core.Ports.Secondary;
using ImageBirb.Core.Workflows.Results;

namespace ImageBirb.Core.Workflows
{
    public class ReadSettingsWorkflow : Workflow<SettingsResult>
    {
        private readonly IFileSystemAdapter _fileSystemAdapter;

        public ReadSettingsWorkflow(IFileSystemAdapter fileSystemAdapter)
        {
            _fileSystemAdapter = fileSystemAdapter;
        }

        protected override async Task<SettingsResult> Run()
        {
            var settings = await _fileSystemAdapter.ReadJsonFile<Settings>("ImageBirb.json");
            return new SettingsResult(ResultState.Success, settings);
        }
    }
}