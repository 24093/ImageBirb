using ImageBirb.Core.Ports.Secondary;
using ImageBirb.Core.Workflows.Parameters;
using ImageBirb.Core.Workflows.Results;
using System.Threading.Tasks;

namespace ImageBirb.Core.Workflows
{
    internal class ReadSettingWorkflow : Workflow<SettingParameters, SettingResult>
    {
        private readonly ISettingsManagementAdapter _settingsManagementAdapter;

        public ReadSettingWorkflow(ISettingsManagementAdapter settingsManagementAdapter)
        {
            _settingsManagementAdapter = settingsManagementAdapter;
        }

        protected override async Task<SettingResult> RunImpl(SettingParameters p)
        {
            var setting = await _settingsManagementAdapter.GetSetting(p.Setting.Key);
            return new SettingResult(setting ?? p.Setting);
        }
    }
}