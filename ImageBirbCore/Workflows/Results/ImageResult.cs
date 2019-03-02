using ImageBirb.Core.BusinessObjects;
using System;

namespace ImageBirb.Core.Workflows.Results
{
    internal class ImageResult : WorkflowResult
    {
        public Image Image {get; }

        public ImageResult(ErrorCode errorCode = ErrorCode.None, Exception exception = null) 
            : base(errorCode, exception)
        {
        }

        public ImageResult(Image image, ErrorCode errorCode = ErrorCode.None, Exception exception = null) 
            : base(errorCode, exception)
        {
            Image = image;
        }
    }
}