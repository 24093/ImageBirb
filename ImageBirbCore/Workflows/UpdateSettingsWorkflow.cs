using System.Threading.Tasks;
using ImageBirb.Core.Ports.Secondary;
using ImageBirb.Core.Workflows.Parameters;
using ImageBirb.Core.Workflows.Results;

namespace ImageBirb.Core.Workflows
{
    internal class UpdateSettingsWorkflow : Workflow<SettingParameters, WorkflowResult>
    {
        private readonly ISettingsManagementAdapter _settingsManagementAdapter;

        public UpdateSettingsWorkflow(ISettingsManagementAdapter settingsManagementAdapter)
        {
            _settingsManagementAdapter = settingsManagementAdapter;
        }

        protected override async Task<WorkflowResult> RunImpl(SettingParameters p)
        {
            await _settingsManagementAdapter.UpdateSettings(p.Setting);
            return new WorkflowResult(ResultState.Success);
        }
    }
}