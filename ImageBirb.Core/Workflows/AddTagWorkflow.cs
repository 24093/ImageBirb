using ImageBirb.Core.Workflows.Parameters;
using ImageBirb.Core.Workflows.Results;
using System.Threading.Tasks;
using ImageBirb.Core.Ports.Secondary;

namespace ImageBirb.Core.Workflows
{
    internal class AddTagWorkflow : Workflow<ImageIdTagParameters, WorkflowResult>
    {
        private readonly ITagManagementAdapter _tagManagementAdapter;

        public AddTagWorkflow(ITagManagementAdapter tagManagementAdapter)
        {
            _tagManagementAdapter = tagManagementAdapter;
        }

        protected override async Task<WorkflowResult> RunImpl(ImageIdTagParameters p)
        {
            if (string.IsNullOrEmpty(p.ImageId))
            {
                return WorkflowResult.CreateInvalidParameterResult(nameof(p.ImageId));
            }

            if (string.IsNullOrEmpty(p.TagName))
            {
                return WorkflowResult.CreateInvalidParameterResult(nameof(p.TagName));
            }

            await _tagManagementAdapter.AddTag(p.ImageId, p.TagName);
            return new WorkflowResult();
        }
    }
}
