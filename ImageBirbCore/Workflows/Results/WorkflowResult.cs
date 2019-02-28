using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ImageBirb.Core.Workflows.Results
{
    public class WorkflowResult
    {
        public bool IsSuccess => State == ResultState.Success;
        
        [JsonConverter(typeof(StringEnumConverter))]
        public ResultState State { get; }
        
        [JsonConverter(typeof(StringEnumConverter))]
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
                ResultState.Failure, 
                ErrorCode.InvalidParameter,
                new ArgumentException("parameter can not be empty", parameterName));
        }
    }
}