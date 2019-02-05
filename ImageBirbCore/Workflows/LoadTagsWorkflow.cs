using System.Linq;
using System.Threading.Tasks;
using ImageBirb.Core.Common;
using ImageBirb.Core.Ports.Secondary;
using ImageBirb.Core.Workflows.Results;

namespace ImageBirb.Core.Workflows
{
    public class LoadTagsWorkflow : Workflow<TagsResult>
    {
        private readonly IDatabaseAdapter _databaseAdapter;

        public LoadTagsWorkflow(IDatabaseAdapter databaseAdapter)
        {
            _databaseAdapter = databaseAdapter;
        }

        protected override async Task<TagsResult> Run()
        {
            var tags = await _databaseAdapter.GetTags();
            var tagItems = tags.Select(t => new Tag(t.Key, t.Value)).ToList();

            // Sort tags by count, highest value first.
            tagItems.Sort((x, y) => -1 * x.Count.CompareTo(y.Count));

            return new TagsResult(ResultState.Success, tagItems);
        }
    }
}
