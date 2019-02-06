using ImageBirb.Core.Ports.Secondary;
using ImageBirb.Core.Workflows.Results;
using System.Threading.Tasks;

namespace ImageBirb.Core.Workflows
{
    internal class LoadThumbnailsWorkflow : Workflow<ThumbnailsResult>
    {
        private readonly IDatabaseAdapter _databaseAdapter;

        public LoadThumbnailsWorkflow(IDatabaseAdapter databaseAdapter)
        {
            _databaseAdapter = databaseAdapter;
        }

        protected override async Task<ThumbnailsResult> Run()
        {
            var thumbnails = await _databaseAdapter.GetThumbnails();
            return new ThumbnailsResult(ResultState.Success, thumbnails);
        }
    }
}