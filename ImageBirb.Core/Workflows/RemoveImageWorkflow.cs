using ImageBirb.Core.Workflows.Parameters;
using ImageBirb.Core.Workflows.Results;
using System.Threading.Tasks;
using ImageBirb.Core.Ports.Secondary;

namespace ImageBirb.Core.Workflows
{
    internal class RemoveImageWorkflow : Workflow<ImageIdParameters, WorkflowResult>
    {
        private readonly IImageManagementAdapter _imageManagementAdapter;

        public RemoveImageWorkflow(IImageManagementAdapter imageManagementAdapter)
        {
            _imageManagementAdapter = imageManagementAdapter;
        }

        protected override async Task<WorkflowResult> RunImpl(ImageIdParameters p)
        {
            await _imageManagementAdapter.RemoveImage(p.ImageId);
            return new WorkflowResult();
        }
    }
}