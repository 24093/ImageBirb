using ImageBirb.Core.Workflows.Parameters;
using ImageBirb.Core.Workflows.Results;
using System;
using System.Threading.Tasks;
using ImageBirb.Core.Common;

namespace ImageBirb.Core.Workflows
{
    internal abstract class Workflow<TParameters, TResult> : IWorkflow
        where TParameters : WorkflowParameters
        where TResult : WorkflowResult
    {
        public event EventHandler<ProgressChangedEventArgs> ProgressChanged;
        
        public async Task<TResult> Run(TParameters p)
        {
            try
            {
                return await RunImpl(p);
            }
            catch (Exception ex)
            {
                return (TResult) Activator.CreateInstance(typeof(TResult), ResultState.Error, ErrorCode.WorkflowInternalError, ex);
            }
        }

        protected abstract Task<TResult> RunImpl(TParameters p);

        protected void RaiseProgressChanged(ProgressType type, int progress, int max = 100)
        {
            ProgressChanged?.Invoke(this, new ProgressChangedEventArgs(type, progress, max));
        }
    }

    internal abstract class Workflow<TResult> : IWorkflow
        where TResult : WorkflowResult
    {
        public event EventHandler<ProgressChangedEventArgs> ProgressChanged;

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

        protected void RaiseProgressChanged(ProgressType type, int progress, int max = 100)
        {
            ProgressChanged?.Invoke(this, new ProgressChangedEventArgs(type, progress, max));
        }
    }
}