using GalaSoft.MvvmLight;
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
    internal abstract class WorkflowViewModel : ViewModelBase
    {
        protected readonly IWorkflowAdapter Workflows;

        protected WorkflowViewModel(IWorkflowAdapter workflows)
        {
            Workflows = workflows;
        }

        protected static async Task RunAsync<TResult>(Task<TResult> workflow, Action<TResult> onSuccess = null, Action<TResult> onFailure = null)
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
                    (onFailure ?? DefaultOnFailure).Invoke(t.Result);
                }
            });
        }

        protected static async Task RunAsyncDispatch<TResult>(Task<TResult> workflow, Action<TResult> onSuccess = null, Action<TResult> onFailure = null)
            where TResult : WorkflowResult
        {
            var onSuccessDispatched = onSuccess;
            var onFailureDispatched = (onFailure ?? DefaultOnFailure);

            if (onSuccess != null)
            {
                onSuccessDispatched = r => Application.Current.Dispatcher.Invoke(() => onSuccess(r));
            }

            if (onFailure != null)
            {
                onFailureDispatched = r => Application.Current.Dispatcher.Invoke(() => onFailure(r));
            }

            await RunAsync(workflow, onSuccessDispatched, onFailureDispatched);
        }

        protected static void Run<TResult>(Task<TResult> workflow, Action<TResult> onSuccess = null, Action<TResult> onFailure = null)
            where TResult : WorkflowResult
        {
            Task.Run(async () => await workflow).ContinueWith(t =>
            {
                if (t.Result.IsSuccess)
                {
                    onSuccess?.Invoke(t.Result);
                }
                else
                {
                    (onFailure ?? DefaultOnFailure)?.Invoke(t.Result);
                }
            });
        }
        
        private static void DefaultOnFailure<TResult>(TResult result)
            where TResult : WorkflowResult
        {
            var message = result.Exception?.Message;

            if (Debugger.IsAttached)
            {
                message += Environment.NewLine + Environment.NewLine + result.Exception?.StackTrace;
            }

            message = message?.Trim() ?? string.Empty;

            MessageBox.Show(message, result.ErrorCode.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}