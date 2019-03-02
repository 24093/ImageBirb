using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ImageBirb.Core.Workflows.Results
{
    internal class WorkflowResult
    {
        public bool IsSuccess { get; }
        
        [JsonConverter(typeof(StringEnumConverter))]
        public ErrorCode ErrorCode { get; }

        public Exception Exception { get; }

        public WorkflowResult(ErrorCode errorCode = ErrorCode.None, Exception exception = null)
        {
            IsSuccess = errorCode == ErrorCode.None;
            ErrorCode = errorCode;
            Exception = exception;
        }

        public static WorkflowResult CreateInvalidParameterResult(string parameterName)
        {
            return new WorkflowResult(
                ErrorCode.InvalidParameter,
                new ArgumentException("parameter can not be empty", parameterName));
        }
    }
}