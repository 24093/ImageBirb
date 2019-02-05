using ImageBirb.Core.Common;
using ImageBirb.Core.Ports.Primary;

namespace ImageBirb.ViewModels
{
    internal class SettingsViewModel : WorkflowViewModel
    {
        private Settings _settings;

        private string _databaseFilename;

        public string DatabaseFilename
        {
            get => _databaseFilename;
            set => Set(ref _databaseFilename, value);
        }

        public SettingsViewModel (IWorkflowAdapter workflowAdapter)
            : base (workflowAdapter)
        {
            
        }
    }
}
