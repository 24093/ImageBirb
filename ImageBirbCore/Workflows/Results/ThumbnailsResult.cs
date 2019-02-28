using System;
using System.Collections.Generic;
using ImageBirb.Core.Common;

namespace ImageBirb.Core.Workflows.Results
{
    public class ThumbnailsResult : WorkflowResult
    {
        public IList<Image> Thumbnails { get; }

        public ThumbnailsResult(ResultState state, ErrorCode errorCode = ErrorCode.None, Exception exception = null)
            : base(state, errorCode, exception)
        {
        }

        public ThumbnailsResult(ResultState state, IList<Image> thumbnails, ErrorCode errorCode = ErrorCode.None,
            Exception exception = null)
            : base(state, errorCode, exception)
        {
            Thumbnails = thumbnails;
        }
    }
}