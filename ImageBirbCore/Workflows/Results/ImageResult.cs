using System;
using ImageBirb.Core.Common;

namespace ImageBirb.Core.Workflows.Results
{
    public class ImageResult : WorkflowResult
    {
        public Image Image {get; }

        public ImageResult(ResultState state, ErrorCode errorCode = ErrorCode.None, Exception exception = null) 
            : base(state, errorCode, exception)
        {
        }

        public ImageResult(ResultState state, Image image, ErrorCode errorCode = ErrorCode.None, Exception exception = null) 
            : base(state, errorCode, exception)
        {
            Image = image;
        }
    }
}