using ImageBirb.Core.Common;
using System;

namespace ImageBirb.Core.Workflows.Results
{
    public class SettingsResult : WorkflowResult
    {
        public Settings Settings { get; }

        public SettingsResult(ResultState state, ErrorCode errorCode = ErrorCode.None, Exception exception = null)
            : base(state, errorCode, exception)
        {
        }

        public SettingsResult(ResultState state, Settings settings, ErrorCode errorCode = ErrorCode.None, Exception exception = null)
            : base(state, errorCode, exception)
        {
            Settings = settings;
        }
    }
}