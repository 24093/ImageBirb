using ImageBirb.Core.Ports.Secondary;
using ImageBirb.Core.Workflows.Parameters;
using ImageBirb.Core.Workflows.Results;
using System.Threading.Tasks;

namespace ImageBirb.Core.Workflows
{
    internal class AddTagWorkflow : Workflow<ImageIdTagParameters, WorkflowResult>
    {
        private readonly IDatabaseAdapter _databaseAdapter;

        public AddTagWorkflow(IDatabaseAdapter databaseAdapter)
        {
            _databaseAdapter = databaseAdapter;
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

            await _databaseAdapter.AddTag(p.ImageId, p.TagName);
            return new WorkflowResult(ResultState.Success);
        }
    }
}
