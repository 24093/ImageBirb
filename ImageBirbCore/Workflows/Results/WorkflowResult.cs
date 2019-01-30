using System;

namespace ImageBirb.Core.Workflows.Results
{
    public class WorkflowResult
    {
        public bool IsSuccess => State == ResultState.Success;

        public ResultState State { get; }

        public ErrorCode ErrorCode { get; }

        public Exception Exception { get; }

        public WorkflowResult(ResultState state, ErrorCode errorCode = ErrorCode.None, Exception exception = null)
        {
            State = state;
            ErrorCode = errorCode;
            Exception = exception;
        }

        public static WorkflowResult CreateInvalidParameterResult(string parameterName)
        {
            return new WorkflowResult(
                ResultState.Error, 
                ErrorCode.InvalidParameter,
                new ArgumentException("parameter can not be empty", parameterName));
        }
    }
}