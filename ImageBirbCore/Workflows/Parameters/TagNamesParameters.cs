﻿using System.Collections.Generic;

namespace ImageBirb.Core.Workflows.Parameters
{
    public class TagNamesParameters : WorkflowParameters
    {
        public List<string> TagNames { get; }

        public TagNamesParameters(List<string> tagNames)
        {
            TagNames = tagNames;
        }
    }
}