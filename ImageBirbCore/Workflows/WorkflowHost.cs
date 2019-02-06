using ImageBirb.Core.Workflows.Parameters;
using ImageBirb.Core.Workflows.Results;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageBirb.Core.Workflows
{
    internal class WorkflowHost
    {
        private readonly IList<IWorkflow> _workflows;

        public WorkflowHost(IList<IWorkflow> workflows)
        {
            _workflows = workflows;
        }
        
        public async Task<TResult> Run<TWorkflow, TParameters, TResult>(TParameters parameters)
            where TWorkflow : Workflow<TParameters, TResult>
            where TParameters : WorkflowParameters
            where TResult : WorkflowResult
        {
            var workflow = _workflows.OfType<TWorkflow>().Single();
            var result = await workflow.RunWorkflow(parameters);
            return result;
        }

        public async Task<TResult> Run<TWorkflow, TResult>()
            where TWorkflow : Workflow<TResult>
            where TResult : WorkflowResult
        {
            var workflow = _workflows.OfType<TWorkflow>().Single();
            var result = await workflow.RunWorkflow();
            return result;
        }
    }
}