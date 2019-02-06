using ImageBirb.Core.Workflows.Parameters;
using ImageBirb.Core.Workflows.Results;
using System;
using System.Threading.Tasks;

namespace ImageBirb.Core.Workflows
{
    internal abstract class Workflow<TParameters, TResult> : IWorkflow
        where TParameters : WorkflowParameters
        where TResult : WorkflowResult
    {
        public Type ParameterType => typeof(TParameters);

        public Type ResultType => typeof(TResult);

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

    internal abstract class Workflow<TResult> : IWorkflow
        where TResult : WorkflowResult
    {
        public Type ParameterType => null;

        public Type ResultType => typeof(TResult);
        
        public async Task<TResult> RunWorkflow()
        {
            try
            {
                return await Run();
            }
            catch (Exception ex)
            {
                return (TResult)Activator.CreateInstance(typeof(TResult), ResultState.Error, ErrorCode.WorkflowInternalError, ex);
            }
        }

        protected abstract Task<TResult> Run();
    }
}