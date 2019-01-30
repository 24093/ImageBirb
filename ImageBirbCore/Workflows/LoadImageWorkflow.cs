using System.Threading.Tasks;
using ImageBirb.Core.Ports.Secondary;
using ImageBirb.Core.Workflows.Parameters;
using ImageBirb.Core.Workflows.Results;

namespace ImageBirb.Core.Workflows
{
    public class LoadImageWorkflow : Workflow<ImageIdParameters, ImageResult>
    {
        private readonly IDatabaseAdapter _databaseAdapter;

        public LoadImageWorkflow(IDatabaseAdapter databaseAdapter)
        {
            _databaseAdapter = databaseAdapter;
        }

        protected override async Task<ImageResult> Run(ImageIdParameters p)
        {
            var image = await _databaseAdapter.GetImage(p.ImageId);
            return new ImageResult(ResultState.Success, image);
        }
    }
}
