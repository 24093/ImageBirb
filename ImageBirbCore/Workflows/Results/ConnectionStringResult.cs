using System;

namespace ImageBirb.Core.Workflows.Results
{
    internal class ConnectionStringResult : WorkflowResult
    {
        public string ConnectionString { get; }

        public ConnectionStringResult(ErrorCode errorCode = ErrorCode.None, Exception exception = null)
            : base(errorCode, exception)
        {
        }

        public ConnectionStringResult(string connectionString, ErrorCode errorCode = ErrorCode.None, Exception exception = null)
            : base(errorCode, exception)
        {
            ConnectionString = connectionString;
        }
    }
}