using System;
using System.Collections.Generic;
using ImageBirb.Core.Common;

namespace ImageBirb.Core.Workflows.Results
{
    public class AddImagesResult : WorkflowResult
    {
        public IList<string> IgnoredImages { get; }

        public AddImagesResult(ResultState state, ErrorCode errorCode = ErrorCode.None, Exception exception = null)
            : base(state, errorCode, exception)
        {
        }

        public AddImagesResult(ResultState state, IList<string> ignoredImages, ErrorCode errorCode = ErrorCode.None,
            Exception exception = null)
            : base(state, errorCode, exception)
        {
            IgnoredImages = ignoredImages;
        }
    }
}