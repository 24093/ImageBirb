using ImageBirb.Core.Ports.Primary;
using ImageBirb.Core.Workflows.Results;
using System.Threading.Tasks;

namespace ImageBirb.ViewModels
{
    internal class SettingsViewModel : WorkflowViewModel
    {
        public string DatabaseFilename {get; private set; }

        public SettingsViewModel (IWorkflowAdapter workflowAdapter)
            : base (workflowAdapter)
        {
            Task.Run(async () => await WorkflowAdapter.ReadConnectionString()).ContinueWith(OnConnectionStringReceived);
        }

        private void OnConnectionStringReceived(Task<ConnectionStringResult> obj)
        {
            DatabaseFilename = obj.Result.ConnectionString;
        }
    }
}
