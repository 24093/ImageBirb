﻿using System.Threading.Tasks;
using ImageBirb.Core.Ports.Secondary;
using ImageBirb.Core.Workflows.Parameters;
using ImageBirb.Core.Workflows.Results;

namespace ImageBirb.Core.Workflows
{
    public class RemoveImageWorkflow : Workflow<ImageIdParameters, WorkflowResult>
    {
        private readonly IDatabaseAdapter _databaseAdapter;

        public RemoveImageWorkflow(IDatabaseAdapter databaseAdapter)
        {
            _databaseAdapter = databaseAdapter;
        }

        protected override async Task<WorkflowResult> Run(ImageIdParameters p)
        {
            await _databaseAdapter.RemoveImage(p.ImageId);
            return new WorkflowResult(ResultState.Success);
        }
    }
}