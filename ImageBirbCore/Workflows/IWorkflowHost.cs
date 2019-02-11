using System.Threading.Tasks;
using ImageBirb.Core.Workflows.Parameters;
using ImageBirb.Core.Workflows.Results;

namespace ImageBirb.Core.Workflows
{
    internal interface IWorkflowHost
    {
        Task<TResult> Run<TWorkflow, TParameters, TResult>(TParameters parameters)
            where TWorkflow : Workflow<TParameters, TResult>
            where TParameters : WorkflowParameters
            where TResult : WorkflowResult;

        Task<TResult> Run<TWorkflow, TResult>()
            where TWorkflow : Workflow<TResult>
            where TResult : WorkflowResult;
    }
}