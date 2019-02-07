using ImageBirb.Core.Ports.Primary;

namespace ImageBirb.ViewModels
{
    internal class SettingsViewModel : WorkflowViewModel
    {
        public string DatabaseFilename {get; private set; }

        public SettingsViewModel (IWorkflowAdapter workflows)
            : base (workflows)
        {
            Run(Workflows.ReadConnectionString(), r => DatabaseFilename = r.ConnectionString);
        }

    }
}
