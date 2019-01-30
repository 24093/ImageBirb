using System;
using System.Collections.Generic;
using ImageBirb.Core.Common;

namespace ImageBirb.Core.Workflows.Results
{
    public class TagsResult : WorkflowResult
    {
        public IEnumerable<Tag> Tags { get; }

        public TagsResult(ResultState state, ErrorCode errorCode = ErrorCode.None, Exception exception = null)
            : base(state, errorCode, exception)
        {
        }

        public TagsResult(ResultState state, IEnumerable<Tag> tags, ErrorCode errorCode = ErrorCode.None, Exception exception = null)
            : base(state, errorCode, exception)
        {
            Tags = tags;
        }
    }
}
