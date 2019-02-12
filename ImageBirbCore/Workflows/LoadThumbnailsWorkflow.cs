using ImageBirb.Core.Ports.Secondary;
using ImageBirb.Core.Workflows.Parameters;
using ImageBirb.Core.Workflows.Results;
using System.Threading.Tasks;
using ImageBirb.Core.Ports.Secondary.DatabaseAdapter;

namespace ImageBirb.Core.Workflows
{
    internal class LoadThumbnailsWorkflow : Workflow<TagNamesParameters, ThumbnailsResult>
    {
        private readonly IDatabaseAdapter _databaseAdapter;

        public LoadThumbnailsWorkflow(IDatabaseAdapter databaseAdapter)
        {
            _databaseAdapter = databaseAdapter;
        }

        protected override async Task<ThumbnailsResult> RunImpl(TagNamesParameters p)
        {
            var thumbnails = await _databaseAdapter.ImageManagement.GetThumbnails(p.TagNames);
            return new ThumbnailsResult(ResultState.Success, thumbnails);
        }
    }
}