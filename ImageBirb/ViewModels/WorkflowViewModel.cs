using GalaSoft.MvvmLight;
using ImageBirb.Core.Ports.Primary;
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
            Action<Exception> onFailure = null)
        {
            try
            {
                await workflow.ContinueWith(t => onSuccess?.Invoke(t.Result));
            }
            catch (Exception ex)
            {
                (onFailure ?? DefaultWorkflowOnError).Invoke(ex);
            }
        }

        /// <summary>
        /// Run a workflow. This is the default approach to run a workflow.
        /// </summary>
        /// <param name="workflow">Workflow to run.</param>
        /// <param name="onSuccess">Action to be called if workflow was successfully run.</param>
        /// <param name="onFailure">Action to be called if workflow didn't run successfully.</param>
        protected async Task RunAsync(Task workflow, Action onSuccess = null,
            Action<Exception> onFailure = null)
        {
            try
            {
                await workflow.ContinueWith(t => onSuccess?.Invoke());
            }
            catch (Exception ex)
            {
                (onFailure ?? DefaultWorkflowOnError).Invoke(ex);
            }
        }

        /// <summary>
        /// Run a workflow. Use this to modify data in the UI thread in the result actions.
        /// </summary>
        /// <typeparam name="TResult">Workflow result type.</typeparam>
        /// <param name="workflow">Workflow to run.</param>
        /// <param name="onSuccess">Action to be called if workflow was successfully run.</param>
        /// <param name="onFailure">Action to be called if workflow didn't run successfully.</param>
        protected async Task RunAsyncDispatch<TResult>(Task<TResult> workflow, Action<TResult> onSuccess = null,
            Action<Exception> onFailure = null)
        {
            Action<TResult> onSuccessDispatched = null;
            Action<Exception> onFailureDispatched = r => Application.Current.Dispatcher.Invoke(() => (onFailure ?? DefaultWorkflowOnError)(r));
            
            if (onSuccess != null)
            {
                onSuccessDispatched = r => Application.Current.Dispatcher.Invoke(() => onSuccess(r));
            }

            await RunAsync(workflow, onSuccessDispatched, onFailureDispatched);
        }
        
        /// <summary>
        /// The default error handler. Shows a windows message box in debug mode.
        /// </summary>
        /// <param name="ex">The exception thrown by the workflow host.</param>
        private void DefaultWorkflowOnError(Exception ex)
        {
            Log.Error(ex);

            if (Debugger.IsAttached)
            {
                var message = (ex?.Message + Environment.NewLine + Environment.NewLine + ex?.StackTrace).Trim();
                MessageBox.Show(message, ex?.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }

            WorkflowOnError(ex);
        }

        /// <summary>
        /// Custom error handler to be implemented in concrete classes.
        /// </summary>
        /// <param name="ex">The exception thrown by the workflow host.</param>
        protected virtual void WorkflowOnError(Exception ex)
        {
        }

        public void Dispose()
        {
            Workflows.ProgressChanged -= WorkflowsOnProgressChanged;
        }
    }
}