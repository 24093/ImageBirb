using ImageBirb.Core.BusinessObjects;
using System;

namespace ImageBirb.Core.Workflows.Results
{
    internal class SettingResult : WorkflowResult
    {
        public Setting Setting { get; }

        public SettingResult(ErrorCode errorCode = ErrorCode.None, Exception exception = null) 
            : base(errorCode, exception)
        {
        }

        public SettingResult(Setting setting, ErrorCode errorCode = ErrorCode.None, Exception exception = null)
            : base(errorCode, exception)
        {
            Setting = setting;
        }
    }
}