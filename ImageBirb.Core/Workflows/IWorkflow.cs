using System;
using ImageBirb.Core.Ports.Primary;

namespace ImageBirb.Core.Workflows
{
    internal interface IWorkflow
    {
        event EventHandler<ProgressChangedEventArgs> ProgressChanged;
    }
}