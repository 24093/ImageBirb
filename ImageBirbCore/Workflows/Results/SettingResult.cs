using ImageBirb.Core.Common;
using System;

namespace ImageBirb.Core.Workflows.Results
{
    public class SettingResult : WorkflowResult
    {
        public Setting Setting { get; }

        public SettingResult(ResultState state, ErrorCode errorCode = ErrorCode.None, Exception exception = null) 
            : base(state, errorCode, exception)
        {
        }

        public SettingResult(ResultState state, Setting setting, ErrorCode errorCode = ErrorCode.None, Exception exception = null)
            : base(state, errorCode, exception)
        {
            Setting = setting;
        }
    }
}