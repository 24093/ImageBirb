using GalaSoft.MvvmLight;
using ImageBirb.Core.Ports.Primary;
using ImageBirb.Core.Workflows.Results;
using System;
using System.Diagnostics;
using System.Windows;

namespace ImageBirb.ViewModels
{
    /// <summary>
    /// Base view model for anything that needs access to the backend.
    /// </summary>
    internal abstract class WorkflowViewModel : ViewModelBase
    {
        protected readonly IWorkflowAdapter WorkflowAdapter;

        protected WorkflowViewModel(IWorkflowAdapter workflowAdapter)
        {
            WorkflowAdapter = workflowAdapter;
        }

        private static void VerifyWorkflowResult(WorkflowResult result)
        {
            if (!result.IsSuccess)
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
}