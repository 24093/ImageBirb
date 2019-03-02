using ImageBirb.Core.BusinessObjects;
using System;
using System.Collections.Generic;

namespace ImageBirb.Core.Workflows.Results
{
    internal class ThumbnailsResult : WorkflowResult
    {
        public IList<Image> Thumbnails { get; }

        public ThumbnailsResult(ErrorCode errorCode = ErrorCode.None, Exception exception = null)
            : base(errorCode, exception)
        {
        }

        public ThumbnailsResult(IList<Image> thumbnails, ErrorCode errorCode = ErrorCode.None, Exception exception = null)
            : base(errorCode, exception)
        {
            Thumbnails = thumbnails;
        }
    }
}