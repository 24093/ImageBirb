using ImageBirb.Core.Workflows.Results;
using System;

namespace ImageBirb.Core.Workflows
{
    internal class WorkflowException : Exception
    {
        public WorkflowException(ErrorCode errorCode, Exception innerException)
            : base(errorCode.ToString(), innerException)
        {
        }
    }
}