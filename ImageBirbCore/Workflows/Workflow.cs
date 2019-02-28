using ImageBirb.Core.Common;
using ImageBirb.Core.Workflows.Parameters;
using ImageBirb.Core.Workflows.Results;
using Newtonsoft.Json;
using NLog;
using System;
using System.Threading.Tasks;

namespace ImageBirb.Core.Workflows
{
    internal abstract class Workflow : IWorkflow
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public abstract event EventHandler<ProgressChangedEventArgs> ProgressChanged;
    }

    internal abstract class Workflow<TParameters, TResult> : Workflow
        where TParameters : WorkflowParameters
        where TResult : WorkflowResult
    {
        public override event EventHandler<ProgressChangedEventArgs> ProgressChanged;
        
        public async Task<TResult> Run(TParameters p)
        {
            try
            {
                Logger.Trace("running workflow {0} with parameters {1}", GetType().Name, JsonConvert.SerializeObject(p));
                var result = await RunImpl(p);
                Logger.Trace("workflow {0} with parameters {1} returned result {2}", GetType().Name, JsonConvert.SerializeObject(p), JsonConvert.SerializeObject(result));
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "workflow threw an exception");
                return (TResult) Activator.CreateInstance(typeof(TResult), ResultState.Failure, ErrorCode.WorkflowInternalError, ex);
            }
        }

        protected abstract Task<TResult> RunImpl(TParameters p);

        protected void RaiseProgressChanged(ProgressType type, int progress, int max = 100)
        {
            Logger.Trace("workflow {0} changed progress to {1} of {2}", GetType().Name, progress, max);
            ProgressChanged?.Invoke(this, new ProgressChangedEventArgs(type, progress, max));
        }
    }

    internal abstract class Workflow<TResult> : Workflow
        where TResult : WorkflowResult
    {
        public override event EventHandler<ProgressChangedEventArgs> ProgressChanged;

        public async Task<TResult> Run()
        {
            try
            {
                Logger.Trace("running workflow {0}", GetType().Name);
                var result = await RunImpl();
                Logger.Trace("workflow {0} returned result {1}", GetType().Name, JsonConvert.SerializeObject(result));
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "workflow threw an exception");
                return (TResult)Activator.CreateInstance(typeof(TResult), ResultState.Failure, ErrorCode.WorkflowInternalError, ex);
            }
        }

        protected abstract Task<TResult> RunImpl();

        protected void RaiseProgressChanged(ProgressType type, int progress, int max = 100)
        {
            Logger.Trace("workflow {0} changed progress to {1} of {2}", GetType().Name, progress, max);
            ProgressChanged?.Invoke(this, new ProgressChangedEventArgs(type, progress, max));
        }
    }
}