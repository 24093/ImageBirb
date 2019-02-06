using System.Linq;
using System.Threading.Tasks;
using ImageBirb.Core.Ports.Secondary;
using ImageBirb.Core.Workflows.Parameters;
using ImageBirb.Core.Workflows.Results;

namespace ImageBirb.Core.Workflows
{
    internal class LoadThumbnailsByTagsWorkflow : Workflow<TagNamesParameters, ThumbnailsResult>
    {
        private readonly IDatabaseAdapter _databaseAdapter;

        public LoadThumbnailsByTagsWorkflow(IDatabaseAdapter databaseAdapter)
        {
            _databaseAdapter = databaseAdapter;
        }

        protected override async Task<ThumbnailsResult> Run(TagNamesParameters p)
        {
            var thumbnails = await _databaseAdapter.GetThumbnails(x => x.Tags.Intersect(p.TagNames).Any());
            return new ThumbnailsResult(ResultState.Success, thumbnails);
        }
    }
}