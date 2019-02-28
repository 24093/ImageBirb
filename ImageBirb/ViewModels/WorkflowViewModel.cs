using GalaSoft.MvvmLight;
using ImageBirb.Core.Common;
using ImageBirb.Core.Ports.Primary;
using ImageBirb.Core.Workflows.Results;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

namespace ImageBirb.ViewModels
{
    /// <summary>
    /// Base view model for anything that needs access to the backend.
    /// </summary>
    internal abstract class WorkflowViewModel : ViewModelBase, IDisposable
    {
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        protected readonly IWorkflowAdapter Workflows;

        protected WorkflowViewModel(IWorkflowAdapter workflows)
        {
            Workflows = workflows;
            Workflows.ProgressChanged += WorkflowsOnProgressChanged;
        }

        /// <summary>
        /// Default empty implementation for progress changed event handler.
        /// Override in concrete view model class to use it.
        /// </summary>
        protected virtual void WorkflowsOnProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        }

        /// <summary>
        /// Run a workflow. This is the default approach to run a workflow.
        /// </summary>
        /// <typeparam name="TResult">Workflow result type.</typeparam>
        /// <param name="workflow">Workflow to run.</param>
        /// <param name="onSuccess">Action to be called if workflow was successfully run.</param>
        /// <param name="onFailure">Action to be called if workflow didn't run successfully.</param>
        protected async Task RunAsync<TResult>(Task<TResult> workflow, Action<TResult> onSuccess = null,
            Action<TResult> onFailure = null)
            where TResult : WorkflowResult
        {
            await workflow.ContinueWith(t =>
            {
                if (t.Result.IsSuccess)
                {
                    onSuccess?.Invoke(t.Result);
                }
                else
                {
                    (onFailure ?? DefaultWorkflowOnError).Invoke(t.Result);
                }
            });
        }

        /// <summary>
        /// Run a workflow. Use this to modify data in the UI thread in the result actions.
        /// </summary>
        /// <typeparam name="TResult">Workflow result type.</typeparam>
        /// <param name="workflow">Workflow to run.</param>
        /// <param name="onSuccess">Action to be called if workflow was successfully run.</param>
        /// <param name="onFailure">Action to be called if workflow didn't run successfully.</param>
        protected async Task RunAsyncDispatch<TResult>(Task<TResult> workflow, Action<TResult> onSuccess = null,
            Action<TResult> onFailure = null)
            where TResult : WorkflowResult
        {
            Action<TResult> onSuccessDispatched = null;
            Action<TResult> onFailureDispatched = r => Application.Current.Dispatcher.Invoke(() => (onFailure ?? DefaultWorkflowOnError)(r));
            
            if (onSuccess != null)
            {
                onSuccessDispatched = r => Application.Current.Dispatcher.Invoke(() => onSuccess(r));
            }

            await RunAsync(workflow, onSuccessDispatched, onFailureDispatched);
        }
        
        /// <summary>
        /// The default error handler. Shows a windows message box in debug mode.
        /// </summary>
        /// <typeparam name="TResult">Workflow result type.</typeparam>
        /// <param name="result">The error result.</param>
        private void DefaultWorkflowOnError<TResult>(TResult result)
            where TResult : WorkflowResult
        {
            Log.Error(result.Exception);

            if (Debugger.IsAttached)
            {
                var message = (result.Exception?.Message + Environment.NewLine + Environment.NewLine +
                               result.Exception?.StackTrace).Trim();
                MessageBox.Show(message, result.ErrorCode.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            }

            WorkflowOnError(result);
        }

        /// <summary>
        /// Custom error handler to be implemented in concrete classes.
        /// </summary>
        /// <typeparam name="TResult">Workflow result type.</typeparam>
        /// <param name="result">The error result.</param>
        protected virtual void WorkflowOnError<TResult>(TResult result)
            where TResult : WorkflowResult
        {
        }

        public void Dispose()
        {
            Workflows.ProgressChanged -= WorkflowsOnProgressChanged;
        }
    }
}