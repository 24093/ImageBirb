using ImageBirb.Core.Workflows.Parameters;
using ImageBirb.Core.Workflows.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageBirb.Core.Ports.Primary;

namespace ImageBirb.Core.Workflows
{
    internal class WorkflowHost : IWorkflowHost, IDisposable
    {
        private readonly IList<IWorkflow> _workflows;

        public event EventHandler<ProgressChangedEventArgs> ProgressChanged;

        public WorkflowHost(IList<IWorkflow> workflows)
        {
            _workflows = workflows;

            foreach (var workflow in _workflows)
            {
                workflow.ProgressChanged += WorkflowOnProgressChanged;
            }
        }
        
        public async Task<TResult> Run<TWorkflow, TParameters, TResult>(TParameters parameters)
            where TWorkflow : Workflow<TParameters, TResult>
            where TParameters : WorkflowParameters
            where TResult : WorkflowResult
        {
            var workflow = Get<TWorkflow, TParameters, TResult>();
            var result = await workflow.Run(parameters);

            if (!result.IsSuccess)
            {
                throw new WorkflowException(result.ErrorCode, result.Exception);
            }

            return result;
        }

        public async Task<TResult> Run<TWorkflow, TResult>()
            where TWorkflow : Workflow<TResult>
            where TResult : WorkflowResult
        {
            var workflow = Get<TWorkflow, TResult>();
            var result = await workflow.Run();

            if (!result.IsSuccess)
            {
                throw new WorkflowException(result.ErrorCode, result.Exception);
            }

            return result;
        }

        public TWorkflow Get<TWorkflow, TParameters, TResult>() 
            where TWorkflow : Workflow<TParameters, TResult> 
            where TParameters : WorkflowParameters 
            where TResult : WorkflowResult
        {
            return _workflows.OfType<TWorkflow>().Single();
        }

        public TWorkflow Get<TWorkflow, TResult>() 
            where TWorkflow : Workflow<TResult> 
            where TResult : WorkflowResult
        {
            return _workflows.OfType<TWorkflow>().Single();
        }

        public void Dispose()
        {
            foreach (var workflow in _workflows)
            {
                workflow.ProgressChanged -= WorkflowOnProgressChanged;
            }
        }

        private void WorkflowOnProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressChanged?.Invoke(sender, e);
        }
    }
}