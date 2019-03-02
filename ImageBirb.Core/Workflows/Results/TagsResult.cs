using ImageBirb.Core.BusinessObjects;
using System;
using System.Collections.Generic;

namespace ImageBirb.Core.Workflows.Results
{
    internal class TagsResult : WorkflowResult
    {
        public IList<Tag> Tags { get; }

        public TagsResult(ErrorCode errorCode = ErrorCode.None, Exception exception = null)
            : base(errorCode, exception)
        {
        }

        public TagsResult(IList<Tag> tags, ErrorCode errorCode = ErrorCode.None, Exception exception = null)
            : base(errorCode, exception)
        {
            Tags = tags;
        }
    }
}
