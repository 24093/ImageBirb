using System.Threading.Tasks;
using ImageBirb.Core.Ports.Secondary;
using ImageBirb.Core.Workflows.Parameters;
using ImageBirb.Core.Workflows.Results;

namespace ImageBirb.Core.Workflows
{
    public class RemoveTagWorkflow : Workflow<ImageIdTagParameters, WorkflowResult>
    {
        private readonly IDatabaseAdapter _databaseAdapter;

        public RemoveTagWorkflow(IDatabaseAdapter databaseAdapter)
        {
            _databaseAdapter = databaseAdapter;
        }

        protected override async Task<WorkflowResult> Run(ImageIdTagParameters p)
        {
            await _databaseAdapter.RemoveTag(p.ImageId, p.TagName);
            return new WorkflowResult(ResultState.Success);
        }
    }
}
