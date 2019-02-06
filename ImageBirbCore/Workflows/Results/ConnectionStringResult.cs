using System;

namespace ImageBirb.Core.Workflows.Results
{
    public class ConnectionStringResult : WorkflowResult
    {
        public string ConnectionString { get; }

        public ConnectionStringResult(ResultState state, ErrorCode errorCode = ErrorCode.None, Exception exception = null)
            : base(state, errorCode, exception)
        {
        }

        public ConnectionStringResult(ResultState state, string connectionString, ErrorCode errorCode = ErrorCode.None, Exception exception = null)
            : base(state, errorCode, exception)
        {
            ConnectionString = connectionString;
        }
    }
}