using System;
using System.Threading.Tasks;
using ImageBirb.Core.Ports.Primary;
using ImageBirb.Core.Workflows.Parameters;
using ImageBirb.Core.Workflows.Results;

namespace ImageBirb.Core.Workflows
{
    internal interface IWorkflowHost
    {
        event EventHandler<ProgressChangedEventArgs> ProgressChanged;

        Task<TResult> Run<TWorkflow, TParameters, TResult>(TParameters parameters)
            where TWorkflow : Workflow<TParameters, TResult>
            where TParameters : WorkflowParameters
            where TResult : WorkflowResult;

        Task<TResult> Run<TWorkflow, TResult>()
            where TWorkflow : Workflow<TResult>
            where TResult : WorkflowResult;

        TWorkflow Get<TWorkflow, TParameters, TResult>() 
            where TWorkflow : Workflow<TParameters, TResult>
            where TParameters : WorkflowParameters
            where TResult : WorkflowResult;
        
        TWorkflow Get<TWorkflow, TResult>()
            where TWorkflow : Workflow<TResult>
            where TResult : WorkflowResult;
    }
}