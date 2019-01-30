using System;

namespace ImageBirb.Core.Workflows.Results
{
    public class IsBitmapImageResult : WorkflowResult
    {
        public bool IsBitmapImage { get; }

        public IsBitmapImageResult(ResultState state, ErrorCode errorCode = ErrorCode.None, Exception exception = null) 
            : base(state, errorCode, exception)
        {
        }

        public IsBitmapImageResult(ResultState state, bool isBitmapImage, ErrorCode errorCode = ErrorCode.None, Exception exception = null) 
            : base(state, errorCode, exception)
        {
            IsBitmapImage = isBitmapImage;
        }
    }
}