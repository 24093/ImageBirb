using System;
using System.Collections.Generic;

namespace ImageBirb.Core.Workflows.Results
{
    internal class AddImagesResult : WorkflowResult
    {
        public IList<string> IgnoredImages { get; }

        public AddImagesResult(ErrorCode errorCode = ErrorCode.None, Exception exception = null)
            : base(errorCode, exception)
        {
        }

        public AddImagesResult(IList<string> ignoredImages, ErrorCode errorCode = ErrorCode.None, Exception exception = null)
            : base(errorCode, exception)
        {
            IgnoredImages = ignoredImages;
        }
    }
}