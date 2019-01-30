using System;
using System.Threading.Tasks;
using ImageBirb.Core.Workflows.Parameters;
using ImageBirb.Core.Workflows.Results;

namespace ImageBirb.Core.Workflows
{
    public abstract class Workflow<TParameters, TResult> : IWorkflow
        where TParameters : WorkflowParameters
        where TResult : WorkflowResult
    {
        public async Task<TResult> RunWorkflow(TParameters p)
        {
            try
            {
                return await Run(p);
            }
            catch (Exception ex)
            {
                return (TResult) Activator.CreateInstance(typeof(TResult), ResultState.Error, ErrorCode.WorkflowInternalError, ex);
            }
        }

        protected abstract Task<TResult> Run(TParameters p);
    }

    public abstract class Workflow<TResult> : IWorkflow
        where TResult : WorkflowResult
    {
        public async Task<TResult> Run()
        {
            try
            {
                return await RunImpl();
            }
            catch (Exception ex)
            {
                return (TResult)Activator.CreateInstance(typeof(TResult), ResultState.Error, ErrorCode.WorkflowInternalError, ex);
            }
        }

        protected abstract Task<TResult> RunImpl();
    }
}