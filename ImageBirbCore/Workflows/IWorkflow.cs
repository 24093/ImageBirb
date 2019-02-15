using System;
using ImageBirb.Core.Common;

namespace ImageBirb.Core.Workflows
{
    internal interface IWorkflow
    {
        event EventHandler<ProgressChangedEventArgs> ProgressChanged;
    }
}