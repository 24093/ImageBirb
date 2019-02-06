using System;

namespace ImageBirb.Core.Workflows
{
    internal interface IWorkflow
    {
        Type ParameterType { get; }
    
        Type ResultType { get; }
    }
}