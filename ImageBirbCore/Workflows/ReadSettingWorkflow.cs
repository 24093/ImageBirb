using ImageBirb.Core.Ports.Secondary;
using ImageBirb.Core.Workflows.Parameters;
using ImageBirb.Core.Workflows.Results;
using System.Threading.Tasks;
using ImageBirb.Core.Common;

namespace ImageBirb.Core.Workflows
{
    internal class ReadSettingWorkflow : Workflow<KeyParameters, SettingResult>
    {
        private readonly ISettingsManagementAdapter _settingsManagementAdapter;

        public ReadSettingWorkflow(ISettingsManagementAdapter settingsManagementAdapter)
        {
            _settingsManagementAdapter = settingsManagementAdapter;
        }

        protected override async Task<SettingResult> RunImpl(KeyParameters p)
        {
            var setting = await _settingsManagementAdapter.GetSetting(p.Key) ?? new Setting {Key = p.Key};
            return new SettingResult(ResultState.Success, setting);
        }
    }
}