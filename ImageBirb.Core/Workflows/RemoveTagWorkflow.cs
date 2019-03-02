using ImageBirb.Core.Workflows.Parameters;
using ImageBirb.Core.Workflows.Results;
using System.Threading.Tasks;
using ImageBirb.Core.Ports.Secondary;

namespace ImageBirb.Core.Workflows
{
    internal class RemoveTagWorkflow : Workflow<ImageIdTagParameters, WorkflowResult>
    {
        private readonly ITagManagementAdapter _tagManagementAdapter;

        public RemoveTagWorkflow(ITagManagementAdapter tagManagementAdapter)
        {
            _tagManagementAdapter = tagManagementAdapter;
        }

        protected override async Task<WorkflowResult> RunImpl(ImageIdTagParameters p)
        {
            await _tagManagementAdapter.RemoveTag(p.ImageId, p.TagName);
            return new WorkflowResult();
        }
    }
}
